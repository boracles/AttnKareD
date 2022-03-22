using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SimpleSave : MonoBehaviour
{
    public string TutorialLoad;

    public void Save()
    {
      
        ES3.Save("savedTutorial", SceneManager.GetActiveScene().name);
    }

    public void Load()
    {
        TutorialLoad = ES3.Load<string>("savedTutorial", defaultValue:"default");
        SceneManager.LoadScene(TutorialLoad);
    }
}
