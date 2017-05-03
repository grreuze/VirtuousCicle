using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_flyAwayTransition : MonoBehaviour
{
    public GameObject characterToAffect;
    //public string characterToAffectName;
    GameObject charaReference;
    Rigidbody charaRbReference;
    bool inAnimation = false;
    public GameObject whereToTP;
    public GameObject invisibleWallToPop;

    public SC_PlayerHuman HumanScriptReference;

    float progression = 0;
    Quaternion initialRotation;

    private void OnTriggerEnter(Collider other)
    {
        if (!inAnimation)
        {
            if (other.transform.parent != null)
            {
                if (other.transform.parent.gameObject == characterToAffect)
                {
                    charaReference = other.transform.parent.gameObject;
                    charaRbReference = charaReference.GetComponent<Rigidbody>();
                    //charaReference.GetComponent<> //virer les colliders pour éviter les entrées multiples
                    inAnimation = true;
                    initialRotation = charaReference.transform.rotation;
                    StartCoroutine(TurnBackAndGo());
                }
            }
        }
    }

    IEnumerator TurnBackAndGo()
    {
        if (HumanScriptReference != null)
        {
            HumanScriptReference.SetCanMove(false);
        }

        yield return new WaitForSeconds(0.85f);
        
        HumanScriptReference.GetComponent<LimitPositionToScreen>().enabled = false;

        HumanScriptReference.transform.position = whereToTP.transform.position;

        yield return new WaitForSeconds(1.1f);

        HumanScriptReference.GetComponent<LimitPositionToScreen>().enabled = true;

        //float rotation = 0f;
        while (progression <= 1)
        {
            charaReference.transform.rotation = Quaternion.Lerp(initialRotation, new Quaternion (-0.5f, -0.5f, 0.5f, 0.5f), progression);
               // new Vector3(0f, rotation, 0f));
            //rotation = rotation - 1f;
            progression += 0.05f; 
            yield return new WaitForSeconds(0.05f);
        }
        for (int i = 0; i < 5; i++)
        {
            charaRbReference.AddForce(-Vector3.right * 500f);
            yield return new WaitForSeconds(0.75f);
        }

        invisibleWallToPop.SetActive(true);

        if (HumanScriptReference != null)
        {
            HumanScriptReference.SetCanMove(true);
        }
        Destroy(gameObject);
    }
}
