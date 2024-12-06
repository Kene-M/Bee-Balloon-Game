using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    public static bool isPaused = false;


    public void Pause()
    {
        //Main.S.spawnButton.enabled? ;
        //targetPoint = (targetPoint == pointA) ? pointB : pointA;
        //Main.S.spawnButton.enabled = (Main.S.spawnButton.enabled)? false : false;

        PausePanel.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void Continue()
    {
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
