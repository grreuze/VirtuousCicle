using UnityEngine;

public class LimitPositionToScreen : MonoBehaviour {
    
    [SerializeField]
    bool drawGizmo = true;

    Transform my, camTrans;
    Camera cam;
    Vector3 zero = Vector3.zero, 
             one = Vector3.one,
                  min, max;
    
    void Awake() {
        my = transform;
        cam = Camera.main;
        camTrans = cam.transform;
    }

    void LateUpdate() {
        zero.z = one.z = my.position.z - camTrans.position.z;

        min = cam.ViewportToWorldPoint(zero);
        max = cam.ViewportToWorldPoint(one);

        float x = Mathf.Clamp(my.position.x, min.x, max.x);
        float y = Mathf.Clamp(my.position.y, min.y, max.y);
        float z = my.position.z;

        my.position = new Vector3(x, y, z);
    }

    void OnDrawGizmos() {
        if (drawGizmo) {
            Gizmos.color = Color.cyan;

            Vector3 wireCubePosition = (max + min) / 2;
            Vector3 wireCubeSize = max - min;

            Gizmos.DrawWireCube(wireCubePosition, wireCubeSize);
        }
    }
}