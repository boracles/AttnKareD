using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System.Linq; AllSlotList = AllSlotList.OrderBy(obj => obj.name).ToList();

public class PlayBlockManager : MonoBehaviour
{
    /*
    public class BlockData
    {
        public int StageNo { get; set; }        // 1, 2, 3 ...
        public int BlockNo { get; set; }        // 1, 2, 3 ...
        public string SuccessYn { get; set; }   // y, n
        public string TipYn { get; set; }       // y, n
        public int FinishTime { get; set; }     // 1, 2, 3 ...
        public int DecisionCnt { get; set; }    // 1, 2, 3 ...
        public string Reason { get; set; }  // Timeout, Throw, Miss, Disturb ...
    }
*/

    public GameObject Canvas_Intro;
    public GameObject Canvas_DIsplay;
    public GameObject DIsplay_Text;
    public GameObject DIsplay_TipBtn;

    public GameObject MyBlock;
    public GameObject BotBlock;

    public Transform MyBlockPoint;
    public Transform BotBlockPoint;

    public Transform SlotGrp;

    List<Transform> AllSlotList;
    public List<Transform> SlotList_My;
    public List<Transform> SlotList_Bot;

    List<Transform> BlockList_My;
    List<Transform> BlockList_Bot;

    Transform myBlock;


    bool GameIntro = false;
    bool GameStart = false;
    bool ReDo = false;
    bool Pause = false;
    bool StageFinish = false;


    float IntroTimer = 0;

    int BlockNum = 0;           //몇번째 블록인지
    int StageLevel = 0;         //단계
    //int ShowTipTime = 20;       //지정 시간이 지나면 자동으로 팁 발동 하도록 (진행 유도 목적)
    int TipClicked = 0;         //팁보기 누른 횟수

    float TotalElapsed = 0;     //총 시간

    int TotalElapsed_Show = 0;      //총 시간 보여주기용
    //int TurnTimeElapsed_Show = 0;   //턴 제한 시간 보여주기용

    //int BlockRetryCnt = 0;  //블록 다시하기 카운팅용

    void Start()
    {
        AllSlotList = new List<Transform>();
        SlotList_My = new List<Transform>();
        SlotList_Bot = new List<Transform>();
        BlockList_My = new List<Transform>();
        BlockList_Bot = new List<Transform>();
    }

    void Update()
    {
        if (GameIntro)
        {
            IntroTimer += Time.deltaTime;

            if (IntroTimer > 1f)
            {
                GameIntro = false;
                StartCoroutine(StageManager());
            }
        }

        if (GameStart)
        {
            TotalElapsed += Time.deltaTime;

            if (TotalElapsed > 1)
            {
                TotalElapsed = 0;
                TotalElapsed_Show += 1;

/*                TurnTimeElapsed_Show += 1;

                if (TurnTimeElapsed_Show > ShowTipTime)
                {
                    ShowUpSlotTip("TIMEOUT");
                    TipClicked += 1;
                }*/

                ShowBoardUpdate();
            }
        }
    }

    void ShowBoardUpdate()
    {
        if (!Pause)
        {
            DIsplay_Text.GetComponent<Text>().text = "전체시간:" + TotalElapsed_Show.ToString() + " / 턴:" + StageLevel.ToString() + " / 팁재생:" + TipClicked.ToString();
        }
    }

    public void DoStartGame()
    {
        Canvas_Intro.SetActive(false);
        Canvas_DIsplay.SetActive(true);

        GameIntro = true;
    }


    IEnumerator StageManager()
    {
        for (int stage = 0; stage < 10; stage++)
        {
            StageFinish = false;
            DIsplay_TipBtn.SetActive(false);

            DIsplay_Text.GetComponent<Text>().text = "초록색칸은 내칸, 붉은색칸은 친구칸";
            yield return new WaitForSeconds(2);

            if (!ReDo)
            {
                StageLevel += 1;
                DIsplay_Text.GetComponent<Text>().text = StageLevel.ToString() + " 단계 (" + StageLevel.ToString() + "개 맞추기)";
            }
            else
            {
                DIsplay_Text.GetComponent<Text>().text = StageLevel.ToString() + " 단계 다시하기";
            }

            SetStage();

            float waitSec = 0;

            if (StageLevel < 4)
            {
                waitSec = 3f;
            }
            else if (StageLevel >= 4 && StageLevel < 7)
            {
                waitSec = 5f;
            }
            else if (StageLevel >= 7 && StageLevel < 10)
            {
                waitSec = 7f;
            }
            else
            {
                waitSec = 10f;
            }

            ShowUpSlotTip("START");

            yield return new WaitForSeconds(waitSec);


            SetNextMyBlock();

            Pause = false;
            GameStart = true;
            DIsplay_TipBtn.SetActive(true);

            yield return new WaitUntil(() => StageFinish);


            //결과 저장

            Pause = true;
            //TurnTimeElapsed_Show = 0;

            foreach (Transform myB in BlockList_My)
            {
                Destroy(myB.gameObject);
            }
            foreach (Transform botB in BlockList_Bot)
            {
                Destroy(botB.gameObject);
            }

            SlotList_My.Clear();
            SlotList_Bot.Clear();

            BlockList_My.Clear();
            BlockList_Bot.Clear();
        }

        //결과 전송

    }

    public void ShowUpSlotTip(string Type)
    {
        foreach (Transform MySlot in SlotList_My)
        {
            MySlot.GetComponent<SlotController>().ShowTipUp(StageLevel);
        }

        foreach (Transform BotSlot in SlotList_Bot)
        {
            BotSlot.GetComponent<SlotController>().ShowTipUp(StageLevel);
        }

        if (Type != "START")
        {
            TipClicked += 1;
            ShowBoardUpdate();
        }

        //TurnTimeElapsed_Show = 0;
    }



    //실패 호출
    public void ReDoStage()
    {
        ReDo = true;
        StageFinish = true;
    }



    //내 블럭 하나 끝날때 마다 호출
    public void SetNextMyBlock()
    {
        if (BlockNum < StageLevel)
        {
            BlockList_My[BlockNum].transform.gameObject.SetActive(true);
            BlockList_My[BlockNum].transform.position = MyBlockPoint.position;
            BlockList_My[BlockNum].GetComponent<BlockController>().NowActivated = true;

            BlockList_Bot[BlockNum].transform.gameObject.SetActive(true);
            BlockList_Bot[BlockNum].transform.position = BotBlockPoint.position;

            BlockNum += 1;
        }
        else
        {
            StageFinish = true;
        }
    }




    void SetStage()
    {
        if (!ReDo)
        {
            if (AllSlotList.Count != 0)
            {
                AllSlotList.Clear();
            }

            for (int s = 0; s < SlotGrp.childCount; s++)
            {
                AllSlotList.Add(SlotGrp.GetChild(s));
            }

            while (true)
            {
                Transform MySlot = AllSlotList[Random.Range(0, AllSlotList.Count)];

                if (!SlotList_My.Contains(MySlot))
                {
                    MySlot.GetComponent<SlotController>().BlockType = "MY";
                    SlotList_My.Add(MySlot);
                    AllSlotList.Remove(MySlot);
                    MySlot = null;
                }

                if (SlotList_My.Count == StageLevel)
                {
                    break;
                }
            }

            while (true)
            {
                Transform BotSlot = AllSlotList[Random.Range(0, AllSlotList.Count)];

                if (!SlotList_Bot.Contains(BotSlot))
                {
                    BotSlot.GetComponent<SlotController>().BlockType = "BOT";
                    SlotList_Bot.Add(BotSlot);
                    AllSlotList.Remove(BotSlot);
                    BotSlot = null;
                }

                if (SlotList_Bot.Count == StageLevel)
                {
                    break;
                }
            }


            for (int n = 0; n < StageLevel; n++)
            {
                BlockList_My.Add(Instantiate(MyBlock).transform);
                BlockList_Bot.Add(Instantiate(BotBlock).transform);
            }


        }
        else
        {
            foreach (Transform MySlot in SlotList_My)
            {
                MySlot.GetComponent<SlotController>().ResetAllSlotData();
            }

            foreach (Transform BotSlot in SlotList_Bot)
            {
                BotSlot.GetComponent<SlotController>().ResetAllSlotData();
            }

            foreach (Transform MyBlock in BlockList_My)
            {
                MyBlock.GetComponent<BlockController>().ResetBlock();
            }

            foreach (Transform BotBlock in BlockList_Bot)
            {
                //BotBlock.GetComponent<BlockController>().ResetBlock();
            }
        }

        BlockNum = 0;
    }


    public void UpdateBlockData()
    {

    }

}
