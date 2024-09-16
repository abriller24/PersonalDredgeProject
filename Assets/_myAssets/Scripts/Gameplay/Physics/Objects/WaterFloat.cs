using Mono.Cecil;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
[RequireComponent(typeof(Rigidbody))]  
public class WaterFloat : MonoBehaviour
{
    public float airDrag = 1;
    public float waterDrag = 10;
    [SerializeField] private Transform[] floatPoints;
    [SerializeField] private bool attachToSurface;

    private Rigidbody rb;
    private Waves waves;
    private WaterPhysicsHelper waterPhysicsHelper;
    //water line components
    private float WaterLine;
    private Vector3[] WaterLinePoints;

    private Vector3 CenterOffset;
    private Vector3 SmoothVectorRotation;
    private Vector3 TargetUp;

    private Vector3 Center
    {
        get
        {
            return transform.position + CenterOffset;
        }

    }

    private void Awake()
    {
        waterPhysicsHelper = GetComponent<WaterPhysicsHelper>();
        waves = FindAnyObjectByType<Waves>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        WaterLinePoints = new Vector3[floatPoints.Length];
        for (int i = 0; i < floatPoints.Length; i++)
        {
            WaterLinePoints[i] = floatPoints[i].position;
        }
        CenterOffset = WaterPhysicsHelper.GetCenter(WaterLinePoints) - transform.position;
    }

    private void Update()
    {
        var newWaterLine = 0f;
        var pointUnderWater = false;

        for (int i = 0;i < floatPoints.Length;i++)
        {
            WaterLinePoints[i] = floatPoints[i].position;
            WaterLinePoints[i].y = waves.GetHeight(floatPoints[i].position);
            newWaterLine += WaterLinePoints[i].y / floatPoints.Length; 
            if (WaterLinePoints[i].y > floatPoints[i].position.y)
            {
                pointUnderWater = true;
            }
        }

        var waterLineDelta = newWaterLine - WaterLine;
        WaterLine = newWaterLine;

        var gravity = Physics.gravity;

        rb.linearDamping = airDrag;
        if(WaterLine > Center.y)
        {
            rb.linearDamping = waterDrag;
            if (attachToSurface)
            {
                rb.position = new Vector3(rb.position.x, WaterLine - CenterOffset.y, rb.position.z);
            }
            else
            {
                gravity = -Physics.gravity;
                transform.Translate(Vector3.up * waterLineDelta * 0.9f);
            }
        }
        rb.AddForce(gravity * Mathf.Clamp(Mathf.Abs(WaterLine - Center.y), 0, 1));

        TargetUp = WaterPhysicsHelper.GetNormal(WaterLinePoints);

        if(pointUnderWater)
        {
            TargetUp = Vector3.SmoothDamp(transform.up, TargetUp, ref SmoothVectorRotation, 0.2f);
            rb.rotation = Quaternion.FromToRotation(transform.up, TargetUp) * rb.rotation;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (floatPoints == null)
            return;

        for (int i = 0; i < floatPoints.Length; i++)
        {
            if (floatPoints[i] == null)
                continue;

            if (waves != null)
            {

                //draw cube
                Gizmos.color = Color.red;
                Gizmos.DrawCube(WaterLinePoints[i], Vector3.one * 0.3f);
            }

            //draw sphere
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(floatPoints[i].position, 0.1f);

        }

        //draw center
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector3(Center.x, WaterLine, Center.z), Vector3.one * 1f);
            Gizmos.DrawRay(new Vector3(Center.x, WaterLine, Center.z), TargetUp * 1f);
        }

    }
}
