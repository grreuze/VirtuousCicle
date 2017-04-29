using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_ItemFall : MonoBehaviour {

    [HideInInspector]
    public bool droped;
    [HideInInspector]
    public bool falling;
    [HideInInspector]
    public float fallSpeed;
    public float groundDetectionHeight;

    Vector3 myPosition;

    LayerMask layMask;
    RaycastHit hit;

    private void Start()
    {
        layMask = 1 << 4 | 1 << 8; //ignorer les joueurs et l'eau
        layMask = ~layMask;
    }

    void Update ()
    {
		if (droped) //si lâché, tombe
        {
            falling = true;
            droped = false;
        }

        if (falling) //si suffisamment proche du sol, arrête de tomber
        {
            myPosition = transform.position;

            transform.position = new Vector3(myPosition.x, myPosition.y - fallSpeed, myPosition.z);

            if (Physics.Raycast(myPosition, -Vector3.up, out hit, Mathf.Infinity, layMask));
            {
                if (hit.distance < groundDetectionHeight)
                {
                    stopFall();
                }
            }
        }
	}

    public void stopFall()
    {
        falling = false;
    }
}
