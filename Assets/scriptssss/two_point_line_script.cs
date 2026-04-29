using UnityEngine;

[ExecuteInEditMode]
public class two_point_line_script : MonoBehaviour
{
    
    public Transform PointA, PointB;
    private LineRenderer line;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        line = GetComponent<LineRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        line.positionCount = 2;
        line.SetPosition(0, PointA.position);
        line.SetPosition(1, PointB.position);
    }
}
