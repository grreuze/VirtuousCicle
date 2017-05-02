using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SC_FadeOutTransition : MonoBehaviour {

    [Header("References")]
    public Image blackOut;
    public GameObject canvas;

    [Header("Tweaking")]
    public float transitionSpeed;
    public bool delayed;
    public float delayDuration;
    public bool addInverseTransition;

    float progression = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (delayed)
            {
                StartCoroutine(Waiting());
            }
            else
            {
                StartCoroutine(SmoothTransition());
            }
        }
    }

    IEnumerator Waiting()
    {
        for (int i = 0; i < delayDuration; i++)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine(SmoothTransition());
    }

    IEnumerator SmoothTransition()
    {
        canvas.SetActive(false);
        while (progression <= 1)
        {
            blackOut.color = new Vector4(0f, 0f, 0f, Mathf.Lerp(0f, 1f, progression));
            progression = progression + 0.1f * transitionSpeed;
            yield return new WaitForSeconds(0.05f);
        }
        blackOut.color = new Vector4(0f, 0f, 0f, 1f);
        if (addInverseTransition)
        {
            while (progression >= 0)
            {
                blackOut.color = new Vector4(0f, 0f, 0f, Mathf.Lerp(0f, 1f, progression));
                progression = progression - 0.1f * transitionSpeed;
                yield return new WaitForSeconds(0.05f);
            }
            canvas.SetActive(true);
        }
        Destroy(gameObject);
    }
}
