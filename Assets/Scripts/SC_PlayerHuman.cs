using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SC_PlayerHuman : PlayerCharacter {

    [Header("Movement")]
    public float maxSpeed; //vitesse maximale de déplacement
    public float maxSpeedUnderwater; //vitesse maximale de déplacement sous l'eau
    public float minSpeed; //vitesse minimale de déplacement
    public float climbSpeedReducCoeff = 1; //par combien est divisiée la vitesse en escalade
    public float airPercentLoss; //pourcentage d'air perdu à chaque frame
    public AnimationCurve speedLossFromAir; //tableau de la perte de vitesse en fonction de l'air restant
    public float jumpHeight; //hauteur du saut
    public float distFloorForJump; //distance minimale avec le sol pour pouvoir sauter

    [Header("Item management")]
    bool canTake; //si le personnage peut prendre quelque chose
    public bool holdSmthg; //vrai si le personnage porte quelque chose
    GameObject canBeTakenItem; //objet à portée du personnage qu'il peut porter
    public GameObject heldItem; //ce que le personnage porte
    public GameObject takeIcon; //GameObject de l'icone montrant que le personnage peut prendre un objet
    Transform itemParentTrans;
    GameObject itemPositionGo;

    [Header("Gauge colors")]
    public Color airSliderColorSafe; //couleur de la jauge d'oxygène avant la perte de vitesse
    public Color airSliderColorDanger; //couleur de la jauge d'oxygène à la fin de la perte de vitesse

    [Header("Bird lifting")]
    public bool canTakeBird;
    public float maxDistanceToTakeBird;
    public float throwForce;
    GameObject birdReference;
    public GameObject birdThrowIcon;


    //Variables de déplacement
    float speed = 1; //vitesse de déplacement à l'instant
    float airStock = 100; //pourcentage d'air restant au personnage

    //Variables d'état
    bool canClimb; //si le personnage peut escalader une échelle
    bool climbing; //si le personnage est en train de monter le long d'une échelle
    bool isUnderwater; //vrai si le personnage est sous la surface, faux s'il est au-dessus
    bool canMove; //vrai si le joueur peut contrôler le personnage

    //Autre variables
    Rigidbody rb; //rigidbody de l'acteur
    //Collider coll; //collider de l'acteur (inutilisé)
    GameObject airGauge; 
    Slider airSlider; //jauge d'oxygène
    Transform sliderFill; //référence au "fill" du slider
    Transform rightArms;
    Transform leftArms;
    SC_ArmAnimation[] armsAnimations;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //coll = GetComponent<Collider>(); //inutilisé
        canMove = true;
        speed = isUnderwater ? maxSpeedUnderwater : maxSpeed;
        itemPositionGo = transform.GetChild(2).gameObject;
        rightArms = transform.GetChild(3);
        leftArms = transform.GetChild(4);
    }
    
    void FixedUpdate() {
        if (controller.character != this) return;
        //Identification du layer du joueur à ignorer pour les Raycast
        int layerMask = 1 << 8; //on identifie le layer 8 "player" comme étant celui à ignorer
        layerMask = ~layerMask;


        //Déplacements & interactions
        //Debug.Log(Input.GetAxisRaw("Horizontal"));
        if (canMove)
        {
            if (!climbing) //si est en train de marcher
            {
                Vector3 movement = new Vector3(Input.GetAxis(controller.horizontalAxis), 0, 0); //déplacement uniquement horizontal
                gameObject.transform.position += movement * speed * Time.deltaTime;

                if ((Input.GetAxis(controller.verticalAxis) >= 1 || Input.GetAxis(controller.verticalAxis) <= -1) && canClimb) //si essaye de monter près d'une échelle
                {
                    ClimbLadder(true);
                }
            }
            else //si en train d'escalader une échelle
            {
                Vector3 movement = new Vector3(0, Input.GetAxis(controller.verticalAxis), 0); //déplacement uniquement vertical
                gameObject.transform.position += movement * (speed / climbSpeedReducCoeff) * Time.deltaTime;

                if ((Input.GetAxis(controller.horizontalAxis) >= 1 || Input.GetAxis(controller.horizontalAxis) <= -1) && Physics.Raycast(transform.position, -Vector3.up, distFloorForJump, layerMask)) //si essaye de s'éloigner quand au sol
                {
                    ClimbLadder(false);
                }
            }

            if (canClimb)
            {
                foreach (SC_ArmAnimation anim in armsAnimations)
                {
                    anim.animationSpeed = speedLossFromAir.Evaluate(airStock / 100) + 0.1f;
                }
            }

            if (Input.GetButtonDown(controller.jumpButton)) //sauter
            {
                Jump(layerMask);
            }

            if (Input.GetButtonDown(controller.interactButton) && (canTake || holdSmthg || canTakeBird)) //prendre un objet ou l'oiseau
            {
                if (!holdSmthg) //si ne porte rien
                {
                    if (canTake)
                    {
                        heldItem = canBeTakenItem;
                        heldItem.transform.SetParent(transform);
                        holdSmthg = true;
                    }
                    else if (canTakeBird)
                    {
                        holdSmthg = true;
                        birdReference.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                        takeIcon.SetActive(false);
                        birdThrowIcon.SetActive(true);
                    }
                }
                else //si porte quelque chose
                {
                    if (heldItem != null) //si porte bien un objet
                    {
                        heldItem.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
                        heldItem.transform.SetParent(null);
                        heldItem = null;
                        holdSmthg = false;
                    }
                    else if(birdReference != null)// si porte l'oiseau
                    {
                        birdReference.GetComponent<Rigidbody>().AddForce(Vector3.up * throwForce);
                        birdThrowIcon.SetActive(false);
                    }
                }
            }

            if (holdSmthg) //adapter position d'un objet tenu en fonction du déplacement
            {
                if (heldItem != null) //si porte bien un objet
                {
                    heldItem.transform.position = itemPositionGo.transform.position;

                    if (Input.GetAxis(controller.horizontalAxis) > 0)
                    {
                        itemPositionGo.transform.localPosition = new Vector3(0.75f, -0.25f, 0f);
                        heldItem.transform.localEulerAngles = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
                    }
                    else if (Input.GetAxis(controller.horizontalAxis) < 0)
                    {
                        itemPositionGo.transform.localPosition = new Vector3(-0.75f, -0.25f, 0f);
                        heldItem.transform.localEulerAngles = new Vector3(transform.rotation.x, -180f, transform.rotation.z);
                    }
                }
            }

            if (canTakeBird) //équivalent moins pété de "OnCollisionExit" relatif à l'oiseau
            {
                if (Vector3.Distance(transform.position, birdReference.transform.position) > maxDistanceToTakeBird)
                {
                    BirdInteraction(null, false);
                    holdSmthg = false;
                    birdThrowIcon.SetActive(false);
                }
            }
        }

        
        //Gestion de l'air sous l'eau
        if (isUnderwater && airStock > 0)
        {
            airStock = airStock - airPercentLoss;
            SpeedChange(false);
            airSlider.value = airStock / 100;
            sliderFill.GetComponent<Image>().color = Color.Lerp(airSliderColorDanger, airSliderColorSafe, speedLossFromAir.Evaluate(airStock / 100)); //adapter la couleur de la jauge à l'air 
        }
    }



//Fonctions
    void Jump(LayerMask layerM) //saut
    {
        ClimbLadder(false);

        if (Physics.Raycast(transform.position, -Vector3.up, distFloorForJump, layerM))
        {
            if (!isUnderwater)
            {
                rb.AddForce(0, 100 * jumpHeight, 0);
            }
            else
            {
                rb.AddForce(0, 40 * jumpHeight, 0);
            }
        }
        
        /*
        //En cas de besoin de tweak du raycast de détection du sol
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, distFloorForJump, layerMask))
        {
            Debug.Log("là ça touche" + hit.collider.gameObject.name);
        }
        else
        {
            Debug.Log("touche pas");
        }*/
    }



    void SpeedChange(bool gain) //modification de la vitesse de déplacement
    {
        if (!gain) //est-ce une perte de vitesse ?
        {
            speed = Mathf.Lerp(minSpeed, maxSpeedUnderwater, speedLossFromAir.Evaluate(airStock / 100)); //la vitesse s'adapte en fonction de l'air restant
        }
        else //si c'est un regain de vitesse ou un reset, la valeur dépend du milieu
        {
            if (isUnderwater)
            {
                speed = maxSpeedUnderwater;
            }
            else
            {
                speed = maxSpeed;
            }
        }
    }



    void ClimbLadder(bool onLadder) //changement d'état pour pouvoir escalader
    {
        climbing = onLadder;
        gameObject.GetComponent<Rigidbody>().useGravity = !onLadder;
    }



    void ItemInteraction (GameObject item, bool isclose) //interaction vis-à-vis d'un objet au contact
    {
        takeIcon.gameObject.SetActive(isclose);
        canTake = isclose;
        canBeTakenItem = item;
    }

    void BirdInteraction (GameObject bird, bool isclose) //interaction vis-à-vis de l'oiseau au contact
    {
        takeIcon.gameObject.SetActive(isclose);
        canTakeBird = isclose;
        birdReference = bird;
    }

    private void OnTriggerEnter(Collider c)
    {
        //Debug.Log(c.gameObject.name);
		if (c.gameObject.tag == "Water") //entre dans l'eau
        {
			isUnderwater = true;
            SpeedChange(true);
            airGauge.SetActive(true);
        }
        else if (c.gameObject.tag == "Ladder") //au contact d'une échelle
        {
            if (c.gameObject.name.Contains("Top"))
            {
                canClimb = true;
                if (c.transform.parent.GetComponent<SC_LadderTop>().endAtRight)
                {
                    rightArms.gameObject.SetActive(true);
                    armsAnimations = rightArms.GetComponentsInChildren<SC_ArmAnimation>();
                    foreach (SC_ArmAnimation anim in armsAnimations)
                    {
                        anim.animationSpeed = speedLossFromAir.Evaluate(airStock / 100);
                        anim.StartAnimation();
                    }
                }
                else
                {
                    leftArms.gameObject.SetActive(true);
                    armsAnimations = leftArms.GetComponentsInChildren<SC_ArmAnimation>();
                    foreach (SC_ArmAnimation anim in armsAnimations)
                    {
                        anim.animationSpeed = speedLossFromAir.Evaluate(airStock / 100);
                        anim.StartAnimation();
                    }
                }
            }
        }
        else if (c.gameObject.tag == "Item") //au contact d'un objet
        {
            if (c.name.Contains("Air")) //récupère une bulle d'air
            {
                if (c.transform.parent != null)
                {
                    itemParentTrans = c.transform.parent;
                    if (itemParentTrans.name.Contains("Bird"))
                    {
                        itemParentTrans.GetComponent<BirdController>().heldItem = null;
                        itemParentTrans.GetComponent<BirdController>().holdSmthg = false;
                    }
                    else if (itemParentTrans.name.Contains("Otter"))
                    {
                        itemParentTrans.GetComponent<FloatingController>().heldItem = null;
                        itemParentTrans.GetComponent<FloatingController>().holdSmthg = false;
                    }
                }
                SpeedChange(true);
                Destroy(c.gameObject);
                airStock = 100;
            }
            else if (c.transform.parent == null)
            {
                ItemInteraction(c.gameObject, true);
            }
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag == "Water") //sort de l'eau
        {
            isUnderwater = false;
            SpeedChange(true);
            airStock = 100;
            airSlider.gameObject.SetActive(false);
        }
        else if (c.gameObject.tag == "Ladder" && canMove) //n'est plus au contact d'une échelle et déplacé par le joueur
        {
            if (c.gameObject.name.Contains("Top") && climbing && Input.GetAxis(controller.verticalAxis) >= 1) //si en haut de de l'échelle, effectue un dernier saut et s'en détache
            {
                Vector3 arrivalPosition = c.GetComponentInParent<SC_LadderTop>().GetArrivalPosition();
                canMove = false;
                StartCoroutine(ClimbingFinalJump(arrivalPosition, Time.time));
            }
            else
            {
                if (c.gameObject.name.Contains("LD"))
                {
                    if (c.GetComponent<SC_LadderTop>().endAtRight)
                    {
                        rightArms.gameObject.SetActive(false);
                        armsAnimations = rightArms.GetComponentsInChildren<SC_ArmAnimation>();
                        foreach (SC_ArmAnimation anim in armsAnimations)
                        {
                            anim.StopAllCoroutines();
                        }
                        armsAnimations = null;
                    }
                    else
                    {
                        leftArms.gameObject.SetActive(false);
                        leftArms.GetComponentInChildren<SC_ArmAnimation>().StopAnimation();
                        armsAnimations = leftArms.GetComponentsInChildren<SC_ArmAnimation>();
                        foreach (SC_ArmAnimation anim in armsAnimations)
                        {
                            anim.StopAllCoroutines();
                        }
                        armsAnimations = null;
                    }
                }

                canClimb = false;
                ClimbLadder(false);
            }
        }
        else if (c.gameObject.tag == "Item") //n'est plus au contact d'un objet
        {
            ItemInteraction(null, false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (birdReference == null)
        {
            if (collision.gameObject.tag == "Player") //au contact de l'oiseau
            {
                if (collision.gameObject.name.Contains("Bird"))
                {
                    BirdInteraction(collision.gameObject, true);
                }
            }
        }
    }
    
    public void SetSlider(GameObject gObj, Transform fill) //Informations envoyées par le slider
    {
        airGauge = gObj;
        airSlider = airGauge.GetComponent<Slider>();
        sliderFill = fill;
    }

    public void SetCanMove (bool allowMove) //Établir si le personnage peut se déplacer ou non
    {
        canMove = allowMove;
    }

    IEnumerator ClimbingFinalJump(Vector3 arrival, float startTime) //déplacement en fin d'escalade vers l'arrivée fixée
    {
        for (float fracJourney = 0; fracJourney <= 1.0f;)
        {
            float distCovered = (Time.time - startTime) * speed/50; //gestion de la durée de l'animation
            fracJourney = distCovered / Vector3.Distance(transform.position, arrival); //calcul de la progession
            transform.position = Vector3.Lerp(transform.position, arrival, fracJourney);
            yield return null;
        }
        canMove = true;
        canClimb = false;
        ClimbLadder(false);

        //Réinitilisation des animations (la détection de sortie des trigger ne se fait pas durant le lerp)
        rightArms.gameObject.SetActive(false);
        armsAnimations = rightArms.GetComponentsInChildren<SC_ArmAnimation>();
        foreach (SC_ArmAnimation anim in armsAnimations)
        {
            anim.StopAllCoroutines();
        }
        armsAnimations = null;

        leftArms.gameObject.SetActive(false);
        leftArms.GetComponentInChildren<SC_ArmAnimation>().StopAnimation();
        armsAnimations = leftArms.GetComponentsInChildren<SC_ArmAnimation>();
        foreach (SC_ArmAnimation anim in armsAnimations)
        {
            anim.StopAllCoroutines();
        }
        armsAnimations = null;
    }
}
