using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_LadderTop : MonoBehaviour {
    public bool endAtRight; //vrai si la suite du chemin en haut de l'échelle va vers la droite
    public GameObject arrival;

    public Vector3 GetArrivalPosition()
    {
        return arrival.gameObject.transform.position;
    }
}
