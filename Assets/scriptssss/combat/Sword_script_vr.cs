using UnityEngine;

public class Sword_script_vr : MonoBehaviour

/*{
    public int damage = 1;
    public float minSwingVelocity = 1.5f;

    private Vector3 lastPos;
    private float swingSpeed;

    void Update()
    {
        // Calculate swing speed
        swingSpeed = (transform.position - lastPos).magnitude / Time.deltaTime;
        lastPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only damage if swinging fast enough
        if (swingSpeed < minSwingVelocity) return;

        if (other.TryGetComponent<actor>(out actor enemy))
        {
            enemy.TakeDamage(damage);
            Debug.Log("Hit enemy with swing!");
        }
    }
}
*/

{
    public int damage = 1;
    public float minSwingVelocity = 1.5f;
    public float hitCooldown = 0.2f;

    public sword_fx sword_Fx;

    private Vector3 lastPos;
    private float swingSpeed;
    private float lastHitTime;



    void Update()
    {
        swingSpeed = (transform.position - lastPos).magnitude / Time.deltaTime;
        lastPos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ⛔ prevent spam hits
        if (Time.time - lastHitTime < hitCooldown) return;

        // ⛔ ignore weak swings
        if (swingSpeed < minSwingVelocity) return;

        // 🎯 check tag
        if (collision.gameObject.CompareTag("hittable"))
        {
            lastHitTime = Time.time;

            // 💥 apply damage
            if (collision.gameObject.TryGetComponent<actor>(out actor enemy))
            {
                enemy.TakeDamage(damage);
                sword_Fx.SpawnVFX(collision);
                sword_Fx.SpawnScratch(collision);
                sword_Fx.PlayHitEffects(collision);
            }

            Debug.Log("Solid hit!");
        }
    }
}