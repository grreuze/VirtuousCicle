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
        offset.z = 0;
    }

    void Update() {
        Vector3 camPos = offset;
        if (targets.Count > 0) {
            bool obstacle = false;
            foreach (Transform target in targets) {
                camPos += target.position;

                RaycastHit hit;
                if (Physics.Linecast(transform.position, target.position, out hit) && hit.transform.tag != "Player") {
                    obstacle = true;
                    offset.z -= 1;
                    print(hit.transform.name + " entre caméra et " + target);
                } else if (offset.z != 0 && Physics.Linecast(transform.position + Vector3.forward*5, target.position, out hit) && hit.transform.tag != "Player") {
                    print("double check succesful");
                    obstacle = true;
                }

            }
            camPos /= targets.Count;
            if (!obstacle && offset.z < 0) {
                offset.z += 1;
                offset.z = Mathf.Min(offset.z, 0);
            }

            if (targets.Count > 1) {
                currentDistance = distance.Clamp(Vector3.Distance(targets[0].position, targets[1].position) * distanceModifier);
            }
        }
        
        camPos.z -= currentDistance;

        my.position = Vector3.Slerp(my.position, camPos, smooth * Time.deltaTime);
    }   
    
    void OnValidate() {
        offset.z = 0;
    }
}
