using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using KetosGames.SceneTransition;

public class KeyInput : MonoBehaviour
{
    private static KeyInput Instance;

    private InputMoveScene inputMoveScene;

    private int buildIndex;    

    public static KeyInput GetInstance()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<KeyInput>();

            if(Instance == null)
            {
                GameObject container = new GameObject("KeyInput");

                Instance = container.AddComponent<KeyInput>();
            }
        }
        return Instance;
        }
    

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (Instance != null)
        {
            if (Instance != this)
            {
                Destroy(Instance.gameObject);
                return;
            }
        }
        inputMoveScene = new InputMoveScene();
    }
    
    private void OnEnable()
    {        
        inputMoveScene.SceneCotrolMap.MoveToScene_1.performed += GoToScene_1;
        inputMoveScene.SceneCotrolMap.MoveToScene_2.performed += GoToScene_2;
        inputMoveScene.SceneCotrolMap.MoveToScene_3.performed += GoToScene_3;
        inputMoveScene.SceneCotrolMap.MoveToScene_4.performed += GoToScene_4;
        inputMoveScene.SceneCotrolMap.MoveToScene_5.performed += GoToScene_5;
        inputMoveScene.SceneCotrolMap.MoveToScene_6.performed += GoToScene_6;
        inputMoveScene.SceneCotrolMap.MoveToScene_7.performed += GoToScene_7;
        inputMoveScene.SceneCotrolMap.MoveToScene_8.performed += GoToScene_8;
        inputMoveScene.SceneCotrolMap.MoveToScene_9.performed += GoToScene_9;
        inputMoveScene.SceneCotrolMap.MoveToScene_10.performed += GoToScene_10;
        inputMoveScene.SceneCotrolMap.MoveToScene_11.performed += GoToScene_11;
        inputMoveScene.SceneCotrolMap.MoveToScene_14.performed += GoToScene_14;
        inputMoveScene.SceneCotrolMap.MoveToScene_1.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_2.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_3.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_4.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_5.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_6.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_7.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_8.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_9.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_10.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_11.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_14.Enable();
    }

    private void OnDisable()
    {
        inputMoveScene.SceneCotrolMap.MoveToScene_1.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_2.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_3.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_4.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_5.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_6.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_7.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_8.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_9.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_10.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_11.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_14.Disable();
    }

    private void GoToScene_1(InputAction.CallbackContext obj)
    {
        buildIndex = 1;
        SceneLoader.LoadScene(buildIndex);
    }

    private void GoToScene_2(InputAction.CallbackContext obj)
    {
        buildIndex = 2;
        SceneLoader.LoadScene(buildIndex);
    }
    private void GoToScene_3(InputAction.CallbackContext obj)
    {
        buildIndex = 3;
        SceneLoader.LoadScene(buildIndex);
    }
    private void GoToScene_4(InputAction.CallbackContext obj)
    {
        buildIndex = 4;
        SceneLoader.LoadScene(buildIndex);
    }
    private void GoToScene_5(InputAction.CallbackContext obj)
    {
        buildIndex = 5;
        SceneLoader.LoadScene(buildIndex);
    }
    private void GoToScene_6(InputAction.CallbackContext obj)
    {
        buildIndex = 6;
        SceneLoader.LoadScene(buildIndex);
    }

    private void GoToScene_7(InputAction.CallbackContext obj)
    {
        buildIndex = 7;
        SceneLoader.LoadScene(buildIndex);
    }
    private void GoToScene_8(InputAction.CallbackContext obj)
    {
        buildIndex = 8;
        SceneLoader.LoadScene(buildIndex);
    }
    private void GoToScene_9(InputAction.CallbackContext obj)
    {
        buildIndex = 9;
        SceneLoader.LoadScene(buildIndex);
    }
    private void GoToScene_10(InputAction.CallbackContext obj)
    {
        buildIndex = 10;
        SceneLoader.LoadScene(buildIndex);
    }
    private void GoToScene_11(InputAction.CallbackContext obj)
    {
        UserData.DataManager.GetInstance().isPlayed = false;
        UserData.DataManager.GetInstance().FilePath_Root = null;
        UserData.DataManager.GetInstance().FilePath_Folder = null;
        buildIndex = 0;
        SceneLoader.LoadScene(buildIndex);
    }
    private void GoToScene_14(InputAction.CallbackContext obj)
    {
        buildIndex = 14;
        SceneLoader.LoadScene(buildIndex);
    }
}
