using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using KetosGames.SceneTransition;
//using HutongGames.PlayMaker.Actions;

namespace UserData
{
    [System.Serializable]
    public class InputText : MonoBehaviour
    {
        [SerializeField] private Text txt_Name;
        [SerializeField] private Text txt_Age;
        [SerializeField] private Text txt_Fon;
        [SerializeField] private Text txt_Loca;

        [SerializeField] private InputField inputTxt_Name;
        [SerializeField] private InputField inputTxt_Age;
        [SerializeField] private InputField inputTxt_Fon;
        [SerializeField] private InputField inputTxt_Loca;

        [SerializeField] private Toggle genderTg_M;
        [SerializeField] private Toggle genderTg_W;
        [SerializeField] private Toggle gradeTg_L;
        [SerializeField] private Toggle gradeTg_H;
        [SerializeField] private Toggle testMode_Tg;

        [SerializeField] private PlayMakerFSM warningFSM;        
        [SerializeField] private SetPlayerData setPlayerData;       

        private bool isProb;

        private void Awake()
        {
            // KeyInput 컴포넌트가 현재 Scene에 존재한다면 유저정보 입력시 Scene 이동이 될 수 있으므로 파괴한다
            if(GameObject.Find("KeyInput"))
            {
                GameObject findObj = GameObject.Find("KeyInput");
                Destroy(findObj);
            }                        
        }

        private void Start()
        {                      
            ManualXRControl.GetInstance().XR_AutoStarter();
            
            setPlayerData.ClearDataSetting();            

            inputTxt_Name.onValueChanged.AddListener(
                (word) => inputTxt_Name.text = Regex.Replace(word, @"[^가-힣]", "")
                );
            inputTxt_Loca.onValueChanged.AddListener(
                (word) => inputTxt_Loca.text = Regex.Replace(word, @"[^가-힣]", "")
                );

            // 만약 진단씬을 끝내거나 진단씬 진행중에 Input 씬으로 돌아오면 기존에 저장된 Location이 그대로 입력된다
            if (DataManager.GetInstance().userInfo.Location != null)
            {
                inputTxt_Loca.text = DataManager.GetInstance().userInfo.Location;
            }
        }

        private void Update()
        {
            //다운로드 시도 실패를 감지하면 로컬에 파일을 저장하고 서버에 업로드 실행
            //if (JsonManager.GetInstance().isError_DN)
            //{
            //    Debug.Log("isErrorDN는 참");
            //    JsonManager.GetInstance().isError_DN = false;
            //    JsonManager.GetInstance().SavePlayerDataToJson();
            //    UploadData();
            //}
        }

        public void InputAgeOnEnd()
        {
            string str = inputTxt_Age.GetComponent<InputField>().text;
            if (str.Length == 1 && str != "0")
            {
                str = "0" + inputTxt_Age.GetComponent<InputField>().text;
                inputTxt_Age.GetComponent<InputField>().text = str;
            }
        }

        public void InputAgeOnValueChanged()
        {
            if (inputTxt_Age.GetComponent<InputField>().text == "")
                return;

            string str = inputTxt_Age.GetComponent<InputField>().text;

            int result = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (!(int.TryParse(str, out result)))
                {
                    inputTxt_Age.GetComponent<InputField>().text = "";
                    return;
                }
            }
        }

        private void CreatFolder()
        {
            // Persistant Folder Path C:\Users\<계정이름>\AppData\LocalLow\HippoTnC\Strengthen_Concentration_VR\
            DataManager.GetInstance().FilePath_Root = Application.persistentDataPath + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            
            DataManager.GetInstance().userInfo.FolderName = txt_Name.text + "_" + txt_Fon.text + "_" + 
            DataManager.GetInstance().userInfo.Grade;

            DataManager.GetInstance().FilePath_Root = Application.persistentDataPath + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            DataManager.GetInstance().FilePath_Folder = DataManager.GetInstance().FilePath_Root +
            DataManager.GetInstance().userInfo.FolderName + "/";

            if (!Directory.Exists(DataManager.GetInstance().FilePath_Root))
            {
                Directory.CreateDirectory(DataManager.GetInstance().FilePath_Root);
            }
            if (!Directory.Exists(DataManager.GetInstance().FilePath_Folder))
            {
                Directory.CreateDirectory(DataManager.GetInstance().FilePath_Folder);
            }
        }

        private void Collect_UserInfo()
        {
            DataManager.GetInstance().userInfo.FolderName = txt_Name.text + "_" + txt_Age.text + "_" +
            DataManager.GetInstance().userInfo.Gender + "_" + DataManager.GetInstance().userInfo.Grade + "_" + txt_Fon.text;
        }

        public int AgeToInt(string age)
        {
            DataManager.GetInstance().userInfo.Age = int.Parse(age);
            return DataManager.GetInstance().userInfo.Age;
        }

        private void Check_Gender()
        {
            if (genderTg_M != null || genderTg_W == null)
            {                
                if (genderTg_M.isOn == true)
                {
                    DataManager.GetInstance().userInfo.IsBoy = true;                                        
                }

                else
                {
                    DataManager.GetInstance().userInfo.IsGirl = true;
                }
            }
        }

        private void Check_Grade()
        {
            DataManager.GetInstance().userInfo.Grade = gradeTg_L.isOn ? "L" : "H";
        }

        private void Check_TestMode()
        {
            DataManager.GetInstance().isTest = testMode_Tg.isOn ? true : false;
        }

        private bool ExceptionHandling_Check01()
        {
            string u_N = txt_Name.text;
            string u_A = txt_Age.text;
            string u_P = txt_Fon.text;            

            // 각 InputField에 미입력이 있는지 검사
            if (u_N == "" || u_A == "" || u_P == "")
            {
                if (u_N == "")
                {
                    SendEvent("PL Input Name");
                    Debug.Log("이름을 입력하세요!");
                }

                if (u_A == "")
                {
                    SendEvent("PL Input Age");
                    Debug.Log("나이를 입력하세요!");
                }

                if (u_P == "")
                {
                    SendEvent("PL Input Pon");
                    Debug.Log("전화번호를 입력하세요!");
                }
                isProb = true;
            }

            // 나이 입력을 올바르게 했는지 검사
            //if (u_A != "")
            //{
            //    AgeToInt(u_A);

            //    if (currentAge < minAge || currentAge > maxAge)
            //    {
            //        //SendEvent();
            //        Debug.Log("나이를 다시 입력하세요!(1~99)");
            //        isProb = true;
            //    }            
            //}
            return isProb;
        }
        
        // Location 입력이 되지 않았을때 예외 처리
        private string ExceptionHandling_Check02()
        {
            string u_L = inputTxt_Loca.text;
            if (u_L == "")
            {
                u_L = "웅진플레이도시";
                inputTxt_Loca.text = u_L;
            }

            return inputTxt_Loca.text;
        }

        public void Confirm_n_DataExistenceCheck()
        {
            SendEvent("TurnOff Messages");

            ExceptionHandling_Check01();
            ExceptionHandling_Check02();

            if (!isProb)
            {
                DataManager.GetInstance().userInfo.Name = txt_Name.text;
                DataManager.GetInstance().userInfo.Age = int.Parse(txt_Age.text);
                DataManager.GetInstance().userInfo.PhoneNumber = txt_Fon.text;                               
                DataManager.GetInstance().userInfo.Location = inputTxt_Loca.text;               

                Check_Gender();
                Check_Grade();
                Check_TestMode();

                CreatFolder();

                DataManager.GetInstance().SavePlayerDataToJson();

                GetComponent<NetworkManager>().DoSendToTextMsg();       // <<<< ---------------- 문자전송 추가                

                SceneLoader.LoadScene(11);
            }

            Reset_BoolData();
        }

        private void UploadData()
        {
            DataManager.GetInstance().StartCoroutine("UploadRoutine");
        }

        private void Reset_BoolData()
        {
            isProb = false;
            DataManager.GetInstance().isError_DN = false;
            DataManager.GetInstance().isError_UP = false;
        }

        private void SendEvent(string eventName)
        {
            warningFSM.SendEvent(eventName);
        }
    }
}

