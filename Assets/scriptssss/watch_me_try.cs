using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(TextMesh))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class watch_me_try : MonoBehaviour
{

    public bool yawOnly = false;
    public float minimalViewDistance = 0.5f;

    private MeshRenderer meshRenderer;
    private TextMesh textMesh;
    private XRBaseInteractable interactable;



    private void OnEnable()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        textMesh = GetComponent<TextMesh>();
    }


/*    private void LateUpdate()
    {
        var cam = Camera.main;
        if (cam == null) return;
        //var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        Debug.Log("intera: ", interactable);
        //Debug.Log("intera count: "+ interactable.interactorsHovering.Count);

        //bool isHovered = interactable != null && interactable.interactorsHovering.Count > 0;

        var lookDir = transform.position - cam.transform.position;
        if (yawOnly) lookDir.y = 0;

        if (Vector3.SqrMagnitude(lookDir) < minimalViewDistance )
        {
            meshRenderer.enabled = false;
            //meshRenderer.enabled = true;
            transform.rotation = Quaternion.LookRotation(lookDir);
        }
        else
        {
            //meshRenderer.enabled = false;
            meshRenderer.enabled = true;
            //transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }
*/

    public void enable_text()

    {
        Debug.Log("inside enaBLE: ");
        var cam = Camera.main;
        var lookDir = transform.position - cam.transform.position;
        if (Vector3.SqrMagnitude(lookDir) < minimalViewDistance)
        {
            meshRenderer.enabled = true;
            transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }
    public void disable_text()
    {
        Debug.Log("inside DISABLE: ");
        meshRenderer.enabled = false;
    }

}