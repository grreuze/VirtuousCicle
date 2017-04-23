using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SetCharacterController : MonoBehaviour {

    BoxCollider coll;
    public Character characterToAffect;
    [Tooltip("Set to true to enable the Character, false to disable it.")]
    public bool setActive;
    [Tooltip("Leave empty to affect the Character entering the trigger")]
    public PlayerCharacter specificCharacter;

    public enum Character {
        All, Otter, Human, Bird
    }
    System.Type[] characterComponent = {
        typeof(PlayerCharacter), typeof(FloatingController), typeof(SC_PlayerHuman), typeof(BirdController)
    };

    private void Start() {
        coll = GetComponent<BoxCollider>();
        coll.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) {
        PlayerCharacter character = other.GetComponentInParent<PlayerCharacter>();
        if (character) {
            if (specificCharacter)
                character = specificCharacter;

            if (character.GetType() == characterComponent[(int)characterToAffect]) {

                if (setActive) {
                    character.EnableController();
                    print("Succesfully enabled " + characterToAffect);
                } else {
                    character.DisableController();
                    print("Succesfully disabled " + characterToAffect);
                }
            }
        }
    }

    #region Gizmo
    [Header("Gizmo")]
    [SerializeField]
    bool drawGizmo;
    [SerializeField]
    Color gizmoColor;
    private void OnDrawGizmos() {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = rotationMatrix;
        if (!coll) Start();

        if (drawGizmo) {
            Gizmos.color = gizmoColor;
            Gizmos.DrawCube(coll.center, coll.size);
        }
    }
    #endregion
}
