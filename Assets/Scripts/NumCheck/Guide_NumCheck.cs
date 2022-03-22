using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BNG;
using TMPro;
using UnityEngine.UI;
using HutongGames.PlayMaker;
using KetosGames.SceneTransition;
using TooltipAttribute = UnityEngine.TooltipAttribute;

public class Guide_NumCheck : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("CREATE PREFAB")]
    [Tooltip("Prefab for Number")]
    public GameObject prefab_Button;
    public InputBridge XrRig;
    public UIPointer RighthandPointer;
    public int int_buttonN;
    public Transform ButtonParent;
    public List<Vector3> arrPos = new List<Vector3>();
    public List<MoveButton> arrBtn;
    public GameObject[] arrTrig;
    [Tooltip("Images for disctraction")]
    public Sprite[] DistracImage;

    [Header("DATA COLLECTION")]
    public AutoVoiceRecording DataCollection;
    public CheckData_NumCheck dataCheck;
    public PlayMakerFSM dataFin;
    public Transform GameDataMG;

    public bool active= false;
    public GameObject Ghost;
    public TextandSpeech narration;
    [HideInInspector]
    public bool turn = true;
    int sprite = 0;
    bool m_bColor = true;
    int m_nPos = 0;
    public static int Index = 0;
    AutoButton auto;
    List<GameObject> m_goAnswer;

    public enum INDEX { ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN }
    public struct NUMCHECK_LIST
    {
        public INDEX eIndex;
        public int nNum;//종류
        public GameObject goNum;
        public GameObject goTrig; //trigger
        public int nBtn;
        public bool bColor;
        public NUMCHECK_LIST(INDEX eIndex, int nNum, GameObject goNum, GameObject goTrig, int nBtn, bool bColor)
        {
            this.eIndex = eIndex;
            this.nNum = nNum;
            this.goNum = goNum;
            this.goTrig = goTrig;
            this.nBtn = nBtn;
            this.bColor = bColor; // true = red // false = yellow
        }
    }

    public static NUMCHECK_LIST[] NCDB = new NUMCHECK_LIST[]  {
      new NUMCHECK_LIST( INDEX.ONE ,1,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.TWO ,2,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.THREE ,3,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.FOUR ,4,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.FIVE,5,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.SIX ,6,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.SEVEN ,7,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.EIGHT ,8,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.NINE ,9,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.TEN ,10, null, null, 0, false)
    };
    public static NUMCHECK_LIST[] NCDB2 = new NUMCHECK_LIST[] {
      new NUMCHECK_LIST( INDEX.ONE ,1,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.TWO ,2,  null, null, 0, true),
      new NUMCHECK_LIST(INDEX.THREE ,3,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.FOUR ,4,  null, null, 0, true),
      new NUMCHECK_LIST(INDEX.FIVE,5,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.SIX ,6,  null, null, 0, true),
      new NUMCHECK_LIST(INDEX.SEVEN ,7,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.EIGHT ,8,  null, null, 0, true),
      new NUMCHECK_LIST(INDEX.NINE ,9,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.TEN ,10, null, null, 0, true)

    };

    void Awake()
    {
        SetPosition();
        CreateButton();

    }
    void Start()
    {
        SetTrigger();
        auto = GetComponentInChildren<AutoButton>();
        StartCoroutine(GameStart());
    }
   
    private void SetPosition() //creat list of position of number buttons on board(total 30)
    {
        for (float i = -0.8f; i <= 0.95f; i += 0.25f)
            for (float j = 0.3f; j >= -0.2f; j -= 0.25f)
            {
                float n = i;
                float m = j;
                n += Random.Range(-0.07f, 0.07f);   //위치를 랜던하게 하기 위해서
                m -= Random.Range(-0.08f, 0.08f);   //위치를 랜던하기 하기 위해서
                arrPos.Add(new Vector3(n, m, 0)); 
            }
        Shuffle.ShuffleList(arrPos);
    }
    private void CreateButton()
    {
        arrBtn = new List<MoveButton>();
        for (int i = 0; i < int_buttonN; i++)
        {
            bool color = i % 2 != 0;
            GameObject go = Instantiate(prefab_Button, new Vector3(0, 0, 0), Quaternion.identity, ButtonParent);
            MoveButton goTemp = go.GetComponent<MoveButton>();
            go.transform.localPosition = arrPos[i]; go.transform.SetSiblingIndex(i); //Set button's position and index in Hiearchy(if not above trigger, pointer cannot detect button)
            goTemp.btnNum = (i + 1).ToString(); go.GetComponent<MoveButton>().SetBtnNum(color); //Set button Number
            goTemp.XrRig = this.XrRig;
            goTemp.RighthandPointer = this.RighthandPointer;
            if (i < NCDB.Length) { NCDB[i].goNum = goTemp.gameObject; goTemp.m_eIndex = NCDB[i].eIndex; }
            arrBtn.Add(goTemp);
            if (i > int_buttonN - DistracImage.Length) SetSprite(goTemp);
        }
    }

    private void CreateStage2()
    {
        arrBtn = new List<MoveButton>();
        for (int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                bool isEven = i % 2 == 0;
                m_bColor = j == 0 ? false : true; //j 0이면 true(red) 1이면 false(yellow)
                GameObject go = Instantiate(prefab_Button, new Vector3(0, 0, 0), Quaternion.identity, ButtonParent);
                MoveButton goTemp = go.GetComponent<MoveButton>();
                go.transform.localPosition = arrPos[m_nPos]; go.transform.SetSiblingIndex(m_nPos); //Set button's position and index in Hiearchy(if not above trigger, pointer cannot detect button)
                goTemp.btnNum = (i + 1).ToString(); go.GetComponent<MoveButton>().SetBtnStage2(m_bColor); //Set button Number
                goTemp.XrRig = this.XrRig;
                goTemp.RighthandPointer = this.RighthandPointer;
                if (i < NCDB2.Length)
                { goTemp.m_eIndex = NCDB2[i].eIndex;
                    if(isEven != m_bColor) NCDB2[i].goNum = goTemp.gameObject;
                }
                arrBtn.Add(goTemp);
                goTemp.bStage = true; //stage 2가 시작되면 cangrab을 바로 키고, move button stage2임을 알려준다
                m_nPos++;
            }
            
        }
        for(int i = 0; i <NCDB2.Length; i++)
        {
            Debug.Log(NCDB2[i].goNum + " " + NCDB2[i].bColor);
        }

    }

    void SetTrigger()
    {
        for (int i = 0; i < arrTrig.Length; i++) NCDB[i].goTrig = arrTrig[i];
    }

    void TriggerStage2()
    {
        for (int i = 0; i < arrTrig.Length; i++) NCDB2[i].goTrig = arrTrig[i];
    }
    private void SetSprite(MoveButton btn) //특정 버튼에 Distraction Image를 추가
    {
        btn.distraction = true;
        btn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = null;
        Image btnImage = btn.transform.GetChild(0).GetComponent<Image>();
        btnImage.sprite = DistracImage[sprite];
        var tempColor = btnImage.color;
        tempColor.a = 1f;
        btnImage.color = tempColor;
        sprite++;
    }
    public void CannotGrab(MoveButton num) //한 버튼 만졌을 때 다른 버튼 MoveButton OFF
    {
        for(int i =0;i<arrBtn.Count;i++){
            if(arrBtn[i] != num) arrBtn[i].enabled = false;
        }
    }
    public void CanGrab() //버튼 놓은 후 다시 MoveButton On
    {
        for (int i = 0; i < arrBtn.Count; i++) arrBtn[i].enabled = true;
    }
    public void NumInTrigger(MoveButton num, GameObject trigger) //카드가 트리거 안에 들어갔을 때 데이터 체크
    {
        int cardNum = int.Parse(num.btnNum);
        NUMCHECK_LIST m_current = NCDB[Index];
        m_current.nBtn = cardNum;
        if(NCDB[(int)num.m_eIndex].nNum == NCDB[Index].nNum && NCDB[(int)num.m_eIndex].goTrig == trigger.gameObject)
        {
            num.SetButton();
            active = false;
            if (cardNum >= arrTrig.Length)
            {
                Stage2();
                return;
            }
            arrBtn.Remove(num);
            GameObject go = num.gameObject;
            Destroy(num);
            Index++;
            auto.AutoMove();
            return;
        }
        if(NCDB[(int)num.m_eIndex].nNum != m_current.nNum) //현재 버튼이 순서에 맞지 않음
        {
            if (!narration.coroutine) StartCoroutine(narration.BoardUI(0)); //wrong order warning and narration
            dataCheck.wrongorder_stage1++;
        }
        if(NCDB[(int)num.m_eIndex].goTrig != trigger.gameObject) //현재 버튼이 올바른 칸에 있지 않음
        {
            if (!narration.coroutine) StartCoroutine(narration.BoardUI(1)); //wrong trigger warning text and narration
            dataCheck.wrongTrigger_stage1++;
        }
        CanGrab();
        num.ResetButton();

    }

    public void NumStage2(MoveButton num, GameObject trigger)
    {
        int cardNum = int.Parse(num.btnNum);
        NUMCHECK_LIST m_current = NCDB[Index];
        m_current.nBtn = cardNum;
        if (NCDB2[(int)num.m_eIndex].nNum == NCDB2[Index].nNum && NCDB2[(int)num.m_eIndex].goTrig == trigger.gameObject && NCDB2[(int)num.m_eIndex].bColor == num.bColor)
        {
            num.SetButton();
            active = false;
            if (cardNum >= arrTrig.Length)
            {
                StageComplete();
                return;
            }
            arrBtn.Remove(num);
            Destroy(num);
            Index++;
            CanGrab();
            return;
        }
        if (NCDB2[(int)num.m_eIndex].nNum != m_current.nNum) //현재 버튼이 순서에 맞지 않음
        {
            if (!narration.coroutine) StartCoroutine(narration.BoardUI(0)); //wrong order warning and narration
            dataCheck.wrongorder_stage2++;
        }
        if (NCDB2[(int)num.m_eIndex].goTrig != trigger.gameObject) //현재 버튼이 올바른 칸에 있지 않음
        {
            if (!narration.coroutine) StartCoroutine(narration.BoardUI(1)); //wrong trigger warning text and narration
            dataCheck.wrongTrigger_stage2++;
        }
        if(NCDB2[(int)num.m_eIndex].bColor != num.bColor)
        { 
            if (!narration.coroutine) StartCoroutine(narration.BoardUI(4));
            dataCheck.wrongColor++;
        }
        CanGrab();
        num.ResetButton();
    }
    private IEnumerator GameStart() //Highlight Trigger as introduction
    {
        Index = 0;
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(narration.CharacterSpeak(0,4));
        for (int i =0; i <1; i++){
            foreach(GameObject trigger in arrTrig)
            {
                trigger.SetActive(false);
                yield return new WaitForSeconds(0.2f);
                trigger.SetActive(true);
            }
        }
        yield return new WaitForSeconds(0.8f);
        foreach (MoveButton button in arrBtn) { button.enabled = true; }
        dataCheck.start = true; //data check playtime
        auto.AutoMove();

    }
    private IEnumerator ClearCoroutine()
    {
        Ghost.GetComponent<Animator>().SetBool("isJump", true);
        yield return StartCoroutine(narration.BoardUI(5)); //Game clear narration
        dataFin.SendEvent("AllDone");
        yield return new WaitForSeconds(2.5f);
        narration.EndUI("다음으로 넘어가는 중....");
        yield return new WaitForSeconds(2.0f);
        KetosGames.SceneTransition.SceneLoader.LoadScene(13); //load play paddle scene
    }

    private void Stage2()
    { //reset before starting stage 2
        StopAllCoroutines();
        dataCheck.Stage1();
        foreach(MoveButton go in arrBtn)Destroy(go.gameObject);
        foreach (Transform child in ButtonParent.transform) Destroy(child.gameObject);
        Index = 0;
        StartCoroutine(Stage2Start());
    }

    IEnumerator Stage2Start()
    {
        CreateStage2();
        TriggerStage2();
        yield return StartCoroutine(narration.CharacterSpeak(4, 6));
        CanGrab();
    }

    void StageComplete()
    {
        dataCheck.Stage2();
        GameClear();
    }
    public void GameClear()
    {
        StopAllCoroutines();
        auto.enabled = false;
        dataCheck.GetAllData();
        DataCollection.StopRecordingNBehavior();
        GameDataMG.GetComponent<GameDataManager>().SaveCurrentData();
        StartCoroutine(ClearCoroutine());
    }
    //1단계에서 잘못된 순서, 트리거
    //2단계에서 잘못돈 순서, 트리거, 색깔

   
}
