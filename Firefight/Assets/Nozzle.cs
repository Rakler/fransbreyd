using UnityEngine;

public class Nozzle : MonoBehaviour
{
    [Header("Scene refs")]
    public Transform muzzle;            // tip of nozzle (faces +X)
    public SpriteRenderer jet;          // child sprite (Draw Mode = Tiled)
    public Transform impact;            // optional splash sprite

    [Header("Stream")]
    public float maxDistance = 10f;     // max range
    public float width = 0.25f;         // visual & hit “thickness”
    public LayerMask hitMask;           // what water can hit
    public float pressure = 1.0f;       // scales recoil/force

    [Header("Physics")]
    public float recoilPerSecond = 80f; // hose kickback while spraying
    public float pushPerSecond = 20f;   // push applied to hit rigidbodies

    public Animator animator;
    Rigidbody2D nozzleRb;
    Rigidbody2D holder;                 // player's RB while held
    bool spraying;
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        nozzleRb = GetComponent<Rigidbody2D>();
        if (jet)
        {
            jet.drawMode = SpriteDrawMode.Tiled; // lets us change length at runtime
            jet.gameObject.SetActive(false);
        }
        if (impact) impact.gameObject.SetActive(false);
        if (!muzzle) muzzle = transform; // fallback
    }


    public void BeginSpray(Rigidbody2D hoseHolder)
    {
        holder = hoseHolder;
        spraying = true;
        animator.SetBool("IsSpray", spraying);

        Debug.Log("Spraying has begun! " + animator.GetBool("IsSpray"));
        if (jet) jet.gameObject.SetActive(true);
        if (impact) impact.gameObject.SetActive(true);
        
    }

    public void EndSpray()
    {
        spraying = false;
        holder = null;
        animator.SetBool("IsSpray", spraying);
        if (jet) jet.gameObject.SetActive(false);
        if (impact) impact.gameObject.SetActive(false);
        
    }
    
      void FixedUpdate()
    {
        if (!spraying || !jet) return;

        Vector2 origin = muzzle.position;
        Vector2 dir = muzzle.right.normalized;

        // Find hit point
        var hit = Physics2D.Raycast(origin, dir, maxDistance, hitMask);
        float dist = hit ? hit.distance : maxDistance;

        // Place/size the jet sprite
        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        jet.transform.SetPositionAndRotation(origin + dir * (dist * 0.5f), Quaternion.Euler(0,0,ang));
        jet.size = new Vector2(dist, width);            // length = distance, height = width

        // Optional: place impact sprite and apply push
        if (impact)
        {
            if (hit)
            {
                impact.gameObject.SetActive(true);
                impact.position = hit.point;
                impact.right = -dir; // face back along the stream

                if (hit.rigidbody)
                    hit.rigidbody.AddForce(dir * (pushPerSecond * pressure) * Time.fixedDeltaTime,
                                           ForceMode2D.Force);
            }
            else
            {
                impact.gameObject.SetActive(false);
            }
        }

        // Recoil back through the hose (and a bit into the player if attached)
        Vector2 recoil = -dir * (recoilPerSecond * pressure) * Time.fixedDeltaTime;
        nozzleRb.AddForce(recoil, ForceMode2D.Force);
        if (holder) holder.AddForce(recoil * 0.5f, ForceMode2D.Force);

        // (Optional) Area push along the whole beam for multiple targets:
        var center = origin + dir * (dist * 0.5f);
        var hits = Physics2D.BoxCastAll(center, new Vector2(dist, width), ang, Vector2.zero, 0f, hitMask);
        foreach (var h in hits)
            if (h.rigidbody) h.rigidbody.AddForce(dir * (pushPerSecond * 0.5f * pressure) * Time.fixedDeltaTime,
                                                  ForceMode2D.Force);
    }
}

