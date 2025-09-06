using UnityEngine;

public class Nozzle : MonoBehaviour
{
    [Header("Water")]
    public ParticleSystem water;        // assign a PS on the nozzle (child is fine)
    public float pressure = 1.0f;       // scale 0..2
    public float baseSpeed = 30f;       // particle start speed at pressure=1

    [Header("Recoil")]
    public float recoilPerSecond = 80f; // force per second while spraying

    Rigidbody2D rb;
    Rigidbody2D holder;                 // player's RB while held
    bool spraying;

    void Awake()
    {
        rb.GetComponent<Rigidbody2D>();
    }


    public void BeginSpray(Rigidbody2D hoseHolder)
    {
        spraying = true;

        if (water && !water.isPlaying)
        {
            water.Play();

        }

        if (water)
        {
            var main = water.main;
        }
        //Implement spray logic
    }

    // public float sprayDistance = 2f;
    // public Rigidbody2D waterSpray;
    // void OnEnable()
    // {

    // }

    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    // }

    // // Update is called once per frame
    // void Update()
    // {
    // }

    // void OnDrawGizmos()
    // {
    //     //Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));

    //     Gizmos.DrawLine(transform.position, (transform.forward).normalized * sprayDistance);
    // }
}

