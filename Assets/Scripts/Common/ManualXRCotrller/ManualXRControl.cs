using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using UnityEngine.SceneManagement;


public class ManualXRControl : MonoBehaviour
{

    private static ManualXRControl instance;
    
    public static ManualXRControl GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<ManualXRControl>();

            if(instance == null)
            {
                GameObject container = new GameObject("ManualXRControl");

                instance = container.AddComponent<ManualXRControl>();
            }
        }

        return instance;
    }

    public IEnumerator StartXR()
    {
        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
        }
        else
        {
            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
    }

    void StopXR()
    {
        Debug.Log("Stopping XR...");

        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Debug.Log("XR stopped completely.");
    }

    public void XR_AutoStarter()
    {
        int sceneIndex;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex == 0)
        {
            if (XRSettings.enabled)
            {
                StopXR();
                Debug.Log("Stop XR");
            }            
        }
        else
        {
            if(!XRSettings.enabled)
            {
                Debug.Log("Start XR");
                StartCoroutine("StartXR");
            }            
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        StopXR();
    }

    private void OnApplicationFocus(bool isApplicationHasFocus)
    {
        Debug.Log("isApplicationHasFocus" + isApplicationHasFocus);
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance != null)
        {
            if(instance != this)
            {
                Destroy(instance.gameObject);
                return;
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        XR_AutoStarter();
    }

    private void OnDisable()
    {
        StopXR();
    }   
}

