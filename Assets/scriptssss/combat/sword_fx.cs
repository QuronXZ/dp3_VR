using UnityEngine;
using UnityEngine.VFX;

using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class sword_fx : MonoBehaviour
{
    [Header("Scratch Effect")]
    public GameObject hitVFXPrefab;
    public GameObject scratchPrefab, bloodprefab;
    public float minSwingVelocity = 1.5f;

    [Header("Hit Filtering")]
    public string hittableTag = "hittable"; 
    public float scratcheffectdestroyafter = 5f;
    public float bloodeffectdestroyafter = 5f;

    [Header("Cooldown")]
    public float effectCooldown = 0.1f;

    private Vector3 lastPos;
    private float swingSpeed;
    private float lastEffectTime;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hitSound;

    [Header("Haptics")]
    public XRGrabInteractable grabInteractable;
    public float hapticAmplitude = 0.7f;
    public float hapticDuration = 0.1f;

    void Update()
    {
        // Calculate swing speed
        swingSpeed = (transform.position - lastPos).magnitude / Time.deltaTime;
        lastPos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ⛔ cooldown
        if (Time.time - lastEffectTime < effectCooldown) return;

        // ⛔ weak swing
        if (swingSpeed < minSwingVelocity) return;

        // 🎯 tag filter
        if (!collision.gameObject.CompareTag(hittableTag)) return;

        lastEffectTime = Time.time;

        //SpawnScratch(collision);
    }

    public void SpawnScratch(Collision collision)
    {
        if (hitVFXPrefab == null) return;

        ContactPoint contact = collision.contacts[0];

        Vector3 pos = contact.point;
        Quaternion rot = Quaternion.LookRotation(-contact.normal);

        GameObject vfx = Instantiate(scratchPrefab, pos, rot);

        // Optional: parent it so it follows moving objects
        vfx.transform.SetParent(collision.transform);

        // If it has VisualEffect component, play it
        var visualEffect = vfx.GetComponent<VisualEffect>();
        if (visualEffect != null)
        {
            visualEffect.Play();
        }

        // Cleanup
        Destroy(vfx, scratcheffectdestroyafter);
    }

    public void Spawnblood(Collision collision)
    {
        if (hitVFXPrefab == null) return;

        ContactPoint contact = collision.contacts[0];

        Vector3 pos = contact.point;
        Quaternion rot = Quaternion.LookRotation(-contact.normal);

        GameObject vfx = Instantiate(bloodprefab, pos, rot);

        // Optional: parent it so it follows moving objects
        vfx.transform.SetParent(collision.transform);

        // If it has VisualEffect component, play it
        var visualEffect = vfx.GetComponent<VisualEffect>();
        if (visualEffect != null)
        {
            visualEffect.Play();
        }

        // Cleanup
        Destroy(vfx, bloodeffectdestroyafter);
    }
    /*    {
            if (scratchPrefab == null) return;

            ContactPoint contact = collision.contacts[0];

            Vector3 pos = (transform.position - lastPos).normalized;
            //
            //
            //
            Quaternion rot = Quaternion.LookRotation(contact.normal, pos);

            //Vector3 pos = contact.point + contact.normal * 0.01f;
            //Quaternion rot = Quaternion.LookRotation(contact.normal);

            GameObject scratch = Instantiate(scratchPrefab, pos, rot);

            // Stick to object
            scratch.transform.SetParent(collision.transform);

            // Randomize (adds realism)
            scratch.transform.Rotate(0, 0, Random.Range(0, 360));
            float scale = Random.Range(0.8f, 1.2f);
            scratch.transform.localScale *= scale;

            //Destroy(scratch, 10f);

            // ⏳ fade & cleanup
            StartCoroutine(FadeAndDestroy(scratch, lastEffectTime));

        }

        IEnumerator FadeAndDestroy(GameObject obj, float duration)
        {
            var rend = obj.GetComponent<Renderer>();
            if (rend == null)
            {
                Destroy(obj, duration);
                yield break;
            }

            // instance material (so we don't edit the shared one)
            Material mat = rend.material;

            Color start = mat.color;
            float t = 0f;

            while (t < duration)
            {
                float a = Mathf.Lerp(1f, 0f, t / duration);
                Color c = start;
                c.a = a;
                mat.color = c;

                t += Time.deltaTime;
                yield return null;
            }

            Destroy(obj);
        }*/



    public void SpawnVFX(Collision collision)
    {
        if (hitVFXPrefab == null) return;

        ContactPoint contact = collision.contacts[0];

        Vector3 pos = contact.point;
        Quaternion rot = Quaternion.LookRotation(contact.normal);

        GameObject vfx = Instantiate(hitVFXPrefab, pos, rot);

        // Optional: parent it so it follows moving objects
        vfx.transform.SetParent(collision.transform);

        // If it has VisualEffect component, play it
        var visualEffect = vfx.GetComponent<VisualEffect>();
        if (visualEffect != null)
        {
            visualEffect.Play();
        }

        // Cleanup
        Destroy(vfx, 2f);
    }

    public void PlayHitEffects(Collision collision)
    {
        // 🔊 SOUND
        if (audioSource != null && hitSound != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(hitSound);
        }

        // 📳 HAPTICS
        if (grabInteractable != null)
        {
            foreach (var interactor in grabInteractable.interactorsSelecting)
            {
                if (interactor is XRBaseInputInteractor controllerInteractor)
                {
                    controllerInteractor.SendHapticImpulse(hapticAmplitude, hapticDuration);
                }
            }
        }
    }
}
