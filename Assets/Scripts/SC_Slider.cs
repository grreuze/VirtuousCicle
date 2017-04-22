using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Slider : MonoBehaviour {

    public GameObject playerCharacter; //le personnage auquel la jauge sera liée

	void Start ()
    {
        if (playerCharacter.name.Contains("Human"))
        {
            playerCharacter.GetComponent<SC_PlayerHuman>().SetSlider(this.gameObject, transform.FindChild("Fill Area/Fill").transform); //envoie les informations nécessaire au player
            gameObject.SetActive(false);
        }

	}

    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y + 1, playerCharacter.transform.position.z - 1));
        //transform.position = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y + 1, playerCharacter.transform.position.z - 1); //du coup c'est une position dégueue en World Space...    
        //transform.InverseTransformPoint(playerCharacter.transform.position.x, playerCharacter.transform.position.y - 2, playerCharacter.transform.position.z); //ça marche pas bien ça...
    }
}
