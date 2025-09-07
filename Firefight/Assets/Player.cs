using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public string joystickNum = "";
    Rigidbody2D rb;
    Vector2 moveInput;

    private Vector2 JoyDir = Vector2.zero;

    [Header("Movement")]
    public float moveSpeed = 15f;

    [Header("Hose mechanics")]
    public Hose hose;
    public Transform hand;
    public float pickupRange = 0.9f;

    public float maxSpeed = 8f;
    public float accel = 60f;
    public float linDrag = 6f;

    //Runtime variables
    FixedJoint2D gripJoint;
    Rigidbody2D handRb;
    Rigidbody2D nozzleRb;
    Collider2D[] playerColliders;
    Collider2D[] hoseCols;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (!hand)
        {
            hand = new GameObject("Hand").transform;
            hand.SetParent(transform);
            hand.localPosition = new Vector3(0.4f, 0f, 0f);
        }

        handRb = hand.GetComponent<Rigidbody2D>();
        if (!handRb)
        {
            handRb = hand.gameObject.AddComponent<Rigidbody2D>();
            handRb.bodyType = RigidbodyType2D.Kinematic;
            handRb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        playerColliders = GetComponentsInChildren<Collider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        JoyDir.x = Input.GetAxisRaw("Horizontal" + joystickNum);
        JoyDir.y = Input.GetAxisRaw("Vertical" + joystickNum);

        if (Input.GetButtonDown("Fire" + joystickNum))
        {
            TryPickup();
        }
        if (Input.GetButtonDown("Drop" + joystickNum))
        {
            Drop();
        }
    }

    void FixedUpdate()
    {
        Vector2 desired = new Vector2(JoyDir.x, JoyDir.y) * maxSpeed;
        Vector2 dv = desired - rb.linearVelocity;

        // Cap how hard we accelerate
        float maxForce = accel * rb.mass;
        Vector2 force = dv * rb.mass / Time.fixedDeltaTime;
        if (force.sqrMagnitude > maxForce * maxForce)
            force = force.normalized * maxForce;

        rb.AddForce(force, ForceMode2D.Force);
        rb.linearDamping = linDrag; // mild baseline damping

        // Rotate player to face movement direction
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            angle = angle + 90; // Adjust if your sprite faces up
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }


    void Drop()
    {
        Debug.Log("Drop" + joystickNum);
        if (gripJoint) Destroy(gripJoint);
        gripJoint = null;

        if (nozzleRb)
            nozzleRb.AddForce((Vector2)hand.right * 2f, ForceMode2D.Impulse); // gentle toss
        nozzleRb.GetComponent<Nozzle>().EndSpray();

        ToggleHoseVsPlayerCollisions(false);
        nozzleRb = null;
        hoseCols = null;
    }

    void TryPickup()
    {
        Debug.Log("Try pickup");
        nozzleRb = hose ? hose.nozzle : null;
        if (!nozzleRb) { Debug.LogWarning("No nozzle found on HoseManager."); return; }

       

        if (Vector2.Distance(nozzleRb.position, (Vector2)hand.position) > pickupRange)
            return; // too far to grab

        if (gripJoint) return; // already holding
         var nozzleController = nozzleRb.GetComponent<Nozzle>();
        nozzleController.BeginSpray(GetComponentInChildren<Rigidbody2D>());
        // inside your TryPickup() where you currently create the grip
        gripJoint = nozzleRb.gameObject.AddComponent<FixedJoint2D>();
        gripJoint.connectedBody = rb;                 // <-- player's dynamic body
        gripJoint.autoConfigureConnectedAnchor = false;
        // Lock nozzle to the player's "hand" point:
        gripJoint.anchor = nozzleRb.transform.InverseTransformPoint(hand.position);
        gripJoint.connectedAnchor = rb.transform.InverseTransformPoint(hand.position);
        gripJoint.breakForce = Mathf.Infinity;        // set lower if you want it to rip free

        CacheHoseColliders();
        ToggleHoseVsPlayerCollisions(true);
    }

    void CacheHoseColliders()
    {
        if (hose == null || hose.segments == null) return;
        var list = new List<Collider2D>();
        foreach (var r in hose.segments)
        {
            if (!r) continue;
            list.AddRange(r.GetComponentsInChildren<Collider2D>());
        }
        hoseCols = list.ToArray();
    }

    void ToggleHoseVsPlayerCollisions(bool ignore)
    {
        if (hoseCols == null || playerColliders == null) return;
        foreach (var pc in playerColliders)
            foreach (var hc in hoseCols)
                if (pc && hc)
                    Physics2D.IgnoreCollision(pc, hc, ignore);
    }

    void OnDrawGizmosSelected()
    {
        if (!hand) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(hand.position, pickupRange);
    }

    // Debug log on nozzle bumps:
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit " + collision.gameObject.name);

        if (collision.collider && collision.collider.attachedRigidbody == hose?.nozzle)
            Debug.Log("Hit Nozzle!");
    }

}
