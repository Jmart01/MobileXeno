using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class GameplayStatic
{
    static int startLevelBuildIndex = 1;
    public static void StartNewGame()
    {
        SceneManager.LoadScene(startLevelBuildIndex);
    }

    public static void LoadGame()
    {
        SaveGameManager.LoadGame();
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
