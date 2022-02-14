using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameBtn : MonoBehaviour
{
    
    public void OnSaveBtnClick()
    {
        SaveGameManager.SaveGame();
    }

    public void OnLoadGameClick()
    {
        SaveGameManager.LoadGame();
    }
}
