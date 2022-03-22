using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudManager : MonoBehaviour 
{
	// The Text of the "Save and Upload" button, so we can use it to
	// show the upload status.
	public Text buttonText;

	// Replace these with your own URL and API Key, as provided when you set-up ES3Cloud:
	// https://docs.moodkie.com/easy-save-3/es3-guides/saving-loading-files-to-web-using-es3cloud/
	private string url = "https://hippotnc.synology.me/ES3Cloud.php";
	private string apiKey = "13de814c5d55";

	/* 
	 * This is called when the scene first loads.
	 * We'll use this to download our file from the server if it exists.
	 */
	IEnumerator Start()
	{
		// Create an ES3Cloud object with our URL and API key.
		var cloud = new ES3Cloud (url, apiKey);
		yield return StartCoroutine(cloud.DownloadFile());
		// Now check that no errors occurred.
		if(cloud.isError)
		{
			// If the error code is 3, this means there was no data to download on the server.
			// In this case, we shouldn't throw an error and shouldn't try to load any data.
			if(cloud.errorCode == 3)
				yield break;
			Debug.LogError(cloud.error);
		}

		// Now get all of our Prefab Managers and get them to load their prefabs.
		var prefabMgrs = FindObjectsOfType<ES3CloudPrefabManager>();
		foreach (var prefabMgr in prefabMgrs)
			prefabMgr.Load();
	}

	/*
	 *  We call this when we want to upload our data.
	 * 	This starts the coroutine which uploads the file to the server.
	 */
	public void SaveAndUpload()
	{
		StartCoroutine(UploadFile());
	}

	IEnumerator UploadFile()
	{
		// Get all of our Prefab Managers and get them to save their prefabs locally.
		var prefabMgrs = FindObjectsOfType<ES3CloudPrefabManager>();
		foreach (var prefabMgr in prefabMgrs)
			prefabMgr.Save();

		// If there's no data to upload, do nothing.
		if(!ES3.FileExists())
			yield break;

		// Create an ES3Cloud object with our URL and API key.
		var cloud = new ES3Cloud(url, apiKey);

		// Change the button text to show we're uploading.
		buttonText.text = "Uploading...";

		// Upload the default file to the server.
		yield return StartCoroutine(cloud.UploadFile());
		// Now check that no errors occurred.
		if(cloud.isError)
		{
			buttonText.text = "Error!";
			Debug.LogError(cloud.error);
		}
		else
			// Now change the button to show that the data has been uploaded successfully.
			buttonText.text = "Uploaded!";
	}
}
