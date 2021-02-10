using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class SaveSystem
{
    public static void Init()
    {
        Debug.Log("Init save system");
    }

    public static void Save<T>(T saveObject, string filePath)
    {
        string jsonString = JsonUtility.ToJson(saveObject);    
        File.WriteAllText(filePath, jsonString);
    }

    public static T Load<T>(string filePath, string templateFilePath) where T : new()
    {
        if(!File.Exists(filePath))
        {
            if(File.Exists(templateFilePath))
                File.Copy(templateFilePath, filePath, false);
        }

        if(File.Exists(filePath))
        {
            string saveString = File.ReadAllText(filePath);
            T loadedObject = JsonUtility.FromJson<T>(saveString);

            if(loadedObject != null)
                return loadedObject;
            else
                return new T();

        }
        else 
        {
            return new T();
        }
    }
}
