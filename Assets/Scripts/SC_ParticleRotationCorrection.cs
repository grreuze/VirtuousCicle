using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_ParticleRotationCorrection : MonoBehaviour {

    void Update ()
    {
        transform.rotation = Quaternion.Euler(-135f, 0f, -90f);
	}
}
