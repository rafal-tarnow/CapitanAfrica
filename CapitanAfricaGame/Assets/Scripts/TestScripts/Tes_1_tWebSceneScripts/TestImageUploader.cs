using UnityEngine;

public class TestImageUploader : MonoBehaviour
{
	[SerializeField] SpriteRenderer imageSprite;
	[SerializeField] SpriteRenderer imageSprite2;
	[SerializeField] string serverUrl;

	void Start ()
	{
		ImageUploader
			.Initialize ()
			.SetUrl (serverUrl)
			.SetTexture (imageSprite.sprite.texture)
			.SetFieldName ("myimage")
			.SetFileName ("warrior")
			.SetType (ImageType.JPG)
			.OnError (error => Debug.Log (error))
			.OnComplete (text => Debug.Log (text))
			.Upload ();

		for(int i = 0; i < 35; i++)
		{
		JSONUploader
			.Initialize ()
			.SetUrl (serverUrl)
			.SetJsonFilePath (Paths.LEVELS_EDIT + i.ToString() + ".txt")
			.SetFieldName ("myimage")
			.OnError (error => Debug.Log (error))
			.OnComplete (text => Debug.Log (text))
			.Upload ();
		}

		ImageUploader
			.Initialize ()
			.SetUrl (serverUrl)
			.SetTexture (imageSprite2.sprite.texture)
			.SetFieldName ("myimage")
			.Upload ();
	}
}
