using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.IO;
using System.Text;


public class JSONUploader : MonoBehaviour
{
	string fieldName;
	string url;
    string jsonFileFullPath;
	string jsonFileName = "defaultFileName.json";

	//Events
	UnityAction<string> OnErrorAction;
	UnityAction<string> OnCompleteAction;


	public static JSONUploader Initialize ()
	{
		return new GameObject ("JSONUploader").AddComponent <JSONUploader> ();
	}

	public JSONUploader SetUrl (string serverUrl)
	{
		this.url = serverUrl;
		return this;
	}


    public JSONUploader SetJsonFilePath(string filePath)
    {
        this.jsonFileFullPath = filePath;
		this.jsonFileName = Path.GetFileName(jsonFileFullPath);
        return this;
    }


	public JSONUploader SetFileName (string filename)
	{
		this.jsonFileName = filename;
		return this;
	}

	public JSONUploader SetFieldName (string fieldName)
	{
		this.fieldName = fieldName;
		return this;
	}

	//events
	public JSONUploader OnError (UnityAction<string> action)
	{
		this.OnErrorAction = action;
		return this;
	}

	public JSONUploader OnComplete (UnityAction<string> action)
	{
		this.OnCompleteAction = action;
		return this;
	}

	public void Upload ()
	{
		//check/validate fields
		if (url == null)
			Debug.LogError ("Url is not assigned, use SetUrl( url ) to set it. ");
		//...other checks...
		//...

		StopAllCoroutines ();
		StartCoroutine (StartUploading ());
	}


	IEnumerator StartUploading ()
	{
		byte[] textureBytes = null;

        if(File.Exists(jsonFileFullPath))
        {
            string jsonString = File.ReadAllText(jsonFileFullPath);
            textureBytes = Encoding.UTF8.GetBytes(jsonString);
        }
        else
        {
            OnErrorAction ("Can't find json file " + jsonFileFullPath);
            yield break;
			Destroy (this.gameObject);
        }

		WWWForm form = new WWWForm ();
        form.AddField("userName", "CapitanAfrica");
		form.AddBinaryData (fieldName, textureBytes, jsonFileName , "image/json");

		WWW w = new WWW (url, form);

		yield return w;

		if (w.error != null) {
			//error : 
			if (OnErrorAction != null)
				OnErrorAction (w.error); //or OnErrorAction.Invoke (w.error);
		} else {
			//success
			if (OnCompleteAction != null)
				OnCompleteAction (w.text); //or OnCompleteAction.Invoke (w.error);
		}
		w.Dispose ();
		Destroy (this.gameObject);
	}
}