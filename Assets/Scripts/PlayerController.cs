using UnityEngine;

[System.Serializable]
public struct PlayerController {
    
    public string horizontalAxis, 
                  verticalAxis, 
                  jumpButton, 
                  interactButton;
    public Color auraColor;
    public bool isActive;
}
