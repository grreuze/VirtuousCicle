using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_flyAwayTransition : MonoBehaviour
{
    public GameObject characterToAffect;
    GameObject charaReference;
    Rigidbody charaRbReference;
    bool inAnimation = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other == characterToAffect && !inAnimation)
        {
            Debug.Log("collision");
            charaReference = other.transform.parent.gameObject;
            charaRbReference = charaReference.GetComponent<Rigidbody>();
            //charaReference.GetComponent<> //virer les colliders pour éviter les entrées multiples
            inAnimation = true;
            StartCoroutine(TurnBackAndGo());
        }
    }

    IEnumerator TurnBackAndGo()
    {
        Debug.Log("StartTransition");
        float rotation = 0f;
        while (rotation >= -180f)
        {
            charaReference.transform.Rotate(new Vector3(-180f, 0f, 0f));
            rotation = rotation - 5f;
            yield return new WaitForSeconds(0.05f);
        }
        for (int i = 0; i < 5; i++)
        {
            charaRbReference.AddForce(-Vector3.right * 2f);
            yield return new WaitForSeconds(0.75f);
        }
    }
}
