using System.Collections;
using UnityEngine;

public class BirdTransition : MonoBehaviour {

    [SerializeField]
    BirdController bird;
    ScreenOverlay overlay;
    LimitPositionToScreen limiter;
    
    public float transitionTime;
    public float waitingTime;

    bool flag = true;

    void Start() {
        overlay = Camera.main.GetComponent<ScreenOverlay>();
        limiter = bird.GetComponent<LimitPositionToScreen>();
    }

    private void OnTriggerEnter(Collider other) {
        if (flag) {
            flag = false;
            Fade(0, 1);
        }
    }

    void Fade(float from, float to) {
        StartCoroutine(_Fade(from, to));
    }

    IEnumerator _Fade(float from, float to) {
        Color color = overlay.color;
        color.a = from;
        overlay.color = color;

        for (float elapsed = 0; elapsed < transitionTime; elapsed += Time.deltaTime) {
            float t = elapsed / transitionTime;
            color.a = Mathf.Lerp(from, to, t);
            overlay.color = color;
            yield return null;
        }

        color.a = to;
        overlay.color = color;
        
        if (to == 1) {
            WhenScreenIsBlack();
        } else if (to == 0) {
            WhenTransitionOver();
        }
    }

    void WhenScreenIsBlack() {
        StartCoroutine(_WhenScreenIsBlack());
    }
    IEnumerator _WhenScreenIsBlack() {
        bird.EnableController();
        limiter.enabled = false;
        yield return new WaitForSeconds(waitingTime);
        Fade(1, 0);
    }

    void WhenTransitionOver() {
        limiter.enabled = true;
        this.enabled = false;
    }
}
