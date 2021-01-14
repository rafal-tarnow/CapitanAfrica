using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Paths : MonoBehaviour
{
    enum AndroidMemType
    {
        EMULATED,
        SD_CARD
    }

    public static string LEVELS_EDIT;
    public static string LEVEL_TEMPLATE = "Assets/Resources/Levels/";

    private static bool inited = false;

    static Paths()
    {
        if(inited == true) //if arleady inited then return
            return;

        Debug.Log("Init path system");

        if (Application.platform == RuntimePlatform.Android)
        {
            LEVELS_EDIT = GetAndroidExternalFilesDir(AndroidMemType.EMULATED) + "/CapitanAfrica/";
            
            if(!Directory.Exists(LEVELS_EDIT))
            {
                Directory.CreateDirectory(LEVELS_EDIT);
            }
        }
        else
        {
            LEVELS_EDIT = Application.dataPath + "/Saves/";

            if(!Directory.Exists(LEVELS_EDIT))
            {
                Directory.CreateDirectory(LEVELS_EDIT);
            }
        }
    }

    
    
    private static string GetAndroidExternalFilesDir(AndroidMemType memType)
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
               //if (sdCard != null)
               //     return sdCard.Call<string>("getAbsolutePath");
               //else
               //     return emulated.Call<string>("getAbsolutePath");

                if(memType == AndroidMemType.SD_CARD)
                {
                    if (sdCard != null)
                        return sdCard.Call<string>("getAbsolutePath");
                    else
                        return emulated.Call<string>("getAbsolutePath");
                }
                else
                {
                    return emulated.Call<string>("getAbsolutePath");
                }
            }
      }
    }
}
