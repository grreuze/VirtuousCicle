using UnityEngine;

public abstract class PlayerCharacter : MonoBehaviour {

    public int playerController;
    public bool enabledOnStart;

    /// <summary>
    /// The Controller of the playable character (either Player 1 or Player 2).
    /// </summary>
    protected PlayerController controller;
    ParticleSystem aura;
    ParticleSystem.MainModule auraMain;
    new SC_MultiTargetCamera camera;
    LimitPositionToScreen movementLimiter;

    private void Awake() {
        camera = Camera.main.GetComponent<SC_MultiTargetCamera>();
        movementLimiter = GetComponent<LimitPositionToScreen>();
        aura = GetComponentInChildren<ParticleSystem>();
        auraMain = aura.main;

        SetController(playerController);

        if (enabledOnStart) 
            EnableController();
        else
            DisableController();
    }

    /// <summary>
    /// Sets a new Controller for the playable character. Don't forget to enable it afterward.
    /// </summary>
    /// <param name="newController"> The character's new Controller. Take it from the GameManager.  </param>
    public void SetController(int controllerNumber) {
        PlayerController newController = SC_GameManager.instance.players[controllerNumber];
        controller = newController;
        auraMain.startColor = controller.auraColor;
    }

    public void EnableController() {
        controller.isActive = true;
        // Enable Aura
        if (aura.isStopped)
            aura.Play();
        // Add to Camera Targets
        if (!camera.targets.Contains(transform))
            camera.targets.Add(transform);
        // Limit Movement to Viewport
        movementLimiter.enabled = true;
    }

    public void DisableController() {
        controller.isActive = false;
        // Disable Aura
        if (aura.isPlaying)
            aura.Stop();
        // Remove from Camera Targets
        if (camera.targets.Contains(transform))
            camera.targets.Remove(transform);
        // Allow going offscreen
        movementLimiter.enabled = false;
    }
}
