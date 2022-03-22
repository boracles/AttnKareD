using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneSave : MonoBehaviour
{
    int Sceneselect;

    private void Update()
    {
        if(Sceneselect == 1)
        {
            LoadScene();
        }
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("Tutorial", SceneManager.GetActiveScene().buildIndex);
        Sceneselect = 1;
        PlayerPrefs.SetInt("SceneSelect", Sceneselect);
        PlayerPrefs.Save();



    }

    public void LoadScene()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("Tutorial"));

    }

}
