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

    public static void Save<T>(T saveObject, string fileName)
    {
        string jsonString = JsonUtility.ToJson(saveObject);    
        File.WriteAllText(Paths.LEVELS_EDIT + fileName, jsonString);
    }

    public static T Load<T>(string fileName)
    {

        if(!File.Exists(Paths.LEVELS_EDIT + fileName))
        {
            File.Copy(Paths.LEVEL_TEMPLATE + "template_level.txt", (Paths.LEVELS_EDIT + fileName), false);
        }

        if(File.Exists(Paths.LEVELS_EDIT + fileName))
        {
            string saveString = File.ReadAllText(Paths.LEVELS_EDIT + fileName);
            T loadedObject = JsonUtility.FromJson<T>(saveString);
            return loadedObject;
        }
        else 
        {
            return default(T);
        }
    }
}
