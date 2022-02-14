using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public static class SaveGameManager
{
    static string SaveDir = Application.persistentDataPath + "/xenowerkSave.json";
    static SaveGameData currentSaveData;
    static public void SaveGame()
    {
        Player player = GameObject.FindObjectOfType<Player>();
        if(player == null)
        {
            return;
        }

        SaveGameData data = new SaveGameData();
        data.LevelIndex = SceneManager.GetActiveScene().buildIndex;
        data.SavedTime = System.DateTime.Now.ToString();
        data.PlayerData = player.GenerateSaveData();
        string SaveData = JsonUtility.ToJson(data, prettyPrint:true);

        File.WriteAllText(SaveDir, SaveData);
    }

    static public void LoadGame()
    {
        string saveDataString = File.ReadAllText(SaveDir);
        currentSaveData = JsonUtility.FromJson<SaveGameData>(saveDataString);
        SceneManager.LoadScene(currentSaveData.LevelIndex);
        SceneManager.sceneLoaded += OnSceneLoaded;
        

    }

    private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Player player = GameObject.FindObjectOfType<Player>();

        if (player == null)
        {
            return;
        }
        player.UpdateFromSaveData(currentSaveData.PlayerData);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

//structs are only data so we use structs instead of class
[Serializable]
public struct SaveGameData
{
    public SaveGameData(int levelIndex,PlayerSaveData playerData, string saveTime)
    {
        LevelIndex = levelIndex;
        PlayerData = playerData;
        SavedTime = saveTime;
    }
    public int LevelIndex;
    public PlayerSaveData PlayerData;
    public string SavedTime;
}
