using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Icons : MonoBehaviour
{
    [HideInInspector]
    public GameObject playerCharacter;

    void Update()
    {
        if (playerCharacter != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y + 1, playerCharacter.transform.position.z - 1));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}