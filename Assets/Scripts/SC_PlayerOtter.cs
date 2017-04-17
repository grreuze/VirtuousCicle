using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PlayerOtter : MonoBehaviour
{

    //Variables de déplacement
    public float speed; //vitesse de déplacement à l'instant



    // Use this for initialization
    void Start()
    {

    }



    void Update()
    {

    }



    void FixedUpdate()
    {
        //Déplacements
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1"), 0);
        gameObject.transform.position += movement * speed * Time.deltaTime;
    }

    //Détection des objets collisionnés
    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObj = collision.gameObject;
        if (collidedObj.name.Contains("Item"))
        {
            if (collidedObj.tag == "IAir")
            {
                collision.transform.parent = transform;
            }
        }
    }

    public GameObject GetChild()
    {
        return GetComponentInChildren<GameObject>();
    }
}
