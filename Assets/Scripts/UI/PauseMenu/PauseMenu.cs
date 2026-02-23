using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    GameObject toggleObject;

    [SerializeField]
    int mainMenuIndex = 0;

    InputAction pauseAction;

    void Start()
    {
        pauseAction = InputSystem.actions.FindAction("Pause", true);
        pauseAction.performed += TogglePaused;
        Resume(); // This may bite me later if I do a tutorial.
    }

    private void TogglePaused(InputAction.CallbackContext context)
    {
        if (toggleObject.activeSelf)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        toggleObject.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        toggleObject.SetActive(true);
    }
}
