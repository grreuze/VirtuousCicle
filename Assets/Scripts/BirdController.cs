using UnityEngine;

public class BirdController : PlayerCharacter
{

    [Header("Basic Movement")]
    public float maxVelocity;
    public float acceleration;
    [Header("Starting Boost")]
    public float boostForce;
    public float boostVelocityThreshold;
    [Header("Dash")]
    public float dashForce;
    public float dashCooldown;
    [Header("Gravity")]
    public float gravityScale;
    public float maxGravityVelocity;
    public float flapForce;

    Rigidbody rb;
    Vector3 inputDirection;
    float inputDelta;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
       // if (!controller.isActive) return;

        if (rb.velocity.magnitude > maxVelocity)
            rb.velocity = maxVelocity * rb.velocity.normalized;

        inputDirection = new Vector3(Input.GetAxis(controller.horizontalAxis), Input.GetAxis(controller.verticalAxis), 0f);

        if (inputDirection.magnitude > 0f)
        {
            ApplyMovement();
        }
        else
        {
            ApplyGravity();
        }
    }

    void ApplyMovement()
    {
        if (rb.velocity.magnitude < boostVelocityThreshold)
        {
            rb.velocity = rb.velocity / 2f;
            rb.AddForce(inputDirection * boostForce, ForceMode.VelocityChange);
        }
        else
        {
            rb.AddForce(inputDirection * acceleration);
        }
    }

    void ApplyGravity()
    {
        rb.AddForce(Vector3.up * gravityScale, ForceMode.Acceleration);

        if (rb.velocity.y < -maxGravityVelocity)
            rb.AddForce(Vector3.up * flapForce, ForceMode.VelocityChange);
    }
}
