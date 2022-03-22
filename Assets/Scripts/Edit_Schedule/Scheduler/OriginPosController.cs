using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Scheduler
{
    public class OriginPosController : MonoBehaviour
    {
        public bool isStored;
        public GameObject storedCard;

        [SerializeField] private GameObject originPos;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private ScheduleManager1 schManager;
        
        [SerializeField] private string otherName;

        private GameObject _tempSlot;
        private string _word;
        
        private void Start()
        {
            schManager = FindObjectOfType<ScheduleManager1>();
            originPos = gameObject;
            _word = "(Clone)";
        }
        
        public void CardDestroyer(GameObject cardB)
        {
            if (storedCard == null) //같은 계열의 카드일때 
            {
                Debug.Log("1");
                storedCard = cardB;
                cardB.transform.localPosition = originPos.transform.localPosition;
                cardB.GetComponent<PlanCubeController1>().activeSlot = null;
                Debug.Log("originsPos가 비어서 cardB를 옮겨옴");
            }
            
            // 기존 슬롯 카드의 origin pos에 예비카드가 있을때
            else if (storedCard != null)
            {
                // cardB가 origin인지 체크
                if (!RemoveWord.EndsWithWord(cardB.name, _word))
                {
                    
                    cardB.transform.localPosition = originPos.transform.localPosition;
                    if (storedCard != null)
                    {
                        Debug.Log("destroy originPos Card = " + storedCard);
                        schManager.grpList.Remove(storedCard.transform);
                        Destroy(storedCard);
                        
                    }
                    //cardB.GetComponent<PlanCubeController1>().activeSlot = null;
                    Debug.Log("storedCard =" + storedCard.name);
                    
                    //storedCard도 origin일 경우 체크
                    if (!RemoveWord.EndsWithWord(storedCard.name, _word))
                    {
                        Debug.Log("3");
                        schManager.grpList.Remove(cardB.transform);
                        Destroy(cardB);
                    }
                    //Destroy(storedCard);
                    //storedCard = cardB;
                    Debug.Log("cardB가 원본이라 origin pos로 옮겨짐");
                }
                
                else if (!RemoveWord.EndsWithWord(storedCard.name, _word))
                {
                    schManager.grpList.Remove(cardB.transform);
                    Destroy(cardB);
                    Debug.Log("origin pos에 있는 카드가 원본이라 cardB는 삭제함");
                }
            }

            else
            {
                Debug.Log("아무 조건문도 안거침");
            }
            storedCard.GetComponent<PlanCubeController1>().activeSlot = null;
            Debug.Log((storedCard.name));
            Debug.Log(storedCard.GetComponent<PlanCubeController1>().activeSlot == null);
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("PLAN")) return;
            other.GetComponent<PlanCubeController1>().cardState = PlanCubeController1.CardState.Idle;
            otherName = other.name.Replace("(Clone)", "");

            if (name == otherName && !isStored)
            {                    
                other.GetComponent<PlanCubeController1>().isHomeTW = true;
                isStored = true;
                storedCard = other.gameObject;
            }  
                
            // 리셋 버튼을 눌러 전체 카드 리셋을 하려고 하는데 Origin Pos안에 카드가 들어 있을 경우 해당 카드를 삭제
            else if(storedCard != null && schManager.isReset)
            {
                if(!RemoveWord.EndsWithWord(storedCard.name, _word)) return;
                schManager.grpList.Remove(storedCard.transform);
                Destroy(storedCard);
                isStored = false;
                storedCard = null;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("PLAN")) return;
            otherName = other.name.Replace("(Clone)", "");
                
            if (name != otherName || !isStored) return;
            other.GetComponent<PlanCubeController1>().isHomeTW = false;
            isStored = false;
            storedCard = null;
        }

        public void ResetOriginPos()
        {
            if (storedCard == null) return;
            const string keyword = "(Clone)";
            if (RemoveWord.EndsWithWord(storedCard.name, keyword))
            {
                schManager.grpList.Remove(storedCard.transform);
                Destroy(storedCard);
            }
            storedCard = null;
        }
    }
}

