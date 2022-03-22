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
public class OpenManager : MonoBehaviour
{

    public GameObject PlayerController;
    public GameObject Ghost;
    public GameObject Door;
    public Volume global;
    public Transform Logo;
    public Transform TransitionIN;
    public Transform TransitionOut;

    public NetworkManager NetworkManager;



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


    void Start()
    {


        globalVolume = global.sharedProfile;
        globalVolume.TryGet<ColorAdjustments>(out _coloradjustment);




        Ghost.transform.position = new Vector3(3.1f, -1.337f, 3.09f);
        _coloradjustment.saturation.value = -100f;
        StartCoroutine(StartOpening());





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
                _coloradjustment.saturation.value += 1f;

            }





        }
    }

    private void LateUpdate()
    {
        globalVolume = global.sharedProfile;
        globalVolume.TryGet<ColorAdjustments>(out _coloradjustment);

        if (_coloradjustment.saturation.value == -10f && state != "END")
        {
            fadeColor = false; ; //fading 끝
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




    IEnumerator StartOpening()
    {
        TransitionIN.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);


        TransitionIN.gameObject.SetActive(false);
        Ghost.transform.parent.gameObject.SetActive(true);
        Ghost.transform.localEulerAngles = new Vector3(Ghost.transform.localEulerAngles.x, Ghost.transform.localEulerAngles.y, 45);
        yield return new WaitForSeconds(2.0f);
        Ghost.transform.localEulerAngles = new Vector3(Ghost.transform.localEulerAngles.x, Ghost.transform.localEulerAngles.y, 0);
        yield return new WaitForSeconds(3.0f);

        //bool go = false;

        Vector3 desPos = new Vector3(0.031f, -1.337f, 1.596f);
        StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.3f));



        yield return new WaitUntil(() => Ghost.transform.position == desPos); //wait until it sets position
        fadeColor = true;

        StartCoroutine(LerpLOGO());


        yield return new WaitUntil(() => Logo.localScale == new Vector3(0, 0, 0));

        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.07>안녕!\n만나서 반가워!", audioIndex = 0, 3.0f));
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.07>이곳은</size><size=0.09> <color=#EA2027>A</color><color=#EE5A24>T</color><color=#F79F1F>T</color><color=#009432>N</color><color=#0652DD>K</color><color=#1B1464>A</color><color=#B53471>R</color><color=#0984e3>E</color></size> <size=0.08>세계야", audioIndex = 1, 3.0f));
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.055>나는 오늘\n너의 가이드, 테스야\n잘 부탁해", audioIndex = 2, 5.0f));
        yield return new WaitForSeconds(5.0f);
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.053>시작하기 전에\n간단하게 설명해줄게!\n<color=#2e86de>(^ 0 ^)~", audioIndex = 3, 3.0f));

        yield return new WaitForSeconds(3.0f);
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.06>거기서 잠깐만\n기다려줘!", audioIndex = 4, 3.0f));
        

        desPos = new Vector3(0.031f, -0.664f, 2.823f);
        StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.4f));
        yield return new WaitUntil(() => Ghost.transform.position == desPos);
        // StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("들어간다~!\n<size=0.07><color=#2e86de>(^ - ^)");
        Ghost.GetComponent<Animator>().SetBool("isJump", true);
        yield return new WaitForSeconds(3.0f);
        Ghost.GetComponent<Animator>().SetBool("isJump", false);

        StartCoroutine(LerpDoor());
        desPos = new Vector3(0.031f, -0.519f, 6.885f);
        StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.5f));
        GetHeight player = GetComponent<GetHeight>();
        player.Height();

        yield return new WaitUntil(() => Ghost.transform.localPosition.z > 2.36f);
        TransitionOut.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.8f);

        //PlayerPrefs.SetString("State", "END");

        SceneLoader.LoadScene(10);


        yield return new WaitForSeconds(1.0f);


    }


}








/*

private void Awake()
{
    GameObject JasonManager = GameObject.Find("DataManager");


    if (PlayerPrefs.HasKey("Code"))
    {

        if (JasonManager)
        {
            dataManager = JasonManager.GetComponent<DataManager>();
            childCode = dataManager.userInfo.FolderName;

            Debug.Log(childCode);
            prevChildCode = PlayerPrefs.GetString("Code");


            if (prevChildCode == childCode)
            {
                state = "END"; //아동이 처음부터 재시작
            }
            else
            {
                state = "OPEN"; //새로운 아동이 들어옴
            }




        } else
        {

            state = "END"; //중간에 다시 시작해 이름이 들어있지 않음
            //state = "OPEN";
        }


    }
    else
    {
        state = "OPEN";
    }

}
// Start is called before the first frame update
void Start()
{

    Debug.Log(PlayerPrefs.HasKey("Code"));
    globalVolume = global.sharedProfile;
    globalVolume.TryGet<ColorAdjustments>(out _coloradjustment);



    if (state =="OPEN") 
    {

        Ghost.transform.position = new Vector3(3.1f, -1.337f, 3.09f);
        _coloradjustment.saturation.value = -100f;
        StartCoroutine(StartOpening());


    }
    if(state =="END")
    {
        Ghost.transform.position = new Vector3(0.031f, -0.56f, 6.695f);
        _coloradjustment.saturation.value = -10f;
        StartCoroutine(StartEnding());

    }


    }

// Update is called once per frame
void Update()
{
    float GhostRot = Ghost.transform.eulerAngles.y;
    SpeechBubble.localPosition = new Vector3(Ghost.transform.localPosition.x + 0.482f, Ghost.transform.localPosition.y + 0.865f, Ghost.transform.localPosition.z) ;
    Logo.localEulerAngles = new Vector3(Logo.localEulerAngles.x, GhostRot, Logo.localEulerAngles.z);

    globalVolume = global.sharedProfile;
    globalVolume.TryGet<ColorAdjustments>(out _coloradjustment);
    if (fadeColor)
    {
        if (state == "OPEN")
        {
            Timer += Time.deltaTime;
            if (Timer > 0.02f)
            {
                Timer = 0;
                _coloradjustment.saturation.value += 1f;

            }

        }
        if (state == "END")
        {
            Timer += Time.deltaTime;
            if (Timer > 0.02f)
            {
                Timer = 0;
                _coloradjustment.saturation.value -= 1f;


            }


        }
    }
}

private void LateUpdate()
{
    globalVolume = global.sharedProfile;
    globalVolume.TryGet<ColorAdjustments>(out _coloradjustment);

    if (_coloradjustment.saturation.value == -10f && state != "END")
    {
       state="MID"; ; //fading 끝
    }
    if (_coloradjustment.saturation.value == -100f && state != "OPEN")
    {
        state = "MID"; //fading 끝
    }



}





IEnumerator LerpDoor()
{
    float t = 0;
    while (true)
    {
        Door.transform.localEulerAngles = Vector3.Lerp(new Vector3(Door.transform.localEulerAngles.x, Door.transform.localEulerAngles.y, Door.transform.localEulerAngles.z), new Vector3(Door.transform.localEulerAngles.x, 106.108f, Door.transform.localEulerAngles.z), t);

        t += Time.deltaTime;

        if(Door.transform.localEulerAngles == new Vector3(Door.transform.localEulerAngles.x, 106.108f, Door.transform.localEulerAngles.z))
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

        Logo.transform.localScale = Vector3.Lerp(new Vector3(0,0,0), new Vector3(5050f,5050f,5050f), t);

        if (Logo.transform.localScale == new Vector3(5050f, 5050f, 5050f))
        {
            yield return new WaitForSeconds(3f);
            Logo.transform.localScale = Vector3.Lerp(new Vector3(5050,5050,5050), new Vector3(0,0,0), t);

            yield break;
        }

        yield return null;
    }

}




    IEnumerator StartOpening()
    {
    TransitionIN.gameObject.SetActive(true);
    yield return new WaitForSeconds(2f);


    TransitionIN.gameObject.SetActive(false);
    Ghost.transform.parent.gameObject.SetActive(true);
    Ghost.transform.localEulerAngles = new Vector3(Ghost.transform.localEulerAngles.x, Ghost.transform.localEulerAngles.y, 45 );
    yield return new WaitForSeconds(2.0f);
    Ghost.transform.localEulerAngles = new Vector3(Ghost.transform.localEulerAngles.x, Ghost.transform.localEulerAngles.y, 0 );
    yield return new WaitForSeconds(3.0f);

    //bool go = false;

    Vector3 desPos = new Vector3(0.031f, -1.337f, 1.596f);
    StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.3f));



    yield return new WaitUntil(() => Ghost.transform.position ==desPos); //wait until it sets position
    fadeColor = true;

    StartCoroutine(LerpLOGO());


    yield return new WaitUntil(() => Logo.localScale == new Vector3(0, 0, 0));

    StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.08>안녕 \n<color=#2e86de>(^ v ^)~", audioIndex = 0));
    yield return new WaitForSeconds(3.0f);
    StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.07>이곳은</size><size=0.09> <color=#EA2027>A</color><color=#EE5A24>T</color><color=#F79F1F>T</color><color=#009432>N</color><color=#0652DD>K</color><color=#1B1464>A</color><color=#B53471>R</color><color=#0984e3>E</color></size> <size=0.08>세계야", audioIndex = 1));
    yield return new WaitForSeconds(3.0f);
    StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.06>나는 오늘 \n너의 가이드야. \n 잘 부탁해 \n<color=#2e86de>(^ 0 ^)~", audioIndex = 2));
    yield return new WaitForSeconds(3.0f);
    StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.053>시작하기 전에\n간단하게 설명해줄게! \n<color=#2e86de>(^ 0 ^)~", audioIndex = 3));

    yield return new WaitForSeconds(3.0f);
    StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.06>잠깐만 기다려줘!!\n<color=#2e86de><size=0.074>(o v o)/", audioIndex = 4));


    desPos = new Vector3(0.031f, -0.664f, 2.823f);
    StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos,0.4f));
    yield return new WaitUntil(() => Ghost.transform.position == desPos);
   // StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("들어간다~!\n<size=0.07><color=#2e86de>(^ - ^)");
    Ghost.GetComponent<Animator>().SetBool("isJump", true);
    yield return new WaitForSeconds(3.0f);
    Ghost.GetComponent<Animator>().SetBool("isJump", false);

    StartCoroutine(LerpDoor());
    desPos = new Vector3(0.031f, -0.519f, 6.885f);
    StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.5f));


    yield return new WaitUntil(()=>Ghost.transform.localPosition.z >2.36f);
    TransitionOut.gameObject.SetActive(true);

    yield return new WaitForSeconds(1.8f);

    //PlayerPrefs.SetString("State", "END");
    PlayerPrefs.SetString("Code", childCode);
    SceneLoader.LoadScene("Tutorial");


    yield return new WaitForSeconds(1.0f);


    }

IEnumerator StartEnding()
{
    NetworkManager.DoSendToFinishData();
    TransitionIN.gameObject.SetActive(true);
    yield return new WaitForSeconds(1.5f);
    TransitionIN.gameObject.SetActive(false);
    //밖으로 달려나감
    //bool go = true;

    StartCoroutine(LerpDoor());
    desPos = new Vector3(0.031f, -0.56f,3.734f);
    StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.5f));
    yield return new WaitForSeconds(3.0f);


    StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.06>아쉽지만\n이제 헤어질 \n시간이야\n<color=#2e86de><size=0.09><b>(T ^ T)", audioIndex = 5));
    yield return new WaitForSeconds(2.5f);

    desPos = new Vector3(-0.02f, -0.693f, 2.795f);
    StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.06>오늘 함께해서 \n너무 즐거웠어\n<color=#2e86de><size=0.09><b>(^ - ^)", audioIndex = 6));
    yield return new WaitForSeconds(3.0f);
    StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.06>너도 즐거웠다면 \n좋겠다!\n<color=#2e86de><size=0.09><b>(//^ ^//)", audioIndex = 7));
    yield return new WaitForSeconds(2.5f);
    StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.2f));
    yield return new WaitForSeconds(1.8f);
    StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.055>난 항상 \n이 곳에 있을게!\n다음에 \n또 놀러와\n<color=#2e86de><b>(^ 0 ^)/", audioIndex = 8));
    yield return new WaitForSeconds(2.0f);


    fadeColor = true;
    SpeechBubble.gameObject.SetActive(false);
    Ghost.GetComponent<Animator>().SetBool("isJump", true);

    yield return new WaitForSeconds(3.0f);

    TransitionOut.gameObject.SetActive(true);
    Ghost.transform.parent.gameObject.SetActive(false);
    PlayerPrefs.SetString("State", "OPEN");
    yield return new WaitForSeconds(1.2f);


    Application.Quit();





}








}
*/
