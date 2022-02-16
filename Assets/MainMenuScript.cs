using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
   public void StartGameBtnClicked()
    {
        GameplayStatic.StartNewGame();
    }

    public void LoadGameBtnClicked()
    {
        GameplayStatic.LoadGame();
    }
    
    public void ExitGameBtnClicked()
    {
        GameplayStatic.QuitGame();
    }
}
