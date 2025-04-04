using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private static string _savingPath = Application.persistentDataPath + "/save.json";

    public static GameData Load() 
    {
        if (File.Exists(_savingPath)) 
        {
            var json = File.ReadAllText(_savingPath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            return data;
        }
        else 
        {
            return null;
        }
    }

    public static void Save(GameData data) 
    {
        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_savingPath, json);
        print("Saved!" + " " + _savingPath);
    }
}
