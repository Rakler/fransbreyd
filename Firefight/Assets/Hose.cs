using Unity.VisualScripting;
using UnityEngine;

public class Hose : MonoBehaviour
{
    // inside your build loop, when you use the nozzlePrefab:
    [Header("Prefab references")]
    public Rigidbody2D nozzlePrefab;
    public Rigidbody2D hoseSectionPrefab;
    public Vector2 anchorPosition;
    public int length { get; private set; } = 150;
    public int maxLength = 300;
    public bool isVertical;
    public float hoseSegmentSpacing = 0.01f;
    public Rigidbody2D nozzle;

    public Rigidbody2D[] segments { get; private set; }

    void Start()
    {
        segments = new Rigidbody2D[maxLength];

        Vector2 pos = anchorPosition; // actually use this and advance it each loop
        Vector2 step = isVertical ? Vector2.down : Vector2.left;

        for (int i = 0; i < this.length; i++)
        {
            bool isLast = (i == this.length - 1);
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
                joint.limits = new JointAngleLimits2D { min = -90f, max = 90f };
                joint.enableCollision = true;
            }

            // advance spawn position
            pos += step * hoseSegmentSpacing;
        }
    }


    public void UpdateLength(int length)
    {
        Vector2 pos = Vector2.zero;
        if (this.length > length)
        {
            for (int i = 0; i < length - 1; i++)
            {
                Destroy(segments[i]);
                segments[i] = segments[i + 1];
            }
        }
        else if (this.length < length)
        {
            var nozzle = segments[this.length - 1];
            Rigidbody2D prefab = null;

            for (int i = 0; i < length; i++)
            {
                if (i >= this.length - 1)
                {
                    if (i >= length - 1)
                    {
                        segments[i] = nozzle;
                    }
                    else
                    {
                        segments[i] = Instantiate(prefab, pos, Quaternion.identity);
                    }
                    
                }

            }
        }
        else
        {
            return;
        }
        
        
    }
}
