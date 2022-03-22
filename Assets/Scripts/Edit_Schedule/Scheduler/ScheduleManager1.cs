using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KetosGames.SceneTransition;
using BNG;
using UnityEngine.Serialization;
using Button = UnityEngine.UI.Button;
using SceneLoader = KetosGames.SceneTransition.SceneLoader;

public enum ESoundType
{
    In,
    Click,
    Cnt,
    Put
}

namespace Scheduler
{
    [RequireComponent(typeof(Transform))]
    public class ScheduleManager1 : MonoBehaviour
    {
        
        public CollectData collectData;
        public AutoVoiceRecording voiceRecording;
        
        private const float TimeLimit = 120; //시간 제한 사용 방향 기획 필요
        private const float TimeLimitForFinish = 180; //강제종료시간

        [SerializeField] private Transform hud;
        [SerializeField] private Transform mainUi;
        [SerializeField] private Transform subUi;
        [SerializeField] private GameObject board;
        [SerializeField] private Transform finishPanel;
        [SerializeField] private Transform startBtn;
        [SerializeField] private Transform wellDoneAndBye;
        [SerializeField] private TextMeshProUGUI finishCntDwn;

        [SerializeField] private Transform bgmController;

        [SerializeField] private Text result;

        [SerializeField] private Transform btnFinish;

        [SerializeField] private TextMeshProUGUI textTitle;

        [SerializeField] private Text toShow;

        [SerializeField] private GameObject defCards;
        [SerializeField] private Transform slot;
        [SerializeField] private Transform grp;
        [SerializeField] private Transform originPos;

        [SerializeField] private Text timerText;

        [SerializeField] private List<Transform> slotList; 
        public List<Transform> grpList;
        private List<Transform> oPosList;

        public float[] Scene2Arr { get; set; }

        public bool isReset;
        public bool pointerLock;

        private bool leGogo;
        private bool beforeStart;
        private bool firstSelect;
        private bool beforeStartGuideCheck;

        private bool checkTimeLimit;
        private bool checkTimeOut;

        private float _planData01;
        private float _planData02;
        
        private float guide_Length = 25;

        [SerializeField] private int completionCtn;
        private int timerMin = 2;
        private int timerSec = 30;
        
        private float totalElapsedTime;         //수행한 시간 계산용
        private float totalElapsedTimeForCalc;  //수행한 시간 보여주기용
        private float timerForBeforeStarted;    //시작하기 누르기까지 시간
        private float timerForFirstSelect;      //시작하기 누르고 첫 선택까지 시간
        private float data_210;
        private float data_211;
        private float data_212;
        private float data_213;

        //float TotalScenePlayingTime = 0;    //컨텐츠 시작부터 끝까지 총 시간 TimerForBeforeStarted + TotalElapsedTimeForCalc

        [Header("Data Field")]
        [SerializeField] private int totalMovingCnt;      //이동 횟수
        [SerializeField] private int resetCnt;            //초기화 누른 횟수
        [SerializeField] private int selectNoCtn;                 //아니오 누른 횟수
        [SerializeField] private int skipYn;

        //------------- SOUND    
        [FormerlySerializedAs("sound_Count")] public AudioClip soundCount;
        [FormerlySerializedAs("sound_In")] public AudioClip soundIn;
        [FormerlySerializedAs("sound_Click")] public AudioClip soundClick;
        [FormerlySerializedAs("sound_Put")] public AudioClip soundPut;
        private AudioSource audioSource;

        //------------- Manager
        [FormerlySerializedAs("SetPlayerData")] public Transform setPlayerData;
        [FormerlySerializedAs("GameDataManager")] public Transform gameDataManager;

        private void Awake()
        {
            completionCtn = 0;
            skipYn = 0;

            pointerLock = false;
            beforeStart = true;
            isReset = false;

            audioSource = this.GetComponent<AudioSource>();
            slotList = new List<Transform>();
            grpList = new List<Transform>();
            oPosList = new List<Transform>();
        }

        private void Start()
        {
            InitSlotList();
            InitGrpList();
            InitOposList();

            collectData.AddTimeStamp("GUIDE START");
        }

        private void Update()
        {
            if (leGogo)
            {
                totalElapsedTime += Time.deltaTime;

                if (totalElapsedTime > 1)
                {
                    totalElapsedTime = 0;
                    totalElapsedTimeForCalc += 1;

                    if (timerMin != 0 || timerSec != 0)
                    {
                        timerSec -= 1;

                        if (timerSec < 0 && timerMin > 0)
                        {
                            timerSec = 59;
                            timerMin = 0;
                        }

                        string textSec = "";

                        if (timerSec < 10)
                        {
                            textSec = "0" + timerSec.ToString();
                        }
                        else
                        {
                            textSec = timerSec.ToString();
                        }


                        timerText.text = "0" + timerMin.ToString() + ":" + textSec;
                    }

                    if (!checkTimeLimit && totalElapsedTimeForCalc >= TimeLimit)
                    {
                        //시간제한
                        checkTimeLimit = true;
                        StartCoroutine(TimeLimitAndKeepGoing());
                    }

                    if (!checkTimeOut && totalElapsedTimeForCalc >= TimeLimitForFinish)
                    {
                        //강제종료
                        checkTimeOut = true;
                        StartCoroutine(TimeOutAndFinish());
                    }
                }
            }

            if (beforeStart)
            {
                timerForBeforeStarted += Time.deltaTime;

                if (!beforeStartGuideCheck && timerForBeforeStarted > guide_Length)
                {
                    collectData.AddTimeStamp("GUIDE END");
                    beforeStartGuideCheck = true;
                }
            }

            if (firstSelect)
            {
                timerForFirstSelect += Time.deltaTime;
            }
        }

        IEnumerator TimeLimitAndKeepGoing()
        {
            collectData.AddTimeStamp("TIME LIMIT");
            bgmController.GetComponent<BGMcontroller>().PlayBGMByTypes("LIMIT");

            timerSec = 30;

            yield return new WaitForSeconds(6);

            bgmController.GetComponent<BGMcontroller>().PlayBGMByTypes("BGM");
        }

        private IEnumerator TimeOutAndFinish()
        {
            collectData.AddTimeStamp("TIME OUT");
            bgmController.GetComponent<BGMcontroller>().PlayBGMByTypes("OUT");

            yield return new WaitForSeconds(6);

            FinishPanel_Yes(true);
        }

        private IEnumerator ResetDelay(float wait)
        {
            isReset = true;
            yield return new WaitForSeconds(wait);
            isReset = false;           
        }

        private List<Transform> InitSlotList()
        {
            
            foreach (Transform s in slot)
            {
                slotList.Add(s);
            }
            return slotList;
        }

        public List<Transform> InitOposList()
        {
            foreach (var oP in oPosList)
            {
                oPosList.Add(oP);
            }
            return oPosList;
        }
        
        public List<Transform> InitGrpList()
        {
            if (grpList == null) return grpList;
            RemoveGrpList();
            AddGrpList();
            Debug.Log("grpList 초기화");
            return grpList;
        }

        private void AddGrpList()
        {
            foreach (Transform g in grp)
            {
                grpList.Add(g);
            }
            Debug.Log("grpList 추가");
        }

        private void RemoveGrpList()
        {
            foreach (Transform g in grp)
            {
                grpList.Remove(g);
            }
            Debug.Log("grpList 삭제");
        }

        private List<Transform> ResetGrpList()
        {            
            string keyword = "(Clone)";
            
            foreach (Transform g in grp)
            {
                if(g != null)
                {
                    if (RemoveWord.EndsWithWord(g.name, keyword))
                    {
                        grpList.Remove(g);
                    }
                }                                              
            }            
            return grpList;
        }

        public void LockAllCollision(Transform obj)
        {
            foreach (var go in grpList)
            {
                if (go != obj)
                {
                    go.GetComponent<PlanCubeController1>().enabled = false;
                }
            }
        }

        public void ReleaseAllCollision()
        {
            foreach (Transform go in grpList)
            {
                go.GetComponent<PlanCubeController1>().enabled = true;
            }
        }

        public void CheckMovingCnt()
        {
            totalMovingCnt += 1;
            firstSelect = false;
        }

        bool CheckEmptySlot()
        {
            bool check = false;

            foreach (var s in slotList)
            {
                if (s.GetComponent<PlanSlotController1>().passenger != null)
                {
                    check = true;
                }
            }

            return check;
        }

        public void ReSetAll()
        {
            PlaySoundByTypes(ESoundType.Click);
            foreach (var oP in oPosList)
            {
                oP.GetComponent<OriginPosController>().ResetOriginPos();
            }
            
            foreach(var t in grpList)
            {
                StartCoroutine(t.GetComponent<PlanCubeController1>().ResetPlanCube(0.07f));
            }

            if (CheckEmptySlot())
            {
                StartCoroutine(ResetDelay(0.05f));

                foreach (var t in slotList)
                {
                    t.GetComponent<PlanSlotController1>().ResetPlanSlot();
                }

                foreach (var t in slotList)
                {
                    StartCoroutine(t.GetComponent<PlanSlotController1>().ResetSlotMesh(0.1f));
                }
                ResetGrpList();

                btnFinish.gameObject.SetActive(false);
                resetCnt += 1;
            }
        }

        public void CheckAllScheduleOnSlot()
        {
            var allDone = true;

            foreach (var aSlot in slotList.Where(aSlot => aSlot.GetComponent<PlanSlotController1>().passenger == null))
            {
                allDone = false;
            }

            if (allDone)
            {
                btnFinish.gameObject.SetActive(true);
            }
        }


        public void DoStartSchedule()
        {
            PlaySoundByTypes(ESoundType.Click);
            StartCoroutine(StartCntDown());
            if (completionCtn >= 2)
            {
                this.gameObject.GetComponent<Button>().interactable = false;
            }
        }

        IEnumerator StartCntDown()
        {
            bgmController.GetComponent<BGMcontroller>().PlayBGMByTypes("BGM");

            textTitle.text = "<color=#FFFFFF>준비 ~";

            yield return new WaitForSeconds(1f);
            PlaySoundByTypes(ESoundType.Cnt);
            textTitle.text = "<color=#FFFFFF>3";
            yield return new WaitForSeconds(1);
            PlaySoundByTypes(ESoundType.Cnt);
            textTitle.text = "<color=#FFFFFF>2";
            yield return new WaitForSeconds(1);
            PlaySoundByTypes(ESoundType.Cnt);
            textTitle.text = "<color=#FFFFFF>1";
            yield return new WaitForSeconds(1);
            textTitle.text = "<color=#FFFFFF>시작 !";
            yield return new WaitForSeconds(1f);

            mainUi.GetComponent<GraphicRaycaster>().enabled = true;
            //subUi.gameObject.SetActive(false);
            VisibleStartBtn(false);
            board.gameObject.SetActive(true);
            beforeStart = false;
            leGogo = true;
            firstSelect = true;

            if (!beforeStartGuideCheck)
            {
                collectData.AddTimeStamp("GUIDE SKIP");
            }

            collectData.AddTimeStamp("MISSION START");
        }

        public void ShowFinishPanel()
        {
            PlaySoundByTypes(ESoundType.Click);
            //board.gameObject.SetActive(false);
            VisibleBoard(false);
            //finishPanel.gameObject.SetActive(true);
            VisibleFinPanel(true);
        }

        public void FinishPanel_No()
        {
            PlaySoundByTypes(ESoundType.Click);
            //board.gameObject.SetActive(true);
            VisibleBoard(true);
            //finishPanel.gameObject.SetActive(false);
            VisibleFinPanel(false);

            selectNoCtn += 1;
        }

        public void FinishPanel_Yes(bool isSkip)
        {
            
            Debug.Log("isSkip = " + isSkip);
            // 완료한 계획표 횟수
            completionCtn += 1;
            
            PlaySoundByTypes(ESoundType.Click);
            
            // n번째로 완성한 계획표 변수로 저장
            SortedCardData(isSkip);
            
            //몇번째 완료인지 체크 
            if (completionCtn == 1) // 첫번째 완료라면 아래의 프로세싱 후 재 시작
            {
                //board.gameObject.SetActive(true);
                VisibleBoard(true);
                mainUi.GetComponent<GraphicRaycaster>().enabled = false;
                //finishPanel.gameObject.SetActive(false);
                VisibleFinPanel(false);
                ReSetAll();
                StartCoroutine(hud.GetComponent<HUDSchedule>().HalfInfoSetUiTxt());
                return;
            }

            collectData.AddTimeStamp("MISSION END");

            leGogo = false;

            //board.gameObject.SetActive(false);
            //finishPanel.gameObject.SetActive(false);
            VisibleFinPanel(false);
            wellDoneAndBye.gameObject.SetActive(true);

            voiceRecording.StopRecordingNBehavior();
            
            // if (isSkip)
            // {
            //     skipYn = 1;
            //     subUi.gameObject.SetActive(false);
            //     //Schedule.gameObject.SetActive(true);
            // }
            // else
            // {
            //     skipYn = 0;
            //     string MySchedule = "";
            //     string MyScheduleforJson = "";
            //     Transform plan_Box;
            //
            //     foreach (Transform plan_Slot in slotList)
            //     {
            //         if (plan_Slot.GetComponent<PlanSlotController1>().passenger.transform != null)
            //         {
            //             plan_Box = plan_Slot.GetComponent<PlanSlotController1>().passenger.transform;
            //             
            //             Debug.Log("passenger = " + plan_Box.name);
            //         
            //             if (plan_Box != null)
            //             {
            //                 //MySchedule += plan_Box.GetChild(0).GetComponent<Text>().text + " ";
            //                 //MyScheduleforJson += plan_Box.GetChild(1).name;
            //                 //아래 코드 514,515는 디버깅용
            //                 MySchedule = "3";
            //                 MyScheduleforJson = "3";
            //             }
            //             else
            //             {
            //                 MySchedule += "0 ";
            //                 MyScheduleforJson += "0";
            //             }
            //         }
            //
            //         else
            //         {
            //             return;
            //         }
            //     }
            //     
            //     _planData01 = float.Parse(MyScheduleforJson, System.Globalization.CultureInfo.InvariantCulture);
            // }
            
            /*
            Data_201 계획을 완료하는데 걸린 총 시간                               TotalElapsedTimeForCalc
            Data_202 계획을 얼마나 바꾸는지(이동)                                 TotalMovingCnt
            Data_203 계획 초기화(다시하기)를 누른 횟수                            ResetCnt
            Data_204 완료 결정을 못하고 번복한 횟수                               ClickNoCnt
            Data_205 첫번째로 완료된 계획 전송                                   PlanData01
            Data_206 두번째로 완료된 계획 전송                                   PlanData02
            Data_207 중도 포기(스킵)                                              SkipYn
            Data_208 시작버튼 누르기 까지 걸린 시간                               TimerForBeforeStarted
            Data_209 시작하기 누른 후 첫번째 계획 선택까지 걸린 시간              TimerForFirstSelect
            Data_210 미션과 관계없는 생물을 건든 횟수
            Data_211 첫번째 계획표 점수
            Data_212 두번째 계획표 점수
            Data_213 제한된 카드를 사용한 횟수
            */


            // 흩어져 있는 데이터들을 배열에 넣어 전달할 준비
            Scene2Arr = new[] { totalElapsedTimeForCalc, totalMovingCnt, resetCnt, selectNoCtn, _planData01,_planData02, 
                                skipYn, timerForBeforeStarted, timerForFirstSelect, data_210, data_211, data_212, data_213 };
            gameDataManager.GetComponent<GameDataManager>().SaveCurrentData();
            StartCoroutine(GoToNextScene());
        }

        private void VisibleBoard(bool isOn)
        {

            if (isOn)
            {
                defCards.SetActive(true);
                board.GetComponent<CanvasGroup>().alpha = 1;
                board.GetComponent<CanvasGroup>().blocksRaycasts = true;

                foreach (var currCard in grpList)
                {
                    //currCard = currCard.GetComponent<PlanSlotController1>().passenger.transform;
                    currCard.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
                    //currCard.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            
            else
            {
                defCards.SetActive(false);
                board.GetComponent<CanvasGroup>().alpha = 0;
                board.GetComponent<CanvasGroup>().blocksRaycasts = false;
                
                foreach (var currCard in grpList)
                {
                    //currCard = currCard.GetComponent<PlanSlotController1>().passenger.transform;
                    currCard.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
                    //currCard.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
        
        private void VisibleFinPanel(bool isOn)
        {
            if (isOn)
            {
                finishPanel.GetComponent<CanvasGroup>().alpha = 1;
                finishPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            
            else
            {
                finishPanel.GetComponent<CanvasGroup>().alpha = 0;
                finishPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }

        public void VisibleStartBtn(bool isOn)
        {
            if (isOn)
            {
                startBtn.GetComponent<CanvasGroup>().alpha = 1;
                startBtn.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            
            else
            {
                startBtn.GetComponent<CanvasGroup>().alpha = 0;
                startBtn.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
        
        private void SortedCardData(bool isSkip)
        {
            if (isSkip)
            {
                skipYn = 1;
                subUi.gameObject.SetActive(false);
                //Schedule.gameObject.SetActive(true);
            }
            else
            {
                Transform currCard;
                skipYn = 0;
                var myScheduleForJson = "";
                
                foreach (var slot in slotList)
                {
                    if (slot.GetComponent<PlanSlotController1>().passenger == null) continue;
                    currCard = slot.GetComponent<PlanSlotController1>().passenger.transform;
                        
                    if (slot != null)
                    {
                        var text = currCard.GetChild(0).GetComponent<Text>().text + " ";
                        myScheduleForJson += currCard.GetChild(1).name;
                    }
                    else
                    {
                        myScheduleForJson += "0";
                    }
                }

                switch (completionCtn)
                {
                    case 1 :
                        _planData01 = float.Parse(myScheduleForJson, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case 2 :
                        _planData02 = float.Parse(myScheduleForJson, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    default:
                        Debug.Log("계획표를 완료한 횟수가 유효하지 않습니다");
                        break;
                }
            }
        }
        
        private IEnumerator GoToNextScene()
        {
            yield return new WaitForSeconds(2);

            finishCntDwn.text = "3";
            yield return new WaitForSeconds(1);

            finishCntDwn.text = "2";
            yield return new WaitForSeconds(1);

            finishCntDwn.text = "1";
            yield return new WaitForSeconds(1);
            SceneLoader.LoadScene(3);
        }

        public void PlaySoundByTypes(ESoundType soundType)
        {
            audioSource.clip = soundType switch
            {
                ESoundType.Click => soundClick,
                ESoundType.In => soundIn,
                ESoundType.Cnt => soundCount,
                ESoundType.Put => soundPut,
                _ => null
            };
            audioSource.Play();
        }
    }
}