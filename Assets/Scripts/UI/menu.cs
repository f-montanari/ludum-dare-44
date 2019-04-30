using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public void SetupAdventureMode()
    {
        GameProgress.gameMode = GameProgress.GameMode.ADVENTURE;
        SceneManager.LoadScene("Dungeon Generation");
    }

    public void SetupEndlessMode()
    {
        GameProgress.gameMode = GameProgress.GameMode.ENDLESS;
        SceneManager.LoadScene("Dungeon Generation");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
