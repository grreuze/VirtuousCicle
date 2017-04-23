using UnityEngine;

public class SC_GameManager : MonoBehaviour {

    public PlayerController[] players;
    
    public static SC_GameManager instance;

    private void Awake() {
        instance = this;
    }
}
