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
    //Ÿ�̸� �ð����� ǥ��
    **************************************************************************/   
    //�����ð��� Display�մϴ�
    public Text m_textTimeTaken;
    public void DrawTimeTaken(float time){
        int min = (int)(time/60f);
        int sec = (int)(time%60f);
        m_textTimeTaken.text = min.ToString("00") + ":" +sec.ToString("00");
    }
    
    /*************************************************************************
    //ó�� �ȳ����� ������ ������ �����մϴ�
    *************************************************************************/
    public AudioSource          m_audSIntro;    //���⿡ �ȳ�������ҽ��� �Ҵ�, in Hud
    public AudioClip[]          m_audCIntro;    //���⿡ �ȳ����� Ŭ���� �Ҵ�    
    public DOTweenAnimation[]   m_DotAnim;      //�ؽ�Ʈ �ִϸ��̼� �Ҵ�(DOTWeen)
    enum   INTRO {  ONE, TWO, THREE, FOUR, FIVE, HOWTO, START_FIND, START_ARRANGE, END_ARRAGE }    
    public Canvas m_canvasINFO;

    //����ȳ�-���� �����ϴ� ��� 5�� ���� �� Text Animation ����
    public void PlayHowTo() {                      
        StartCoroutine("PlayHowToVoiceText");        
    }
    IEnumerator PlayHowToVoiceText() {
        CanvasFadeIn(m_canvasINFO,1f);  // INFO Canvas fade In
        yield return new WaitForSeconds(1f); //������ Canvas�� FadeIn�ɵ���
        float wait = PlaySound(m_audSIntro, m_audCIntro[(int)INTRO.HOWTO]);        
        yield return new WaitForSeconds(wait);
        for(int i = 0; i < 5; i++) { 
            yield return new WaitForSeconds(1f); //������
            wait = PlaySound(m_audSIntro, m_audCIntro[i]);                    
            m_DotAnim[i].DOPlay();      // Start Text Animation 
            yield return new WaitForSeconds(wait);
            //m_DotAnim[1].DOKill();      // Stop Text Animation
            //yield return new WaitUntil(() => !m_sndIntro[i].isPlaying);
        }
        CanvasFadeOut(m_canvasINFO,1f);  // Info Canvas fade In
        yield return new WaitForSeconds(2f); //������        
        PlayStartFind();  //�� ���� �����غ��� ����
    }

    /**************************************************************************
    // HMD�� ������ �����մϴ�.
    ***************************************************************************/
    public Canvas m_canvasHMD;
    /*****************************************
    //"������ �����غ��� ���ѽð��� 2��" �����մϴ�.
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
    // HMD�ȳ�- "������ ã�������� �������� " ����    
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
    // HMD�ȳ�- "��� ���� ���� ����.... " ����    
    *************************************************/    
    public void PlayWellDone() {       
        StartCoroutine(
            PlaySoundText(m_audSIntro,
            m_audCIntro[(int)INTRO.END_ARRAGE],
            null,  //Text����
            null,  //Canvas����
            Guide.HUD_REPORT.PLAYED_INTRO_WELLDONE)
        );         
    }      
    /************************************************
    // HMD�ȳ�- "�̵��մϴ� " ����    
    *************************************************/
    public TextMeshProUGUI m_textMoving; //HMD_Canvas>Finish_TMP
    public void PlayMoving() {       
        StartCoroutine(
            PlaySoundText(null, //��������
            null, //Clip����
            m_textMoving.gameObject,
            m_canvasHMD,
            Guide.HUD_REPORT.PLAYED_INTRO_MOVING)
        ); 
    }

    /************************************************
    // FinHMD�ȳ�- "Fin ��ư �����" ����    
    *************************************************/
    //CenterEyeAnchor>FinCanvas>FIN_1("������.."), Fin2("�̵��մϴ�")
    public Canvas m_canvasFin;
    public GameObject m_goReallyWant, m_goMoving; 
    public void PlayWarning() {       
        StartCoroutine(
            PlaySoundText(null, //��������
            null, //����Clip����
            m_goReallyWant, 
            m_canvasFin,
            Guide.HUD_REPORT.NONE) //������ �޼��� ����
        );                 
    }    
    public void ShowMoving() {   
        CanvasFadeIn(m_canvasFin,1f); 
        m_goMoving.SetActive(true);
    }

    /************************************************
    //�޼� ��ư�� ����� ��Ʈ�� ���� �� �־�� ����
    *************************************************/
    public GameObject m_goLeftHint;
    public void DisplayLeftHint(){
        StartCoroutine("DisplayLeftHintText");
    }
    IEnumerator DisplayLeftHintText()  {
        yield return new WaitForSeconds(1f); //������
        m_goLeftHint.SetActive(true);
        yield return new WaitForSeconds(3f); //������
        m_goLeftHint.SetActive(false);
    }

    /************************************************
    //���� �����ϱ��� Target 5�ʵ��� �������� Object�� ǥ���մϴ�
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
    // ������ ã������/���������� "ã�Ҵ� 2/9" �ؽ�Ʈ, ����ƼŬ, "�Ϸ�" ���� ����
    **************************************************************************/
    //"ã�Ҵ� 2/9"�� �ִϸ��̼����� �����ʿ��� ��Ÿ���ٰ� �����ְ� �������� ������� �̶� �ο�尡 Play�ȴ�
    // Scene > XR Rig Advanced>PlayerController>CameraRig....>CenterEyeAnchor>FoundCount_Popup     
    public Animator    m_aniPopup;
    public TextMeshPro m_textPopupValue;
    public AudioClip   m_clipBell;  //���Ҹ� Asset>Sound>Button>DM-CGS-45    
    public GameObject  m_goTextPopupFound, m_goTextPopupCleaned;
    //popup ã��(������)����-�Ѵ� ���        
    public void PopUpCount(int nDone, bool bClean=false) {    
        m_goTextPopupFound.SetActive(!bClean);
        m_goTextPopupCleaned.SetActive(bClean);
        m_textPopupValue.text = nDone.ToString() + " / " + Guide.TOTAL_OBJECT.ToString();
        m_aniPopup.Play("Found_Popup");
        PlaySound(m_audSIntro,m_clipBell);
    }
    
    // ã���� Object �߽ɿ��� Start ��ƼŬ�� ���õȴ�
    public GameObject m_goStarParicle; //Prefabs.BagPacking>VfxStarsThin 1 �� �Ҵ��ϼ���
    public void ShowStarParticle(Transform transform) {
        GameObject go = Instantiate(m_goStarParicle,transform.position, Quaternion.identity);
        go.transform.GetComponent<ParticleSystem>().Play();
        Destroy(go, 3.0f);
    }

    /*************************************************
    // ������ ã����ۺ��� �����÷���
    **************************************************/
    public VideoPlayer  m_Video;
    public MeshRenderer m_VideoRenderer;
    public void PlayVideo(bool bOn)  {
        if(bOn)  m_Video.Play(); else m_Video.Stop();
        m_VideoRenderer.enabled = bOn;
    }

    /*************************************************
    // Duck ���� : ������ ã����ۺ��� ���������̱�
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
    // ã������, ������ ���� ���ڿ� ����
    ***************************************************************************/
    //ã������ ���� ����
    public  TextMeshProUGUI m_textToFind, m_textFound;    
    public void NoteUpdateFind(string name=null)  { //null�� �ʱ�ȭ
        m_textFound.text = name==null ? "" : m_textFound.text + name +",";
        m_textToFind.text = "";
        for(int i=0; i< Manager.CDB.Length; i++) { 
            m_textToFind.text += !Manager.CDB[i].bFound ? Manager.saCleanK[(int)Manager.CDB[i].eClean]+"," : ""; 
        }
    }
    //�����ѹ��� ���� ����
    public  TextMeshProUGUI m_textToClean, m_textCleaned;    
    public void NoteUpdateClean(string name=null)  { //null�� �ʱ�ȭ
        m_textCleaned.text = name==null ? "" : m_textCleaned.text+name +",";
        m_textToClean.text = "";
        for(int i=0; i< Manager.CDB.Length; i++) { 
            m_textToClean.text += !Manager.CDB[i].bClean ? Manager.saCleanK[(int)Manager.CDB[i].eClean]+"," : ""; 
        }
    }
    //ã��,ã��,������,������ ���ڿ� �ʱ�ȭ-������ ResetCDB�� ȣ���� �־�� ��
    public void NoteReset() {
        NoteUpdateFind();  
        NoteUpdateClean(); 
    }
    /**************************************************************************
    // Timeout�� �޼���("�ƽ����� �ð���...") �����մϴ�. �ٶ󺸴� ���⿡ �ȳ����� ���õȴ�    
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
            yield return new WaitForSeconds(0.1f);//0.1�� �������� �˻��ϵ���
            yield return new WaitUntil(() => GetLookDir() != m_eLookDir);
            CanvasFadeOut(m_canvasINFO,1f);
            yield return new WaitForSeconds(1f);
        }while(m_eLookDir != LOOK_DIR.INVALID);
        // INVALID�� ���� �������� ����        
        CanvasFadeOut(m_canvasINFO,1f);  //  Canvas fade Out
        m_Guide.HudReport(Guide.HUD_REPORT.PLAYED_INTRO_TIMEOUT);
    }
    IEnumerator StopTimeOutText() {        
        yield return new WaitForSeconds(20f);
        EnableTimeOutText();        //�������·� ����
        m_eLookDir = LOOK_DIR.INVALID;
    }
    //����ٶ󺸴� ������ �ȳ�Text�� �����մϴ�.    
    void EnableTimeOutText(){
        m_goTimeOutF.SetActive      (m_eLookDir == LOOK_DIR.FORWARD || m_eLookDir == LOOK_DIR.INVALID );
        m_goTimeOutFText.SetActive  (m_eLookDir == LOOK_DIR.FORWARD);
        m_goHowTo.SetActive         (m_eLookDir == LOOK_DIR.INVALID); 
        m_goTimeOutL.SetActive      (m_eLookDir == LOOK_DIR.LEFT);
        m_goTimeOutR.SetActive      (m_eLookDir == LOOK_DIR.RIGHT);
        m_goTimeOutB.SetActive      (m_eLookDir == LOOK_DIR.BACK);        
    }
    //PlayerController�� eulerAngle.y�� �����Ͽ� �ٶ󺸴� ������ �˾Ƴ��ϴ�
    public Transform m_PlayerTransform; //XR Rig Advanced>PlayerController�� �Ҵ��Ͻʽÿ�
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

    //������ AudioSource�� AudioClip��, ������ Text Gameobject�� �����־��ٰ� ������ Canvas�� FadeIn Out�����ݴϴ�.
    //����PlayTime�� = audioClip.lenth + 2�� �Դϴ�.
    IEnumerator PlaySoundText(AudioSource audioSource, AudioClip audioClip, GameObject goText, Canvas canvas, Guide.HUD_REPORT eReport){
        if(goText) goText.SetActive(true);              // Text Gameobject�� �����ְ�
        if(canvas) CanvasFadeIn(canvas,1f);             // Canvas�� �����ְ�
        if(canvas) yield return new WaitForSeconds(1f); // Canvas�� ��Ÿ�������� ��ٷȴٰ�
        if(audioSource && audioClip) { 
            float wait = PlaySound(audioSource, audioClip);   //Sound�� Play            
            yield return new WaitForSeconds(wait);          // Sound��ŭ ��ٸ���       
        } else yield return new WaitForSeconds(5f);       // ���������ô� �ؽ�Ʈ�� 5�ʺ����ְ�
        if(goText) goText.SetActive(false);             // Text Gameobject�� ���߰�
        if(canvas) CanvasFadeOut(canvas,1f);            // Canvas�� ���߰�
        if(canvas) yield return new WaitForSeconds(1f); // Canvas�� ����������� ��ٷȴٰ�
        if(eReport != Guide.HUD_REPORT.NONE)  m_Guide.HudReport(eReport); // Guide���� Report�Ұ� ������ Report
    }

    //Sound������մϴ� : ������ AudioSource�� ������ AudioClip�� Play�ϰ� �ð��� �����մϴ�
    float PlaySound(AudioSource audioSource, AudioClip clip) {
        if(!audioSource || !clip) return 0f;        
        audioSource.clip  = clip;
        audioSource.Play();
        return clip.length;        
    }

    //Canvas Fade In/Out�� ������ �ð����� ó���մϴ�
    //FadeIn�� Canvas�� Enable�����ݴϴ�
    void CanvasFadeIn(Canvas canvas, float time){
        if(!canvas) return;  // check valid canvas ?
        //canvas.gameObject.SetActive(true);  
        StartCoroutine(AnimAlpah(canvas, time,true));
    }

    //FadeOut�� Canvas�� Disable�����ݴϴ�.
    void CanvasFadeOut(Canvas canvas, float time){
        if(!canvas) return;  // check valid canvas ?                
        StartCoroutine(AnimAlpah(canvas, time, false));
    }

    //FadeOut�� Canvas GameObject��  disable�����ݴϴ�
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

    //����ڰ� ����, ������ Grabber Trigger�� ����    
    //Graber�� Hold�̹Ƿ� Grabber�� Event����� �ϴ°��� �ּ�,
    //����Trigger�� �ν��ϹǷ� InputBrige�� Polling. Not Best.
    //Exhausted Polling�� �����ϱ� ���� 0.1f ��������
    //��Ʈ�� �����ִ°��� Hold��ư���� �ٲٴ°� ��� �ʿ�
    public InputBridge m_InputBridge;   
    public GameObject  m_goLeftFindNote, m_goLeftCleanNote; //LeftController�� �ִ� ��Ʈ�� �Ҵ��ϼ���
    public GameObject  m_goRightNotGrabble; //RightController�� ���� ������ �����
    void CheckInputBridge() {                       
        m_goLeftFindNote.SetActive ( (m_InputBridge.LeftTrigger>0.5f) && Guide.m_eState == Guide.STATE.FIND);
        m_goLeftCleanNote.SetActive( (m_InputBridge.LeftTrigger>0.5f) && Guide.m_eState == Guide.STATE.CLEAN);       
        m_goRightNotGrabble.SetActive( (m_InputBridge.RightGrip>0.5f) && Guide.m_eState != Guide.STATE.CLEAN);       
    }
    
    /**************************************************************************
    // Monobehavier
    ***************************************************************************/
    public Guide  m_Guide ;//for Report

    //0.5sec�ֱ�� �������� Task�� ����ϼ���
    void Do05SecTask()  {   
        
    }
    //1sec�ֱ�� �������� Task�� ����ϼ���
    void Do1SecTask()   {       
       CheckInputBridge();
    }
    //5sec�ֱ�� �������� Task�� ����ϼ���
    void Do5SecTask()   {
        
    }
    // Use this for initialization
    
    // UI--����Ʈ�� �׷����� �����͵��� �׷��ݴϴ�
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
        if (Time.time > next05SecUpdate) { Do05SecTask(); next05SecUpdate = Time.time + 0.5f; } //�ð�����
        if (Time.time > next1SecUpdate) { Do1SecTask(); next1SecUpdate = Time.time + 1.0f; } //�ð�����
        if (Time.time > next5SecUpdate) { Do5SecTask(); next5SecUpdate = Time.time + 5.0f; } //�ð�����        
    }

    }
    
}

