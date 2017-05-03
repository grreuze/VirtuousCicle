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
    public bool startsOutWater;
    public float boostForceOnGround;
    [Header("Item management")]
    public GameObject heldItem; //ce que le personnage porte
    public GameObject takeIcon; //GameObject de l'icone montrant que le personnage peut prendre un objet
    public bool holdSmthg; //vrai si le personnage porte quelque chose
    public float dropedItemFallSpeed; //vitesse à laquelle tombe un objet lâché par l'otarie (supposé dans l'eau)
    [Header("Hunger")]
    public bool isHungry;
    public float boostForceHungry;

    Rigidbody rb;
    Vector3 inputDirection;
    float inputDelta;

    float initialBoostForce;
    bool isInWater = true;

    bool canTake; //si le personnage peut prendre quelque chose
    GameObject canBeTakenItem; //objet à portée du personnage qu'il peut porter
    Vector3 itemPosition;
    Transform itemParentTrans;
    SC_ItemFall itemFallScript;

    void Start () {
        rb = GetComponent<Rigidbody>();

        initialBoostForce = boostForce;
        takeIcon.GetComponent<SC_Icons>().playerCharacter = gameObject;
        takeIcon.SetActive(false);

        if (startsOutWater)
        {
            StartCoroutine(TransitionWater(false, 1f));
        }

        if (isHungry)
        {
            transform.GetChild(4).gameObject.SetActive(true);
            boostForce = boostForceHungry;
        }
    }

    void FixedUpdate() {
        if (controller.character != this) return;

        if (isInWater)
        {
            inputDirection = new Vector3(Input.GetAxis(controller.horizontalAxis), Input.GetAxis(controller.verticalAxis), 0f);
        }
        else
        {
            inputDirection = new Vector3(Input.GetAxis(controller.horizontalAxis), 0f, 0f);
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

        if (Input.GetButtonDown(controller.interactButton) && (canTake || holdSmthg)) //prendre un objet
        {
            if (!holdSmthg) //si ne porte rien, prend
            {
                heldItem = canBeTakenItem;
                if (!heldItem.name.Contains("Air"))
                {
                    if (itemFallScript.falling)
                    {
                        itemFallScript.stopFall();
                    }
                }
                heldItem.transform.SetParent(transform);
                itemPosition = transform.GetChild(3).position;
                heldItem.transform.position = itemPosition;
                holdSmthg = true;
            }
            else //si porte quelque chose, lâche
            {
                heldItem.transform.SetParent(null);
                if (!heldItem.name.Contains("Air"))
                {
                    itemFallScript.droped = true;
                    itemFallScript.fallSpeed = dropedItemFallSpeed;
                }
                heldItem = null;
                holdSmthg = false;
            }
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
            if (!isHungry)
            {
                if (isInWater)
                {
                    rb.AddForce(transform.forward * acceleration);
                }
                else
                {
                    rb.AddForce(transform.forward * acceleration * 0.85f);
                }

            }
            else
            {
                rb.AddForce(transform.forward * acceleration * 0.01f);
            }
        }
    }

    void ApplyGravity()
    {
        rb.AddForce(Vector3.up * gravityScale, ForceMode.Acceleration);
    }
    
    IEnumerator TransitionWater(bool entering, float transitionProgression)
    {
        if (entering == false)
        {
            isInWater = false;
            while (isInWater == false)
            {
                ApplyGravityOutWater(transitionProgression);
                if (transitionProgression < 1)
                {
                    transitionProgression += 0.1f;
                }
                yield return new WaitForSeconds(0.066f);
            }
        }
        else
        {
            while (transitionProgression >= 0f)
            {
                ApplyGravityOutWater(transitionProgression);
                transitionProgression -= 0.1f;
                yield return new WaitForSeconds(0.033f);
            }
            isInWater = true;
        }
    }

    void ApplyGravityOutWater(float gravityChangeFactor)
    {
        rb.AddForce(Vector3.up * Mathf.Lerp(gravityScale, gravityScaleOutWater, gravityChangeFactor), ForceMode.Acceleration);
    }

    void ItemInteraction(GameObject item, bool isclose) //interaction vis-à-vis d'un objet au contact
    {
        takeIcon.gameObject.SetActive(isclose);
        canTake = isclose;
        canBeTakenItem = item;
        itemFallScript = item.GetComponent<SC_ItemFall>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            StopCoroutine("TransitionWater");
            StartCoroutine(TransitionWater(true, 0.5f));
            if (isHungry)
            {
                boostForce = boostForceHungry;
            }
            else
            {
                boostForce = initialBoostForce;
            }
        }
        else if (other.gameObject.tag == "Item") //au contact d'un objet
        {
            if (other.name.Contains("Fish") && isHungry) //mange un poisson
            {
                if (other.transform.parent != null)
                {
                    itemParentTrans = other.transform.parent;
                    if (itemParentTrans.name.Contains("Human"))
                    {
                        itemParentTrans.GetComponent<SC_PlayerHuman>().heldItem = null;
                        itemParentTrans.GetComponent<SC_PlayerHuman>().holdSmthg = false;
                    }
                    else if (itemParentTrans.name.Contains("Bird"))
                    {
                        itemParentTrans.GetComponent<BirdController>().heldItem = null;
                        itemParentTrans.GetComponent<BirdController>().holdSmthg = false;
                    }
                }
                Destroy(other.gameObject);
                transform.GetChild(4).gameObject.SetActive(false);
                isHungry = false;
                if (isInWater)
                {
                    boostForce = initialBoostForce;
                }
                else
                {
                    boostForce = boostForceOnGround;
                }
            }
            else if(other.transform.parent == null)
            {
                ItemInteraction(other.gameObject, true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            StopCoroutine("TransitionWater");
            StartCoroutine(TransitionWater(false, 0f));
            if (isHungry)
            {
                boostForce = boostForceHungry;
            }
            else
            {
                boostForce = boostForceOnGround;
            }
        }
        else if (other.gameObject.tag == "Item") //n'est plus au contact d'un objet
        {
            ItemInteraction(other.gameObject, false);
        }
    }
}
