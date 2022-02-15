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

        Zombie[] zombiesInScene = GameObject.FindObjectsOfType<Zombie>();
        if(zombiesInScene == null)
        {
            return;
        }
        List<EnemySaveData> enemySaveDatas = new List<EnemySaveData>();
        foreach(Zombie zombie in zombiesInScene)
        {
            enemySaveDatas.Add(zombie.GenerateEnemySaveData());
        }

        SaveGameData data = new SaveGameData();
        data.LevelIndex = SceneManager.GetActiveScene().buildIndex;
        data.SavedTime = System.DateTime.Now.ToString();
        data.PlayerData = player.GenerateSaveData();
        data.EnemyData = enemySaveDatas.ToArray();
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

        //creates an empty list
        List<Zombie> zombiesList = new List<Zombie>();
        //finds all zombies in scene
        Zombie[] zombiesInScene = GameObject.FindObjectsOfType<Zombie>();
        //loops through array of zombies
        for (int i = 0; i < zombiesInScene.Length; i++)
        {
            //for each zombie in the array, add that zombie to the zombie list
            zombiesList.Add(zombiesInScene[i]);
        }
        foreach (Zombie zombie in zombiesInScene)
        {
            //for all the enemysavedatas in the array at the bottom of the page, this gathers all the data from each zombie
            foreach (EnemySaveData enemySaveData in currentSaveData.EnemyData)
            {
                //compares the zombie's name to the name in the saved data
                if (zombie.gameObject.name == enemySaveData.Name)
                {
                    zombie.UpdateFromEnemySaveData(enemySaveData);
                    zombiesList.Remove(zombie);
                }
            }
        }
        foreach (Zombie zombie in zombiesInScene)
        {
            GameObject.Destroy(zombie.gameObject);
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

//structs are only data so we use structs instead of class
[Serializable]
public struct SaveGameData
{
    public SaveGameData(int levelIndex,PlayerSaveData playerData, EnemySaveData[] enemyData, string saveTime)
    {
        LevelIndex = levelIndex;
        PlayerData = playerData;
        EnemyData = enemyData;
        SavedTime = saveTime;
    }
    public int LevelIndex;
    public PlayerSaveData PlayerData;
    public EnemySaveData[] EnemyData;
    public string SavedTime;
}
