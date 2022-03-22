using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.Rendering;
using KetosGames.SceneTransition;
using UserData;
using BNG;
    
    public class EndingManager : MonoBehaviour
{

    public GameObject PlayerController;
    public GameObject Ghost;
    public GameObject Door;
    public Volume global;
    public Transform Logo;
    public Transform TransitionIN;
    public Transform TransitionOut;

    public NetworkManager NetworkManager;
    public AutoVoiceRecording_Ending VoiceRecord;



    public float speed;
    public Transform SpeechBubble;

    private float Timer = 0;
    private string state;
    bool fadeColor = false;
    float GhostRot;
    int audioIndex;
    string childCode;
    string prevChildCode;
    Vector3 desPos;
    DataManager dataManager;
    VolumeProfile globalVolume;

    ColorAdjustments _coloradjustment = null;





   
    // Start is called before the first frame update
    void Start()
    {


        globalVolume = global.sharedProfile;
        globalVolume.TryGet<ColorAdjustments>(out _coloradjustment);


            Ghost.transform.position = new Vector3(0.031f, -0.56f, 6.695f);
            _coloradjustment.saturation.value = -10f;
            StartCoroutine(StartEnding());


    }

    // Update is called once per frame
    void Update()
    {
        float GhostRot = Ghost.transform.eulerAngles.y;
        SpeechBubble.localPosition = new Vector3(Ghost.transform.localPosition.x + 0.482f, Ghost.transform.localPosition.y + 0.865f, Ghost.transform.localPosition.z);
        Logo.localEulerAngles = new Vector3(Logo.localEulerAngles.x, GhostRot, Logo.localEulerAngles.z);

        globalVolume = global.sharedProfile;
        globalVolume.TryGet<ColorAdjustments>(out _coloradjustment);
        if (fadeColor)
        {
            
                Timer += Time.deltaTime;
            if (Timer > 0.02f)
            {
                Timer = 0;
                _coloradjustment.saturation.value -= 1f;

            }


        }
    }

    private void LateUpdate()
    {
        globalVolume = global.sharedProfile;
        globalVolume.TryGet<ColorAdjustments>(out _coloradjustment);

        if (_coloradjustment.saturation.value == -100f )
        {
            fadeColor = false;
        }



    }





    IEnumerator LerpDoor()
    {
        float t = 0;
        while (true)
        {
            Door.transform.localEulerAngles = Vector3.Lerp(new Vector3(Door.transform.localEulerAngles.x, Door.transform.localEulerAngles.y, Door.transform.localEulerAngles.z), new Vector3(Door.transform.localEulerAngles.x, 106.108f, Door.transform.localEulerAngles.z), t);

            t += Time.deltaTime;

            if (Door.transform.localEulerAngles == new Vector3(Door.transform.localEulerAngles.x, 106.108f, Door.transform.localEulerAngles.z))
            {
                yield break;
            }

            yield return null;
        }
    }





    IEnumerator LerpLOGO()
    {


        Logo.gameObject.SetActive(true);
        float t = 0f;
        while (true)
        {
            t += Time.deltaTime;

            Logo.transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(5050f, 5050f, 5050f), t);

            if (Logo.transform.localScale == new Vector3(5050f, 5050f, 5050f))
            {
                yield return new WaitForSeconds(3f);
                Logo.transform.localScale = Vector3.Lerp(new Vector3(5050, 5050, 5050), new Vector3(0, 0, 0), t);

                yield break;
            }

            yield return null;
        }

    }




    
    IEnumerator StartEnding()
    {
        
        TransitionIN.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        TransitionIN.gameObject.SetActive(false);
        //������ �޷�����
        //bool go = true;

        StartCoroutine(LerpDoor());
        desPos = new Vector3(0.031f, -0.56f, 3.734f);
        StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.5f));
        yield return new WaitForSeconds(3.0f);
     


        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.06>�ƽ�����\n���� ����� \n�ð��� �ƾ�", audioIndex = 0, 2.5f));
        yield return new WaitForSeconds(3.0f);
        VoiceRecord.StartRecording();
        if(DataManager.GetInstance().userInfo.Grade!=null)
        {
            
            if (DataManager.GetInstance().userInfo.Grade == "L")
            {

                StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.05>���� ��й�ȣ ������,\n��ȹǥ �����,\nå���� ì���,\n�� �ű�⸦ �غôµ�", audioIndex = 1,7.0f));
                yield return new WaitForSeconds(7.0f);

            }
            if (DataManager.GetInstance().userInfo.Grade == "H")
            {
                StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.05>���� ������ �ϱ�,\n��� ������,\nå���� ì���,\n�� �ű�⸦ �غôµ�", audioIndex = 2,7.0f));
                yield return new WaitForSeconds(7.0f);


            }
        }
        if(DataManager.GetInstance().userInfo.Grade==null)
        {
            
            StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.06>���� ���� ���Է�", audioIndex = 0,3.0f));
            yield return new WaitForSeconds(3.0f);
        }
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.052>���� ����־���\n�̼��� ����?\n�׸��� �� ��� �̼���\n���� �������?", audioIndex = 3,7.0f));
        yield return new WaitForSeconds(7.0f);

        yield return new WaitForSeconds(8.0f);
        // if ������ ���г� ���г� �и�
        VoiceRecord.StopRecordingNBehavior();
        desPos = new Vector3(-0.02f, -0.693f, 2.795f);
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.06>�׷�����~!", audioIndex = 4,3.0f));
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.05>���� ���� �ʿ� �Բ��ؼ� \n�ʹ� �ʹ� ��ſ���", audioIndex = 5,3.0f));
        NetworkManager.DoSendToFinishData();
        yield return new WaitForSeconds(4.0f);
        StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.2f));
        yield return new WaitForSeconds(1.8f);

        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.055>�ٸ� ģ���� ��ٸ��� �־\n�̸� �������Ұ� ����!", audioIndex = 6, 3.0f));
        yield return new WaitForSeconds(4.0f);
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.055>������ �� ���\n��ٸ���!", audioIndex = 7, 3.0f));
        yield return new WaitForSeconds(4.0f);


        fadeColor = true;
        SpeechBubble.gameObject.SetActive(false);
        Ghost.GetComponent<Animator>().SetBool("isJump", true);
       
        yield return new WaitForSeconds(3.0f);

        TransitionOut.gameObject.SetActive(true);
        Ghost.transform.parent.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.2f);


    //    Application.Quit();





    }

    








}
