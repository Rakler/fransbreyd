// using Unity.Mathematics;
// using UnityEngine;

// public class HoseManager : MonoBehaviour
// {
//     public Rigidbody2D nozzlePrefab;
//     public Rigidbody2D hoseSectionPrefab;
//     public Vector2 anchorPosition;
//     public int length = 40;
//     public bool isVertical;
//     public float hoseSegmentSpacing = 0.2f;

//     public Rigidbody2D[] segments { get; private set; }
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         segments = new Rigidbody2D[length];

//         Vector2 position = transform.position;

//         Rigidbody2D previousBody = null;

//         for (int i = 0; i < segments.Length; i++)
//         {
//             Rigidbody2D prefab = hoseSectionPrefab;

//             var segment = Instantiate(prefab, anchorPosition, quaternion.identity);

//             segments[i] = segment;

//             if (previousBody != null)
//             {
//                 HingeJoint2D joint = segment.GetComponent<HingeJoint2D>();

//                 joint.connectedBody = previousBody;
//                 joint.autoConfigureConnectedAnchor = false;
//                 if (isVertical)
//                 {
//                     joint.anchor = new Vector2(0, -hoseSegmentSpacing * 0.5f);
//                     joint.connectedAnchor = new Vector2(0, hoseSegmentSpacing * 0.5f);
//                 }
//                 else
//                 {
//                     joint.anchor = new Vector2(-hoseSegmentSpacing * 0.5f, 0);
//                     joint.connectedAnchor = new Vector2(hoseSegmentSpacing * 0.5f, 0);
//                 }

//                 joint.useLimits = true;
//                 joint.limits = new JointAngleLimits2D { min = -20f, max = 20f };
//                 joint.enableCollision = true;
//             }
//             previousBody = segment;
//             position += Vector2.left * hoseSegmentSpacing;

//             if (i == segments.Length)
//             {
//                 var endNozzle = Instantiate(nozzlePrefab, anchorPosition, quaternion.identity);

//                 segments[i] = endNozzle;

//                 HingeJoint2D joint = segment.GetComponent<HingeJoint2D>();

//                 joint.connectedBody = previousBody;
//                 joint.autoConfigureConnectedAnchor = false;

//                 if (isVertical)
//                 {
//                     joint.anchor = new Vector2(0, -hoseSegmentSpacing * 0.5f);
//                     joint.connectedAnchor = new Vector2(0, hoseSegmentSpacing * 0.5f);
//                 }
//                 else
//                 {
//                     joint.anchor = new Vector2(-hoseSegmentSpacing * 0.5f, 0);
//                     joint.connectedAnchor = new Vector2(hoseSegmentSpacing * 0.5f, 0);
//                 }
                
//                 joint.useLimits = true;
//                 joint.limits = new JointAngleLimits2D { min = -20f, max = 20f };
//                 joint.enableCollision = true;
//             }
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }

//     void LateUpdate()
//     {
//     }

// }

using UnityEngine;

public class HoseManager : MonoBehaviour
{
// inside your build loop, when you use the nozzlePrefab:
    public Rigidbody2D nozzlePrefab;
    public Rigidbody2D hoseSectionPrefab;
    public Vector2 anchorPosition;
    public int length = 40;
    public bool isVertical;
    public float hoseSegmentSpacing = 0.2f;
    public Rigidbody2D nozzle;

    public Rigidbody2D[] segments { get; private set; }

    void Start()
    {
        segments = new Rigidbody2D[length];

        Vector2 pos = anchorPosition; // actually use this and advance it each loop
        Vector2 step = isVertical ? Vector2.down : Vector2.left;

        for (int i = 0; i < length; i++)
        {
            bool isLast = (i == length - 1);
            var prefab = isLast ? nozzlePrefab : hoseSectionPrefab;



            // spawn this link
            Rigidbody2D seg = Instantiate(prefab, pos, Quaternion.identity);

            if (isLast)
            {
                nozzle = seg;
            }

            segments[i] = seg;

            // connect to previous
            if (i > 0)
            {
                var joint = seg.GetComponent<HingeJoint2D>();
                if (!joint) joint = seg.gameObject.AddComponent<HingeJoint2D>();

                joint.connectedBody = segments[i - 1];
                joint.autoConfigureConnectedAnchor = false;

                if (isVertical)
                {
                    joint.anchor = new Vector2(0, -hoseSegmentSpacing * 0.5f);
                    joint.connectedAnchor = new Vector2(0, hoseSegmentSpacing * 0.5f);
                }
                else
                {
                    joint.anchor = new Vector2(-hoseSegmentSpacing * 0.5f, 0);
                    joint.connectedAnchor = new Vector2(hoseSegmentSpacing * 0.5f, 0);
                }

                joint.useLimits = true;
                joint.limits = new JointAngleLimits2D { min = -20f, max = 20f };
                joint.enableCollision = true;
            }

            // advance spawn position
            pos += step * hoseSegmentSpacing;
        }
    }
}
