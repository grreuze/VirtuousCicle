using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SC_PlayerHuman : MonoBehaviour {

    //Variables de déplacement
    float speed = 1; //vitesse de déplacement à l'instant
    public float maxSpeed; //vitesse maximale de déplacement
    public float maxSpeedUnderwater; //vitesse maximale de déplacement sous l'eau
    public float minSpeed; //vitesse minimale de déplacement
    float airStock = 100; //pourcentage d'air restant au personnage
    public float airPercentLoss; //pourcentage d'air perdu à chaque frame
    public AnimationCurve speedLossFromAir; //tableau de la perte de vitesse en fonction de l'air restant
    public float jumpHeight; //hauteur du saut
    public float distFloorForJump; //distance minimale avec le sol pour pouvoir sauter

    //Variables d'état
    bool isUnderwater; //vrai si le personnage est sous la surface, faux s'il est au-dessus
    bool canClimb; //si le personnage peut escalader une échelle

    //Autre variables
    Rigidbody rb; //rigidbody de l'acteur
    Collider coll; //collider de l'acteur

    GameObject airGauge; 
    Slider airSlider; //jauge d'oxygène
    Transform sliderFill; //référence au "fill" du slider
    public Color airSliderColorSafe; //couleur de la jauge d'oxygène avant la perte de vitesse
    public Color airSliderColorDanger; //couleur de la jauge d'oxygène à la fin de la perte de vitesse
    


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();

        if (isUnderwater)
        {
            speed = maxSpeedUnderwater;
        }
        else
        {
            speed = maxSpeed;
        }
    }

    

    void Update()
    {
        //Vérification des conditions pour sauter
        int layerMask = 1 << 8; //on identifie le layer 8 "player" comme étant celui à ignorer
        layerMask = ~layerMask;

        if (Input.GetButtonDown("Jump2") && Physics.Raycast(transform.position, -Vector3.up, distFloorForJump, layerMask) && !isUnderwater) 
        {
            Jump();
        }

        /*
        //En cas de besoin de tweak du raycast de détection du sol
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, distFloorForJump, layerMask))
        {
            Debug.Log("bon, là ça touche" + hit.collider.gameObject.name);
        }
        else
        {
            Debug.Log("touche pas");
        }*/
    }



    void FixedUpdate()
    {
        //Gestion de l'air sous l'eau
        if (isUnderwater && airStock > 0)
        {
            airStock = airStock - airPercentLoss;
            SpeedChange(false);
            airSlider.value = airStock / 100;
            sliderFill.GetComponent<Image>().color = Color.Lerp(airSliderColorDanger, airSliderColorSafe, speedLossFromAir.Evaluate(airStock / 100)); //adapter la couleur de la jauge à l'air 
        }

        //Déplacements
        //Debug.Log(Input.GetAxisRaw("Horizontal"));
        if (!canClimb) //si n'est pas proche d'une échelle
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal2"), 0, 0); //déplacement uniquement horizontal
            gameObject.transform.position += movement * speed * Time.deltaTime;
        }
        else
        {
            Vector3 movement = new Vector3(0, Input.GetAxis("Vertical2"), 0); //déplacement uniquement vertical
            gameObject.transform.position += movement * speed * Time.deltaTime;
            if (Input.GetAxis("Horizontal2") >= 1 || Input.GetAxis("Horizontal2") <= -1)
            {
                canClimb = false;
            }
        }
    }



//Fonctions
    void Jump() //saut
    {
        rb.AddForce(0, 100 * jumpHeight, 0);
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



    private void OnTriggerEnter(Collider c)
    {
		if (c.gameObject.tag == "Water") //entre dans l'eau
        {
			isUnderwater = true;
            SpeedChange(true);
            airGauge.SetActive(true);
        }
        else if (c.gameObject.tag == "Ladder") //au contact d'une échelle
        {
            canClimb = true;
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
        else if (c.gameObject.tag == "Ladder") //n'est plus au contact d'une échelle
        {
            canClimb = false;
        }
    }

    //Détection des collisions & stuff
    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObj = collision.gameObject;
        Debug.Log(collidedObj);
        if (collision.gameObject.name.Contains("Item")) //en contact avec un object
        {
            if (collision.gameObject.tag == "IAir") //récupère une bulle d'air
            {
                SpeedChange(true);
                Destroy(collision.gameObject);
                airStock = 100;
            }
        }

       /* if (collision.gameObject.tag == "Water") //entre dans l'eau
        {
            isUnderwater = true;
            speed = maxSpeedUnderwater;
        }*/

                /*else if (collidedObj.name.Contains("Otter")) //WIP
        {
            Debug.Log(collidedObj.GetComponentInChildren<GameObject>().tag);

            //GameObject OtterChild = collidedObj
            if (collidedObj.GetComponentInChildren<GameObject>().tag == "IAir")
            {
                Debug.Log("that'll do it");
            }
        }*/
    }



    public void SetSlider(GameObject gObj, Transform fill) //Informations envoyées par le slider
    {
        airGauge = gObj;
        airSlider = airGauge.GetComponent<Slider>();
        sliderFill = fill;
    }
}
