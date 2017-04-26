using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloatingController : PlayerCharacter {

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
    public float gravityScaleOutWater;
    public float maxGravityVelocity;
    [Header("Movement on ground")]
    public float boostForceOnGround;

    Rigidbody rb;
    Vector3 inputDirection;
    float inputDelta;

    float initialBoostForce;
    bool isInWater = true;
    
    void Start () {
        rb = GetComponent<Rigidbody>();
        initialBoostForce = boostForce;
    }

    void FixedUpdate()
    {
        if (!controller.isActive) return;

        if (isInWater)
        {
            inputDirection = new Vector3(Input.GetAxis(controller.horizontalAxis), Input.GetAxis(controller.verticalAxis), 0f);
        }
        else
        {
            inputDirection = new Vector3(Input.GetAxis(controller.horizontalAxis), Input.GetAxis(controller.verticalAxis) * 0.1f, 0f);
        }
        if (rb.velocity.magnitude > maxVelocity)
            rb.velocity = maxVelocity * rb.velocity.normalized;

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
        else if (rb.velocity.magnitude < maxGravityVelocity && isInWater)
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
    
    IEnumerator TransitionWater(bool entering, float transitionProgression)
    {
        isInWater = entering;
        while (isInWater == false)
        {
            ApplyGravityOutWater(transitionProgression);
            transitionProgression += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        if (isInWater == true)
        {
            while (transitionProgression >= 0f)
            {
                ApplyGravityOutWater(transitionProgression);
                transitionProgression -= 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    void ApplyGravityOutWater(float gravityChangeFactor)
    {
        rb.AddForce(Vector3.up * Mathf.Lerp(gravityScale, gravityScaleOutWater, gravityChangeFactor), ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            StartCoroutine(TransitionWater(true, 1.0f));
            boostForce = initialBoostForce;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            StartCoroutine(TransitionWater(false, 0f));
            boostForce = boostForceOnGround;
        }
    }
}
