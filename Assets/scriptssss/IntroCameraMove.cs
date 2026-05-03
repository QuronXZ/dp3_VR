using UnityEngine;

public class IntroCameraMove : MonoBehaviour
{
    public Transform startPoint;
    public Transform midPoint;
    public Transform endPoint;

    public float duration = 6f;

    float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / duration;

        if (t < 0.5f)
        {
            transform.position = Vector3.Lerp(startPoint.position, midPoint.position, t * 2f);
        }
        else
        {
            transform.position = Vector3.Lerp(midPoint.position, endPoint.position, (t - 0.5f) * 2f);
        }

        transform.LookAt(endPoint);
    }
}