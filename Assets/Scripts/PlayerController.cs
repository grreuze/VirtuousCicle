using UnityEngine;

[System.Serializable]
public class PlayerController {
    [HideInInspector]
    public string name;
    public string horizontalAxis, 
                  verticalAxis, 
                  jumpButton, 
                  interactButton;
    public Color auraColor;
    public PlayerCharacter character;
}
