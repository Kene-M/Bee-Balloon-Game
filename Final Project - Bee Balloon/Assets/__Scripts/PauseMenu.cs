using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    public static bool isPaused = false; // Pause status
    bool isSpawned; // Check for current bee spawned status.

    public static PauseMenu S;

    public void Awake()
    {
        S = this;
    }

    public void Pause()
    {
        /*if (isPaused)
        {
            Continue();
            return;
        }*/

        isSpawned = Main.S.spawnButton.isActiveAndEnabled;
        Main.S.spawnButton.gameObject.SetActive(false);
        
        PausePanel.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void Continue()
    {
        Main.S.spawnButton.gameObject.SetActive(isSpawned);

        PausePanel.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
