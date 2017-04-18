using UnityEngine;

public class SC_MultiTargetCamera : MonoBehaviour {

    public Transform[] targets;
    public Vector3 offset;
    public MinMax distance = new MinMax(7, 20);
    public float currentDistance = 14,
                 smooth = 2,
                 distanceModifier = 1.5f;

    Transform my;

    void Start() {
        my = transform;
    }

    void Update() {
        
        Vector3 camPos = (targets[1].position + targets[0].position) / 2 + offset;
        my.LookAt(camPos);

        currentDistance = distance.Clamp(Vector3.Distance(targets[0].position, targets[1].position) * distanceModifier);

        camPos.z -= currentDistance;

        my.position = Vector3.Slerp(my.position, camPos, smooth * Time.deltaTime);
    }   
    
}
