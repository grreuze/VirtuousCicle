using UnityEngine;
using System.Collections.Generic;

public class SC_MultiTargetCamera : MonoBehaviour {
    
    public List<Transform> targets;
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
        Vector3 camPos = offset;
        if (targets.Count > 0) {
            foreach (Transform target in targets) {
                camPos += target.position;
            }
            camPos /= targets.Count;

            if (targets.Count > 1) {
                currentDistance = distance.Clamp(Vector3.Distance(targets[0].position, targets[1].position) * distanceModifier);
            }
        }
        
        camPos.z -= currentDistance;

        my.position = Vector3.Slerp(my.position, camPos, smooth * Time.deltaTime);
    }   
    
}
