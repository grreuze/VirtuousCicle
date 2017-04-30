using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_ArmAnimation : MonoBehaviour {

    [HideInInspector]
    public float animationSpeed;
    public bool goingUp;
    public bool amAtRight;
    private Quaternion myRotation;

    public void StartAnimation()
    {
        StartCoroutine(ClimbAnimation());
    }

    public void StopAnimation()
    {
        StopAllCoroutines();
    }

    public IEnumerator ClimbAnimation()
    {
        myRotation = transform.rotation;

        while (true)
        {
            if (goingUp)
            {
                if (amAtRight)
                {
                    if (transform.rotation.z >= 0.35f)
                    {
                        goingUp = false;
                    }
                    else
                    {
                        transform.Rotate(new Vector3(myRotation.x, myRotation.y, myRotation.z + 5f * animationSpeed));
                        yield return new WaitForSeconds(0.025f);
                    }
                }
                else
                {
                    if (transform.rotation.z >= -0.35f)
                    {
                        transform.Rotate(new Vector3(myRotation.x, myRotation.y, myRotation.z - 5f * animationSpeed));
                        yield return new WaitForSeconds(0.025f);
                    }
                    else
                    {
                        goingUp = false;
                    }
                }
            }
            else
            {
                if (amAtRight)
                {
                    if (transform.rotation.z >= -0.35f)
                    {
                        transform.Rotate(new Vector3(myRotation.x, myRotation.y, myRotation.z - 5f * animationSpeed));
                        yield return new WaitForSeconds(0.025f);
                    }
                    else
                    {
                        goingUp = true;
                    }
                }
                else
                {
                    if (transform.rotation.z >= 0.35f)
                    {
                        goingUp = true;
                    }
                    else
                    {
                        transform.Rotate(new Vector3(myRotation.x, myRotation.y, myRotation.z + 5f * animationSpeed));
                        yield return new WaitForSeconds(0.025f);
                    }
                }
            }
        }
    }
	
}
