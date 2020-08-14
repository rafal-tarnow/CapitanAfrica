using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class SaveSystem
{
    private static string SAVE_FOLDER;
    public static void Init()
    {
        Debug.Log("Init save system");

        if (Application.platform == RuntimePlatform.Android)
        {
            SAVE_FOLDER = Application.persistentDataPath;
        }
        else
        {
            SAVE_FOLDER = Application.dataPath + "/Saves";

            if(!Directory.Exists(SAVE_FOLDER))
            {
                Directory.CreateDirectory(SAVE_FOLDER);
            }
        }
    }

    public static void Save<T>(T saveObject)
    {
        string jsonString = JsonUtility.ToJson(saveObject);    
        File.WriteAllText(SAVE_FOLDER + "/1.txt", jsonString);
    }

    public static T Load<T>()
    {
        if(File.Exists(SAVE_FOLDER + "/1.txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "/1.txt");

            T loadedObject = JsonUtility.FromJson<T>(saveString);

            return loadedObject;
        }
        else 
        {
            return default(T);
        }
    }

}
