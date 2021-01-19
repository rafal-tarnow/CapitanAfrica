using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.IO;
using System.Text;


public class JSONDownloader : MonoBehaviour
{
	string fieldName;
	string url;
    string jsonFileFullPath;
	string jsonFileName = "defaultFileName.json";

	//Events
	UnityAction<string> OnErrorAction;
	UnityAction<string> OnCompleteAction;


	public static JSONDownloader Initialize ()
	{
		return new GameObject ("JSONDownloader").AddComponent <JSONDownloader> ();
	}

	public JSONDownloader SetUrl (string serverUrl)
	{
		this.url = serverUrl;
		return this;
	}


    public JSONDownloader SetJsonFilePath(string filePath)
    {
        this.jsonFileFullPath = filePath;
		this.jsonFileName = Path.GetFileName(jsonFileFullPath);
        return this;
    }


	public JSONDownloader SetFileName (string filename)
	{
		this.jsonFileName = filename;
		return this;
	}

	public JSONDownloader SetFieldName (string fieldName)
	{
		this.fieldName = fieldName;
		return this;
	}

	//events
	public JSONDownloader OnError (UnityAction<string> action)
	{
		this.OnErrorAction = action;
		return this;
	}

	public JSONDownloader OnComplete (UnityAction<string> action)
	{
		this.OnCompleteAction = action;
		return this;
	}

	public void Download ()
	{
		//check/validate fields
		if (url == null)
			Debug.LogError ("Url is not assigned, use SetUrl( url ) to set it. ");
		//...other checks...
		//...

		StopAllCoroutines ();
		StartCoroutine (StartDownloading());
	}


	IEnumerator StartDownloading()
	{        
		WWW w = new WWW (url);
		yield return w;

		if (w.error != null) 
        {
			//error : 
			if (OnErrorAction != null)
				OnErrorAction (w.error); //or OnErrorAction.Invoke (w.error);
		} 
        else 
        {
            processJsonData(w.text);
			//success
			if (OnCompleteAction != null)
				OnCompleteAction ("Sucessfull downloaded json file!"); //or OnCompleteAction.Invoke (w.error);
		}
		w.Dispose ();
		Destroy (this.gameObject);
	}

    private void processJsonData(string jsonString)
    {
        #warning 'Obsluga bledu zapisu'
        File.WriteAllText(jsonFileFullPath, jsonString);
    }
}