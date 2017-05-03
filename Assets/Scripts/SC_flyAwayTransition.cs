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

    float progression = 0;
    Vector3 initialRotation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.gameObject == characterToAffect && !inAnimation)
        {
            Debug.Log("collision");
            charaReference = other.transform.parent.gameObject;
            charaRbReference = charaReference.GetComponent<Rigidbody>();
            //charaReference.GetComponent<> //virer les colliders pour éviter les entrées multiples
            inAnimation = true;
            initialRotation = charaReference.transform.rotation.eulerAngles;
            StartCoroutine(TurnBackAndGo());
        }
    }

    IEnumerator TurnBackAndGo()
    {
        Debug.Log("StartTransition");
        //float rotation = 0f;
        while (progression <= 1)
        {
            Debug.Log(charaReference.transform.rotation.eulerAngles);
            charaReference.transform.Rotate(Vector3.Lerp(initialRotation, new Vector3 (initialRotation.x, 90f, -90f), progression));
               // new Vector3(0f, rotation, 0f));
            //rotation = rotation - 1f;
            progression += 0.05f; 
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < 5; i++)
        {
            Debug.Log("two");
            charaRbReference.AddForce(-Vector3.right * 2f);
            yield return new WaitForSeconds(0.75f);
        }
    }
}
