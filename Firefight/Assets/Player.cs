using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerInputSet input;
    Vector2 moveInput;

    public float moveSpeed = 15f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = new PlayerInputSet();


    }

    void OnEnable()
    {
        //Enable the input system when the player is enabled.
        input.Enable();


        //Subscribe to movement input event.
        input.Player.Movement.performed += ctx =>
         {
             moveInput = ctx.ReadValue<Vector2>();
        };

        // Subscribe to movement canceled event.
        input.Player.Movement.canceled += ctx =>
        {
            moveInput = Vector2.zero;
        };

    }


    void OnDisable()
    {
        // Disable the input system when the player is disabled.
        input.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(moveInput.x, moveInput.y) * moveSpeed;

        Vector2 force = new Vector2(moveInput.x, moveInput.y);
        rb.AddForce(force * moveSpeed);
    }

    void FixedUpdate()
    {
    }


}
