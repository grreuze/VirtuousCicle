using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public float gravityScaleInWater;
    [Header("OnGround")]
    public float speed;
    public float jumpHeight;
    public float distFloorForJump;
    [Header("State")]
    public bool oilCovered;
    public bool grounded;
    [Header("Item management")]
    public GameObject heldItem; //ce que le personnage porte
    public GameObject takeIcon; //GameObject de l'icone montrant que le personnage peut prendre un objet
    public bool holdSmthg; //vrai si le personnage porte quelque chose
    public float dropedItemFallSpeed; //vitesse à laquelle tombe un objet lâché par l'oiseau (supposé dans l'air)
    [Header("Hunger")]
    public bool isHungry;
    public float speedHungry;

    Rigidbody rb;
    Vector3 inputDirection;
    float inputDelta;
    LayerMask layerMask;
    bool isInWater = false;

    float initialSpeed;

    bool canTake; //si le personnage peut prendre quelque chose
    GameObject canBeTakenItem; //objet à portée du personnage qu'il peut porter
    GameObject itemPositionGo;
    Transform itemParentTrans;
    SC_ItemFall itemFallScript;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        takeIcon.GetComponent<SC_Icons>().playerCharacter = gameObject;
        takeIcon.SetActive(false);

        initialSpeed = speed;

        itemPositionGo = transform.GetChild(2).gameObject;

        layerMask = 1 << 8; //on identifie le layer 8 "player" comme étant celui à ignorer pour les raycasts
        layerMask = ~layerMask;

        if (isHungry)
        {
            transform.GetChild(3).gameObject.SetActive(true);
            speed = speedHungry;
        }
    }

    void FixedUpdate()
    {
        // if (!controller.isActive) return;
        if (Input.GetAxis(controller.verticalAxis) >= 0.5f && !oilCovered) //détection atterrissage
        {
            grounded = false;
        }
        else if (Physics.Raycast(transform.position, -Vector3.up, distFloorForJump*3.0f, layerMask) && !isInWater) 
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

            if (!isInWater)
            {
                inputDirection = new Vector3(Input.GetAxis(controller.horizontalAxis), Input.GetAxis(controller.verticalAxis), 0f);
            }
            else
            {
                inputDirection = new Vector3(Input.GetAxis(controller.horizontalAxis), Input.GetAxis(controller.verticalAxis) * 0.00001f, 0f);
            }

            if (inputDirection.magnitude > 0f)
            {
                ApplyMovement();
            }
            else
            {
                if (!isInWater)
                {
                    ApplyGravity();
                }
            }
        }

        if (Input.GetButtonDown(controller.interactButton) && (canTake || holdSmthg)) //prendre un objet
        {
            if (!holdSmthg) //si ne porte rien, prend
            {
                if (itemFallScript.falling)
                {
                    itemFallScript.stopFall();
                }
                heldItem = canBeTakenItem;
                heldItem.transform.SetParent(transform);
                holdSmthg = true;
            }
            else //si porte quelque chose, lâche
            {
                heldItem.transform.SetParent(null);
                itemFallScript.droped = true;
                itemFallScript.fallSpeed = dropedItemFallSpeed;
                heldItem = null;
                holdSmthg = false;
            }
        }

        if (holdSmthg) //adapter position d'un objet tenu en fonction du déplacement
        {
            heldItem.transform.position = itemPositionGo.transform.position;

            if (Input.GetAxis(controller.horizontalAxis) > 0)
            {
                itemPositionGo.transform.localPosition = new Vector3(0.5f, 0f, 0f);
            }
            else if (Input.GetAxis(controller.horizontalAxis) < 0)
            {
                itemPositionGo.transform.localPosition = new Vector3(-0.5f, 0f, 0f);
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

    IEnumerator TransitionWater(bool exiting, float transitionProgression)
    {
        isInWater = exiting;
        while (isInWater == true)
        {
            ApplyGravityOutWater(transitionProgression);
            transitionProgression += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        if (isInWater == false)
        {
            while (transitionProgression >= 0f)
            {
                ApplyGravityOutWater(transitionProgression);
                transitionProgression -= 0.1f;
                yield return new WaitForSeconds(0.005f);
            }
        }
    }

    void ApplyGravityOutWater(float gravityChangeFactor)
    {
        rb.AddForce(Vector3.up * Mathf.Lerp(gravityScale, gravityScaleInWater, gravityChangeFactor), ForceMode.Acceleration);
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
            StartCoroutine(TransitionWater(true, 0f));
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
                    else if (itemParentTrans.name.Contains("Otter"))
                    {
                        itemParentTrans.GetComponent<FloatingController>().heldItem = null;
                        itemParentTrans.GetComponent<FloatingController>().holdSmthg = false;
                    }
                }
                Destroy(other.gameObject);
                transform.GetChild(3).gameObject.SetActive(false);
                isHungry = false;
                speed = initialSpeed;
            }
            else if (other.transform.parent == null)
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
            StartCoroutine(TransitionWater(false, 0.2f));
        }
        else if (other.gameObject.tag == "Item") //n'est plus au contact d'un objet
        {
            ItemInteraction(other.gameObject, false);
        }
    }
}
