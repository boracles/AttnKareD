using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 
 * This class will manage the creation of prefabs, including loading and saving them.
 * It will also store a list of all of the prefabs we've created.
 * 
 */
public class ES3CloudPrefabManager : MonoBehaviour 
{
	// The prefab we want to create.
	public GameObject prefab;
	// An automatically-generated unique identifier for this type of prefab.
	// We will append this to our keys when saving to identifiy different types
	// of prefab in the save file.
	public string id = System.Guid.NewGuid().ToString();

	// A List which we'll add the Transforms of our created prefabs to.
	private List<Transform> prefabInstances = new List<Transform>();

	/*
	 * This is where we will save our data.
	 * This is called by the 'Save and Upload' button. 
	 */
	public void Save()
	{
		// Save the number of created prefabs there are.
		ES3.Save<int>(id+"count", prefabInstances.Count);
		// Save our Transforms.
		ES3.Save<List<Transform>>(id, prefabInstances);
	}

	/*
	 * 	We will call this after downloading the data from the server.
	 */
	public void Load() 
	{
		// Load how many prefabs we need to instantiate.
		int count = ES3.Load<int>(id + "count", 0);
		// If there are prefabs to load, load them.
		if(count > 0)
		{
			// For each prefab we want to load, instantiate a prefab.
			for(int i = 0; i < count; i++)
				InstantiatePrefab();
			// Load our List of Transforms into our prefab array.
			ES3.LoadInto<List<Transform>>(id, prefabInstances);
		}
	}

	/*
	 * 	Creates an instance of the prefab and adds it to the instance list.
	 * 	You should call this instead of Unity's Instantiate method.
	 */
	public GameObject InstantiatePrefab()
	{
		var go = Instantiate(prefab);
		prefabInstances.Add(go.transform);
		return go;
	}

	/*
	 * Instantiates the prefab at a random position and with a random rotation.
	 */
	public void CreateRandomPrefab()
	{
		var go = InstantiatePrefab();
		go.transform.position = Random.insideUnitSphere * 5;
		go.transform.rotation = Random.rotation;
	}

	/*
	 *  Deletes all prefab instances from the scene and from the save file.
	 */
	public void DeletePrefabs()
	{
		// Delete the keys relating to this prefab.
		ES3.DeleteKey(id);
		ES3.DeleteKey(id+"count");
		// Destroy each created prefab, and then clear the List.
		for(int i=0; i<prefabInstances.Count; i++)
			Destroy (prefabInstances[i].gameObject);
		prefabInstances.Clear();
	}
}
