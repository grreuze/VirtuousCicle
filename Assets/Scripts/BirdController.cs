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
    [Header("OnGround")]
    public float speed;
    public float jumpHeight;
    public float distFloorForJump;
    [Header("State")]
    public bool oilCovered;
    public bool grounded;

    Rigidbody rb;
    Vector3 inputDirection;
    float inputDelta;
    LayerMask layerMask;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        layerMask = 1 << 8; //on identifie le layer 8 "player" comme étant celui à ignorer pour les raycasts
        layerMask = ~layerMask;
    }

    void FixedUpdate()
    {
        // if (!controller.isActive) return;
        if (Input.GetAxis(controller.verticalAxis) >= 0.5f && !oilCovered) //détection atterrissage
        {
            grounded = false;
        }
        else if (Physics.Raycast(transform.position, -Vector3.up, distFloorForJump*3.0f, layerMask)) 
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
        
        if (oilCovered || grounded) //déplacements au sol
        {
            rb.useGravity = true;

            Vector3 movement = new Vector3(Input.GetAxis(controller.horizontalAxis), 0, 0); //déplacement uniquement horizontal
            gameObject.transform.position += movement * speed * Time.deltaTime;

            if (Input.GetButtonDown(controller.jumpButton)) //si essaye de s'envoler
            {
                if (!oilCovered)
                {
                    rb.AddForce(0, 75 * jumpHeight, 0);
                }
                else
                {
                    if (Physics.Raycast(transform.position, -Vector3.up, distFloorForJump, layerMask))
                    {
                        rb.AddForce(0, 50 * jumpHeight, 0);
                    }
                }
            }
        }
        else //déplacements dans le ciel
        {
            GetComponent<Rigidbody>().useGravity = false;

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

        if (rb.velocity.y < -maxGravityVelocity && !oilCovered)
            rb.AddForce(Vector3.up * flapForce, ForceMode.VelocityChange);
    }

}
