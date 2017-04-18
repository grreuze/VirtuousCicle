using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingController : MonoBehaviour {

    [Header("Basic Movement")]
    public float maxVelocity;
    public float acceleration;
    public float MovementHalfCone;
    [Header("Starting Boost")]
    public float boostForce;
    public float boostVelocityThreshold;
    public float boostRotationThreshold;
    [Header("Rotation")]
    public float rotationSpeed;
    public AnimationCurve rotationMultiplier;
    [Header("Dash")]
    public float dashForce;
    public float dashCooldown;
    [Header("Gravity")]
    public float gravityScale;
    public float maxGravityVelocity;

    Rigidbody rb;
    Vector3 inputDirection;
    float inputDelta;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate ()
    {
        if (rb.velocity.magnitude > maxVelocity)
            rb.velocity = maxVelocity * rb.velocity.normalized;

        inputDirection = new Vector3(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1"), 0f);

        if (inputDirection.magnitude > 0f)
        {
            inputDelta = Vector3.Angle(transform.forward, inputDirection);

            if (inputDelta > MovementHalfCone)
            {
                ApplyRotation();
            }
            else
            {
                ApplyRotation();
                ApplyMovement();
            }
        }
        else if (rb.velocity.magnitude < maxGravityVelocity)
        {
            ApplyGravity();
        }
	}

    void ApplyRotation()
    {
        float rotDirection = Vector3.Angle(transform.right, inputDirection) >= 90f ? 1f : -1f;

        if (inputDelta > 2f)
        {
            rb.AddTorque(0f, 0f, rotationSpeed * rotationMultiplier.Evaluate(inputDelta) * rotDirection, ForceMode.Acceleration);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    void ApplyMovement()
    {
        if (Vector3.Angle(transform.forward, rb.velocity) > boostRotationThreshold || rb.velocity.magnitude < boostVelocityThreshold)
        {
            rb.velocity = rb.velocity / 2f;
            rb.AddForce(transform.forward * boostForce, ForceMode.VelocityChange);
        }
        else
        {
            rb.AddForce(transform.forward * acceleration);
        }
    }

    void ApplyGravity()
    {
        rb.AddForce(Vector3.up * gravityScale, ForceMode.Acceleration);
    }
}
