using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    [SerializeField]
    Canvas pauseMenu;
    
    bool paused = false;
    float lastTimeInput;

    void Start() {
        pauseMenu.enabled = paused;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            paused ^= true;
            pauseMenu.enabled = paused;
            Time.timeScale = paused ? 0 : 1;
        }
    }

    public void Return() {
        paused = false;
        pauseMenu.enabled = false;
        Time.timeScale = 1;
    }

    public void Restart() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit() {
        Application.Quit();
    }
}
