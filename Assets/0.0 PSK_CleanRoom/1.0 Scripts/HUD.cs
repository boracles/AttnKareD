using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;  //for video play
using TMPro;  //TextMeshPro
using BNG;  //for Grabber Check
using DG.Tweening;  // for intro Text Animation
using System.Text;  //Encoding etc
namespace CleanRoom {
public class HUD : MonoBehaviour
{
    /**************************************************************************
    // Common UI Constant
    ***************************************************************************/    
    //Theme Colors
    public static Color COLOR_OUTLINE_YELLOE= Color.yellow;
    public static Color COLOR_OUTLINE_WHITE = Color.white;
  
    /*************************************************************************
    //타이머 시간정보 표시
    **************************************************************************/   
    //남은시간을 Display합니다
    public Text m_textTimeTaken;
    public void DrawTimeTaken(float time){
        int min = (int)(time/60f);
        int sec = (int)(time%60f);
        m_textTimeTaken.text = min.ToString("00") + ":" +sec.ToString("00");
    }
    
    /*************************************************************************
    //처음 안내문구 음성과 문구을 전시합니다
    *************************************************************************/
    public AudioSource          m_audSIntro;    //여기에 안내오디오소스를 할당, in Hud
    public AudioClip[]          m_audCIntro;    //여기에 안내음성 클립을 할당    
    public DOTweenAnimation[]   m_DotAnim;      //텍스트 애니메이션 할당(DOTWeen)
    enum   INTRO {  ONE, TWO, THREE, FOUR, FIVE, HOWTO, START_FIND, START_ARRANGE, END_ARRAGE }    
    public Canvas m_canvasINFO;

    //전면안내-내방 정리하는 방법 5개 음성 및 Text Animation 전시
    public void PlayHowTo() {                      
        StartCoroutine("PlayHowToVoiceText");        
    }
    IEnumerator PlayHowToVoiceText() {
        CanvasFadeIn(m_canvasINFO,1f);  // INFO Canvas fade In
        yield return new WaitForSeconds(1f); //숨고르기 Canvas가 FadeIn될동안
        float wait = PlaySound(m_audSIntro, m_audCIntro[(int)INTRO.HOWTO]);        
        yield return new WaitForSeconds(wait);
        for(int i = 0; i < 5; i++) { 
            yield return new WaitForSeconds(1f); //숨고르기
            wait = PlaySound(m_audSIntro, m_audCIntro[i]);                    
            m_DotAnim[i].DOPlay();      // Start Text Animation 
            yield return new WaitForSeconds(wait);
            //m_DotAnim[1].DOKill();      // Stop Text Animation
            //yield return new WaitUntil(() => !m_sndIntro[i].isPlaying);
        }
        CanvasFadeOut(m_canvasINFO,1f);  // Info Canvas fade In
        yield return new WaitForSeconds(2f); //숨고르기        
        PlayStartFind();  //자 이제 시작해볼까 전시
    }

    /**************************************************************************
    // HMD에 내용을 전시합니다.
    ***************************************************************************/
    public Canvas m_canvasHMD;
    /*****************************************
    //"자이제 시작해볼까 제한시간은 2분" 전시합니다.
    ******************************************/
    public TextMeshProUGUI m_textStartFind;
    void PlayStartFind() {               
        StartCoroutine(
            PlaySoundText(m_audSIntro,
            m_audCIntro[(int)INTRO.START_FIND],
            m_textStartFind.gameObject,
            m_canvasHMD,
            Guide.HUD_REPORT.PLAYED_INTRO_HOWTO
        ));             
    }    
  
    /************************************************
    // HMD안내- "자이제 찾은물건을 정리볼까 " 전시    
    *************************************************/
    public TextMeshProUGUI m_textStartArrange;
    public void PlayStartClean() {       
        StartCoroutine(
            PlaySoundText(m_audSIntro,
            m_audCIntro[(int)INTRO.START_ARRANGE],
            m_textStartArrange.gameObject,
            m_canvasHMD,
            Guide.HUD_REPORT.PLAYED_INTRO_CLEAN)
        );         
    }    
    /************************************************
    // HMD안내- "우와 방을 아주 깨끗.... " 전시    
    *************************************************/    
    public void PlayWellDone() {       
        StartCoroutine(
            PlaySoundText(m_audSIntro,
            m_audCIntro[(int)INTRO.END_ARRAGE],
            null,  //Text없음
            null,  //Canvas없음
            Guide.HUD_REPORT.PLAYED_INTRO_WELLDONE)
        );         
    }      
    /************************************************
    // HMD안내- "이동합니다 " 전시    
    *************************************************/
    public TextMeshProUGUI m_textMoving; //HMD_Canvas>Finish_TMP
    public void PlayMoving() {       
        StartCoroutine(
            PlaySoundText(null, //음성없음
            null, //Clip없음
            m_textMoving.gameObject,
            m_canvasHMD,
            Guide.HUD_REPORT.PLAYED_INTRO_MOVING)
        ); 
    }

    /************************************************
    // FinHMD안내- "Fin 버튼 누룰시" 전시    
    *************************************************/
    //CenterEyeAnchor>FinCanvas>FIN_1("정말로.."), Fin2("이동합니다")
    public Canvas m_canvasFin;
    public GameObject m_goReallyWant, m_goMoving; 
    public void PlayWarning() {       
        StartCoroutine(
            PlaySoundText(null, //음성없음
            null, //음성Clip없음
            m_goReallyWant, 
            m_canvasFin,
            Guide.HUD_REPORT.NONE) //보고할 메세지 없음
        );                 
    }    
    public void ShowMoving() {   
        CanvasFadeIn(m_canvasFin,1f); 
        m_goMoving.SetActive(true);
    }

    /************************************************
    //왼손 버튼을 누루면 힌트를 얻을 수 있어요 전시
    *************************************************/
    public GameObject m_goLeftHint;
    public void DisplayLeftHint(){
        StartCoroutine("DisplayLeftHintText");
    }
    IEnumerator DisplayLeftHintText()  {
        yield return new WaitForSeconds(1f); //숨고르기
        m_goLeftHint.SetActive(true);
        yield return new WaitForSeconds(3f); //숨고르기
        m_goLeftHint.SetActive(false);
    }

    /************************************************
    //물건 정리하기전 Target 5초동안 점멸한후 Object를 표시합니다
    *************************************************/    
    //Objects>TargetObj
    public GameObject m_goTarget;
    public void ShowTarget(bool bOn) {
        if(bOn) StartCoroutine(TargetBlink());
        else m_goTarget.SetActive(false);
    }
    IEnumerator TargetBlink() {
        for(int i=0; i< 5; i++) {
            m_goTarget.SetActive(!m_goTarget.activeSelf);
            yield return new WaitForSeconds(1f);
        }
        m_goTarget.SetActive(true);
    }

    /**************************************************************************
    // 물건을 찾았을시/정리했을시 "찾았다 2/9" 텍스트, 별파티클, "뾰롱" 사운드 전시
    **************************************************************************/
    //"찾았다 2/9"는 애니매이션으로 오른쪽에서 나타났다가 보여주고 왼쪽으로 사라진다 이때 싸운드가 Play된다
    // Scene > XR Rig Advanced>PlayerController>CameraRig....>CenterEyeAnchor>FoundCount_Popup     
    public Animator    m_aniPopup;
    public TextMeshPro m_textPopupValue;
    public AudioClip   m_clipBell;  //벨소리 Asset>Sound>Button>DM-CGS-45    
    public GameObject  m_goTextPopupFound, m_goTextPopupCleaned;
    //popup 찾은(정리한)갯수-둘다 사용        
    public void PopUpCount(int nDone, bool bClean=false) {    
        m_goTextPopupFound.SetActive(!bClean);
        m_goTextPopupCleaned.SetActive(bClean);
        m_textPopupValue.text = nDone.ToString() + " / " + Guide.TOTAL_OBJECT.ToString();
        m_aniPopup.Play("Found_Popup");
        PlaySound(m_audSIntro,m_clipBell);
    }
    
    // 찾아진 Object 중심에서 Start 파티클이 전시된다
    public GameObject m_goStarParicle; //Prefabs.BagPacking>VfxStarsThin 1 을 할당하세요
    public void ShowStarParticle(Transform transform) {
        GameObject go = Instantiate(m_goStarParicle,transform.position, Quaternion.identity);
        go.transform.GetComponent<ParticleSystem>().Play();
        Destroy(go, 3.0f);
    }

    /*************************************************
    // 물건을 찾기시작부터 비디오플레이
    **************************************************/
    public VideoPlayer  m_Video;
    public MeshRenderer m_VideoRenderer;
    public void PlayVideo(bool bOn)  {
        if(bOn)  m_Video.Play(); else m_Video.Stop();
        m_VideoRenderer.enabled = bOn;
    }

    /*************************************************
    // Duck 관리 : 물건을 찾기시작부터 오리움직이기
    **************************************************/
    public enum DUCK_ACTION { MOVING, GRABBED }
    public GameObject  m_goDuck;    
    public AudioClip   m_audiDuckQuack, m_audiDuckAngry;
    public void PlayDuck(bool bOn, DUCK_ACTION action = DUCK_ACTION.MOVING)  {        
        m_goDuck.SetActive(bOn);
        if(!bOn) return;
        AudioSource audioDuck =  m_goDuck.GetComponent<AudioSource>();
        Animator animator     =  m_goDuck.GetComponent<Animator>();
        audioDuck.Stop();
        switch(action){
        case DUCK_ACTION.MOVING:            
            audioDuck.clip = m_audiDuckQuack;            
            animator.SetInteger("Status", 2);
            break;
        case DUCK_ACTION.GRABBED:
            audioDuck.clip = m_audiDuckAngry;    ;
            m_goDuck.GetComponent<MoveableDuck>().SendMessage("SpeedUp");            
            animator.SetInteger("Status", 1);
            break;
        }
        audioDuck.Play();
    }
    
    /**************************************************************************
    // 찾은물건, 정리한 물건 물자열 관리
    ***************************************************************************/
    //찾은물건 정보 갱신
    public  TextMeshProUGUI m_textToFind, m_textFound;    
    public void NoteUpdateFind(string name=null)  { //null은 초기화
        m_textFound.text = name==null ? "" : m_textFound.text + name +",";
        m_textToFind.text = "";
        for(int i=0; i< Manager.CDB.Length; i++) { 
            m_textToFind.text += !Manager.CDB[i].bFound ? Manager.saCleanK[(int)Manager.CDB[i].eClean]+"," : ""; 
        }
    }
    //정리한물건 정보 갱신
    public  TextMeshProUGUI m_textToClean, m_textCleaned;    
    public void NoteUpdateClean(string name=null)  { //null은 초기화
        m_textCleaned.text = name==null ? "" : m_textCleaned.text+name +",";
        m_textToClean.text = "";
        for(int i=0; i< Manager.CDB.Length; i++) { 
            m_textToClean.text += !Manager.CDB[i].bClean ? Manager.saCleanK[(int)Manager.CDB[i].eClean]+"," : ""; 
        }
    }
    //찾을,찾은,정리할,정리한 물자열 초기화-이전에 ResetCDB가 호출이 있어야 함
    public void NoteReset() {
        NoteUpdateFind();  
        NoteUpdateClean(); 
    }
    /**************************************************************************
    // Timeout시 메세지("아쉽지만 시간이...") 전시합니다. 바라보는 방향에 안내판이 전시된다    
    ***************************************************************************/    
    public GameObject m_goTimeOutF,m_goTimeOutL,m_goTimeOutR,m_goTimeOutB,m_goTimeOutFText, m_goHowTo;
    public enum LOOK_DIR { FORWARD, LEFT, RIGHT, BACK, INVALID}
    LOOK_DIR m_eLookDir;
    public void PlayTimeOut() {                      
        StartCoroutine("PlayTimeOutText"); 
        StartCoroutine("StopTimeOutText");
    }
    IEnumerator PlayTimeOutText() {        
        do{
            m_eLookDir = GetLookDir();
            EnableTimeOutText();
            CanvasFadeIn(m_canvasINFO,1f);  // How To Canvas fade In     
            yield return new WaitForSeconds(0.1f);//0.1초 간격으로 검사하도록
            yield return new WaitUntil(() => GetLookDir() != m_eLookDir);
            CanvasFadeOut(m_canvasINFO,1f);
            yield return new WaitForSeconds(1f);
        }while(m_eLookDir != LOOK_DIR.INVALID);
        // INVALID에 따른 기존상태 원복        
        CanvasFadeOut(m_canvasINFO,1f);  //  Canvas fade Out
        m_Guide.HudReport(Guide.HUD_REPORT.PLAYED_INTRO_TIMEOUT);
    }
    IEnumerator StopTimeOutText() {        
        yield return new WaitForSeconds(20f);
        EnableTimeOutText();        //기존상태로 원복
        m_eLookDir = LOOK_DIR.INVALID;
    }
    //현재바라보는 방향의 안내Text를 선택합니다.    
    void EnableTimeOutText(){
        m_goTimeOutF.SetActive      (m_eLookDir == LOOK_DIR.FORWARD || m_eLookDir == LOOK_DIR.INVALID );
        m_goTimeOutFText.SetActive  (m_eLookDir == LOOK_DIR.FORWARD);
        m_goHowTo.SetActive         (m_eLookDir == LOOK_DIR.INVALID); 
        m_goTimeOutL.SetActive      (m_eLookDir == LOOK_DIR.LEFT);
        m_goTimeOutR.SetActive      (m_eLookDir == LOOK_DIR.RIGHT);
        m_goTimeOutB.SetActive      (m_eLookDir == LOOK_DIR.BACK);        
    }
    //PlayerController의 eulerAngle.y를 참조하여 바라보는 방향을 알아냅니다
    public Transform m_PlayerTransform; //XR Rig Advanced>PlayerController를 할당하십시요
    LOOK_DIR GetLookDir() {
        float ang = m_PlayerTransform.eulerAngles.y%360;
        if(ang >= 315f || ang < 45f ) return LOOK_DIR.FORWARD;
        else if(ang >=45f && ang < 135f) return LOOK_DIR.RIGHT;
        else if(ang >=135f && ang < 225f) return LOOK_DIR.BACK;
        else if(ang >=225f && ang < 315f) return LOOK_DIR.LEFT;
        else return LOOK_DIR.FORWARD;
    }
    /**************************************************************************
    // Helper & Utility
    ***************************************************************************/

    //지정된 AudioSource에 AudioClip을, 지정된 Text Gameobject를 보여주었다가 지정한 Canvas를 FadeIn Out시켜줍니다.
    //예정PlayTime은 = audioClip.lenth + 2초 입니다.
    IEnumerator PlaySoundText(AudioSource audioSource, AudioClip audioClip, GameObject goText, Canvas canvas, Guide.HUD_REPORT eReport){
        if(goText) goText.SetActive(true);              // Text Gameobject를 보여주고
        if(canvas) CanvasFadeIn(canvas,1f);             // Canvas를 보여주고
        if(canvas) yield return new WaitForSeconds(1f); // Canvas가 나타날때까지 기다렸다가
        if(audioSource && audioClip) { 
            float wait = PlaySound(audioSource, audioClip);   //Sound를 Play            
            yield return new WaitForSeconds(wait);          // Sound만큼 기다리고       
        } else yield return new WaitForSeconds(5f);       // 음성없을시는 텍스트를 5초보여주고
        if(goText) goText.SetActive(false);             // Text Gameobject를 감추고
        if(canvas) CanvasFadeOut(canvas,1f);            // Canvas를 감추고
        if(canvas) yield return new WaitForSeconds(1f); // Canvas가 사라질때까지 기다렸다가
        if(eReport != Guide.HUD_REPORT.NONE)  m_Guide.HudReport(eReport); // Guide에게 Report할게 있으면 Report
    }

    //Sound를출력합니다 : 지정된 AudioSource에 지정된 AudioClip을 Play하고 시간을 리턴합니다
    float PlaySound(AudioSource audioSource, AudioClip clip) {
        if(!audioSource || !clip) return 0f;        
        audioSource.clip  = clip;
        audioSource.Play();
        return clip.length;        
    }

    //Canvas Fade In/Out을 지정한 시간동안 처리합니다
    //FadeIn시 Canvas를 Enable시켜줍니다
    void CanvasFadeIn(Canvas canvas, float time){
        if(!canvas) return;  // check valid canvas ?
        //canvas.gameObject.SetActive(true);  
        StartCoroutine(AnimAlpah(canvas, time,true));
    }

    //FadeOut시 Canvas를 Disable시켜줍니다.
    void CanvasFadeOut(Canvas canvas, float time){
        if(!canvas) return;  // check valid canvas ?                
        StartCoroutine(AnimAlpah(canvas, time, false));
    }

    //FadeOut후 Canvas GameObject도  disable시켜줍니다
    IEnumerator AnimAlpah(Canvas canvas,float time, bool bIn){
        CanvasGroup cg = canvas.GetComponent<CanvasGroup>();
        cg.alpha = bIn ? 0 : 1f;
        int loop = (int)(time/0.05f);
        float fadeStep = 1f / loop;
        for(int i=0; i< loop; i++) {
            yield return new WaitForSeconds(0.05f);
            cg.alpha += bIn ? fadeStep : (-1f)*fadeStep;
        }
        //if(!bIn) canvas.gameObject.SetActive(false);
    }

    //사용자가 왼쪽, 오른쪽 Grabber Trigger를 누름    
    //Graber가 Hold이므로 Grabber에 Event등록을 하는것이 최선,
    //기존Trigger를 인식하므로 InputBrige를 Polling. Not Best.
    //Exhausted Polling을 방지하기 위해 0.1f 간격으로
    //노트를 보여주는것을 Hold버튼으로 바꾸는것 고려 필요
    public InputBridge m_InputBridge;   
    public GameObject  m_goLeftFindNote, m_goLeftCleanNote; //LeftController에 있는 노트를 할당하세요
    public GameObject  m_goRightNotGrabble; //RightController에 아직 잡을수 없어소
    void CheckInputBridge() {                       
        m_goLeftFindNote.SetActive ( (m_InputBridge.LeftTrigger>0.5f) && Guide.m_eState == Guide.STATE.FIND);
        m_goLeftCleanNote.SetActive( (m_InputBridge.LeftTrigger>0.5f) && Guide.m_eState == Guide.STATE.CLEAN);       
        m_goRightNotGrabble.SetActive( (m_InputBridge.RightGrip>0.5f) && Guide.m_eState != Guide.STATE.CLEAN);       
    }
    
    /**************************************************************************
    // Monobehavier
    ***************************************************************************/
    public Guide  m_Guide ;//for Report

    //0.5sec주기로 업데이할 Task를 등록하세요
    void Do05SecTask()  {   
        
    }
    //1sec주기로 업데이할 Task를 등록하세요
    void Do1SecTask()   {       
       CheckInputBridge();
    }
    //5sec주기로 업데이할 Task를 등록하세요
    void Do5SecTask()   {
        
    }
    // Use this for initialization
    
    // UI--디폴트로 그려지지 않은것들을 그려줍니다
    void InitDraw() {        
    }

    void Start()  {        

        InitDraw();
        
        //PlayTimeOut();

        //PlayVideo(true);

        //PlayStartFind();
        //PlayStartArrange();
        //PopUpFound(6);
        //ShowStarParticle(TestParticle.transform);       
        //ShowTarget(true);

    }

    float next05SecUpdate = 0;
    float next1SecUpdate = 0;
    float next5SecUpdate = 0;
    
    // Update is called once per frame
    void Update()  {
        //CheckGrabber();
        if (Time.time > next05SecUpdate) { Do05SecTask(); next05SecUpdate = Time.time + 0.5f; } //시간갱신
        if (Time.time > next1SecUpdate) { Do1SecTask(); next1SecUpdate = Time.time + 1.0f; } //시간갱신
        if (Time.time > next5SecUpdate) { Do5SecTask(); next5SecUpdate = Time.time + 5.0f; } //시간갱신        
    }

    }
    
}

