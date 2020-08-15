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
            SAVE_FOLDER = GetAndroidExternalFilesDir() + "/CapitanAfrica";
            
            if(!Directory.Exists(SAVE_FOLDER))
            {
                Directory.CreateDirectory(SAVE_FOLDER);
            }
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

    public static void Save<T>(T saveObject, string fileName)
    {
        string jsonString = JsonUtility.ToJson(saveObject);    
        File.WriteAllText(SAVE_FOLDER + "/" + fileName, jsonString);
    }

    public static T Load<T>(string fileName)
    {
        if(File.Exists(SAVE_FOLDER + "/" + fileName))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "/" + fileName);

            T loadedObject = JsonUtility.FromJson<T>(saveString);

            return loadedObject;
        }
        else 
        {
            return default(T);
        }
    }

    private static string GetAndroidExternalFilesDir()
    {
     using (AndroidJavaClass unityPlayer = 
            new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
     {
          using (AndroidJavaObject context = 
                 unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
          {
               // Get all available external file directories (emulated and sdCards)
               AndroidJavaObject[] externalFilesDirectories = 
                                   context.Call<AndroidJavaObject[]>
                                   ("getExternalFilesDirs", (object)null);

               AndroidJavaObject emulated = null;
               AndroidJavaObject sdCard = null;

               for (int i = 0; i < externalFilesDirectories.Length; i++)
               {
                    AndroidJavaObject directory = externalFilesDirectories[i];
                    using (AndroidJavaClass environment = 
                           new AndroidJavaClass("android.os.Environment"))
                    {
                        // Check which one is the emulated and which the sdCard.
                        bool isRemovable = environment.CallStatic<bool>
                                          ("isExternalStorageRemovable", directory);
                        bool isEmulated = environment.CallStatic<bool>
                                          ("isExternalStorageEmulated", directory);
                        if (isEmulated)
                            emulated = directory;
                        else if (isRemovable && isEmulated == false)
                            sdCard = directory;
                    }
               }
               // Return the sdCard if available
               if (sdCard != null)
                    return sdCard.Call<string>("getAbsolutePath");
               else
                    return emulated.Call<string>("getAbsolutePath");
            }
      }
    }

}
