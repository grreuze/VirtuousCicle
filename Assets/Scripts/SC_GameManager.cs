using UnityEngine;

public class SC_GameManager : MonoBehaviour {
    
    public PlayerController player1, player2;

    public static SC_GameManager instance;

    private void Awake() {
        instance = this;
    }

}
