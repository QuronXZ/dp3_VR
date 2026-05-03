using UnityEngine;

public class IntroCameraSwitch : MonoBehaviour
{
    public Camera introCamera;
    public GameObject xrOrigin; // XR Origin root

    public float introDuration = 5f;

    void Start()
    {
        introCamera.gameObject.SetActive(true);
        xrOrigin.SetActive(false);

        Invoke(nameof(SwitchToXR), introDuration);
    }

    void SwitchToXR()
    {
        introCamera.gameObject.SetActive(false);
        xrOrigin.SetActive(true);
    }
}