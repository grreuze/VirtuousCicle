using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PlayerHuman : MonoBehaviour {

    //Variables de déplacement
    float speed = 1; //vitesse de déplacement à l'instant
    public float maxSpeed; //vitesse maximale de déplacement
    public float maxSpeedUnderwater; //vitesse maximale de déplacement sous l'eau
    public float minSpeed; //vitesse minimale de déplacement
    public float speedLossInWater; //vitesse à laquelle le personnage ralentit 
    public float jumpHeight; //hauteur du saut

    //Variables d'état
    public bool isUnderwater; //vrai si le personnage est sous la surface, faux s'il est au-dessus
    public bool isGrounded; //vrai si le personnage est en contact avec le sol

    //Autre variables
    Rigidbody rb; //rigidbody de l'acteur
    Collider coll; //collider de l'acteur



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
        if (Input.GetButtonDown("Jump2") && isGrounded && !isUnderwater)
        {
            Jump();
        }
    }



    void FixedUpdate()
    {
        //Gestion de la vitesse de déplacement
        if (!isUnderwater)
        {
            speed = maxSpeed;
        }
        else
        {
            if (speed > minSpeed)
            {
                speed = speed - (speedLossInWater * Time.deltaTime);
                //Debug.Log("speed" + speed);
                //Debug.Log("speedLoss" + speedLossInWater * Time.deltaTime);
            }
            else
            {
                speed = minSpeed;
            }
        }

        //Debug.Log(Input.GetAxisRaw("Horizontal"));
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal2"), 0, 0);
        gameObject.transform.position += movement * speed * Time.deltaTime;
    }



//Fonctions
    void Jump()
    {
        //Vector3 jumpDirection = new Vector3(0, 100 * jumpHeight, 0);
		//rb.AddForce(jumpDirection);
        rb.AddForce(0, 100 * jumpHeight, 0);
        isGrounded = false;
    }



//Détection des collisions & stuff
    private void OnTriggerEnter(Collider c) //trigger "aux pieds"
    {
        //Debug.Log(c.gameObject.tag);
        if (c.gameObject.tag == "Ground") //en contact avec le sol
        {
            isGrounded = true;
        }
		else if (c.gameObject.tag == "Water") //entre dans l'eau
        {
			isUnderwater = true;
            speed = maxSpeedUnderwater;
			//isGrounded = false;
        }
    }

    private void OnTriggerExit(Collider c)
    {
        //Debug.Log("Im outta " + c.gameObject.tag);
        if (c.gameObject.tag == "Ground") //n'est plus en contact avec le sol
        {
            isGrounded = false;
        }
		else if (c.gameObject.tag == "Water") //sortie de l'eau
        {
			isUnderwater = false;
			speed = maxSpeed;
		}
    }



    //Détection des objets collisionnés
    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObj = collision.gameObject;
        Debug.Log(collidedObj);
        if (collision.gameObject.name.Contains("Item"))
        {
            if (collision.gameObject.tag == "IAir")
            {
                speed = maxSpeedUnderwater;
                Destroy(collision.gameObject);
            }
        }
        else if (collidedObj.name.Contains("Otter")) //WIP
        {
            Debug.Log(collidedObj.GetComponentInChildren<GameObject>().tag);

            //GameObject OtterChild = collidedObj
            /*if (collidedObj.GetComponentInChildren<GameObject>().tag == "IAir")
            {
                Debug.Log("that'll do it");
            }*/
        }
    }
}
