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

    [SerializeField]
    GameObject[] subMenus;

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

    public void SelfDestruct()
    {
        GameManager.Instance.SelfDestruct(Team.TeamType.TeamA);
        Resume();
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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetMenu(0);

    }

    public void Pause()
    {
        Time.timeScale = 0;
        toggleObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SetMenu(0);
    }

    public void SetMenu(int index)
    {
        if (index >= subMenus.Length || index < 0) index = 0; // fix bad value.

        for (int i = 0; i < subMenus.Length; i++)
        {
            if (i == index)
            {
                subMenus[i].SetActive(true);
            }
            else
            {
                subMenus[i].SetActive(false);
            }
        }
    }
}
