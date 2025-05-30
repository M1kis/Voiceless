using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Pause_Mode : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused;

    private float previousTimeScale = 1f;

    // Guardar el estado previo del cursor
    private bool previousCursorVisible;
    private CursorLockMode previousCursorLockState;

    private void Start()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        if (!isPaused)
        {
            // Guardar el timeScale y el estado del cursor antes de pausar
            previousTimeScale = Time.timeScale;
            previousCursorVisible = Cursor.visible;
            previousCursorLockState = Cursor.lockState;

            isPaused = true;
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.SetActive(true);
            AudioListener.pause = true;
        }
        else
        {
            isPaused = false;
            // Solo restaurar el timeScale si antes era > 0 (no pausar/despausar si otro script lo puso en 0)
            if (previousTimeScale > 0f)
                Time.timeScale = previousTimeScale;

            pauseMenu.SetActive(false);
            AudioListener.pause = false;

            // Restaurar el estado previo del cursor
            Cursor.lockState = previousCursorLockState;
            Cursor.visible = previousCursorVisible;

            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (previousTimeScale > 0f)
            Time.timeScale = previousTimeScale;
        pauseMenu.SetActive(false);
        AudioListener.pause = false;

        // Restaurar el estado previo del cursor
        Cursor.lockState = previousCursorLockState;
        Cursor.visible = previousCursorVisible;

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("Main_Menu");
    }
}
