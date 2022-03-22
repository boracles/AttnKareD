using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BNG;
using UnityEngine.Serialization;

namespace Scheduler
{
    public class PlanCubeController1 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {

        public ScheduleManager1 scheduleManager;

        public enum CardState { Idle, Move, Enter, Done }
        
        public CardState cardState;

        [FormerlySerializedAs("HandCursor")] public Transform handCursor;
        public HandController handController;

        [FormerlySerializedAs("Canvas")] public Transform canvas;
        [FormerlySerializedAs("Grp")] public Transform grp;

        public GameObject activeSlot;
        public GameObject intoSlot;
        public GameObject workingSlot;
        public Vector3 startPos;

        public float t;
        public bool isHomeTW;

        [FormerlySerializedAs("cardCreator")] [SerializeField] OriginPosController originPosController;

        [FormerlySerializedAs("originPos_P")] [SerializeField] GameObject originPosP;


        //[SerializeField] Transform originPos;
        [SerializeField] private GameObject cardPrb;
        [SerializeField] private GameObject prevActSlot;
        [SerializeField] private GameObject prevSlot;
        [SerializeField] Transform cube;
        
        //[SerializeField] bool isWorking;

        private UIPointer _uiPointer;

        private Vector3 _vec2Pos;
        private Vector3 _zPos;

        //[SerializeField] bool working;
        [SerializeField] private bool nowClicked = false;
        private bool _pointerOnCube = false;
        
        //test
        [SerializeField] private List<GameObject> slots;

        private void Start()
        {
            FindOriginPos();

            slots = new List<GameObject>();
            
            intoSlot = null;
            workingSlot = null;
            cube = null;
            cardState =  CardState.Idle;
            cardPrb = gameObject;
            _zPos.z = 2.21874f;
            startPos = transform.localPosition;
            _uiPointer = handCursor.GetComponent<BNG.UIPointer>();
        }

        private void FixedUpdate()
        {
            if (!nowClicked) return;
            if (handCursor.GetComponent<LineRenderer>().enabled)
            {
                if (_pointerOnCube)
                {
                    MoveCard();
                }
            }
            else
            {
                Debug.Log("???");
            }

            if (_pointerOnCube || handController.PointAmount != 1) return;
            transform.SetParent(grp);
            //scheduleManager.ReleaseAllCollision(); 기존 자리
            scheduleManager.PlaySoundByTypes(ESoundType.Put);

            nowClicked = false;
            intoSlot = null;
            workingSlot = null;

            transform.localPosition = activeSlot == null ? startPos : activeSlot.transform.localPosition;
        }

        private void FindOriginPos()
        {
            originPosP = GameObject.Find("Origin Pos");
            var myName = name.Replace("(Clone)", "");
            originPosController = originPosP.transform.Find(myName).GetComponent<OriginPosController>();            
        }

        private void MoveCard()
        {
            // LaserEnd의 포지션을 따라간다
            cardState = CardState.Move;
            Vector2 a = transform.position;
            Vector2 b = _uiPointer._cursor.transform.position;
            _vec2Pos = Vector2.Lerp(a, b, t);
            _vec2Pos.z = _zPos.z;
            transform.position = _vec2Pos;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!nowClicked)
            {
                //scheduleManager.InitGrpList();
                transform.SetParent(canvas);
                scheduleManager.LockAllCollision(transform);
                nowClicked = true;
            }
            
            //아래 조건문에 activeSlot아니고 intoSlot일 수도 있음
            if (activeSlot != null && !scheduleManager.pointerLock)
            {
                scheduleManager.pointerLock = true;
                MeshRendererOn(activeSlot);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            scheduleManager.pointerLock = false;
            transform.SetParent(grp);
            //scheduleManager.ReleaseAllCollision(); // 262번 줄로 이동함
            scheduleManager.PlaySoundByTypes(ESoundType.Put);
            
            if (workingSlot != null) //슬롯에 들어온 경우
            {
                if (workingSlot == null)
                {
                    if(activeSlot == null) return;
                    transform.localPosition = activeSlot.transform.localPosition;
                    return;
                }

                // 슬롯에 큐브가 들어가는 조건 탐색
                if (workingSlot.GetComponent<PlanSlotController1>().passenger == null) //새로운 슬롯이 비어있는 경우
                {
                    if (activeSlot != null)
                    {
                        activeSlot.GetComponent<PlanSlotController1>().passenger = null;
                    }

                    if (!isHomeTW && !originPosController.isStored)
                    {
                        if(!scheduleManager.isReset)
                        {
                            InstantiateCard(cardPrb);
                        }                        
                    }

                    activeSlot = workingSlot;
                    transform.localPosition = activeSlot.transform.localPosition;                    
                    activeSlot.GetComponent<PlanSlotController1>().passenger = this.gameObject;
                    //activeSlot.GetComponent<PlanSlotController1>().inSlot = true;
                    MeshRendererOff(activeSlot);
                    cardState = CardState.Done;
                }
                else
                {
                    //새로운 슬롯에 이미 계획이 있는 경우
                    if (activeSlot == null /*&& workingSlot.GetComponent<PlanSlotController1>().passenger == null*/) //현재 할당된 슬롯이 없는 경우
                    {
                        // 새로운 슬롯에 있던 카드 먼저 처리 프로세스
                        var cardB = workingSlot.GetComponent<PlanSlotController1>().passenger;
                        cardB.GetComponent<PlanCubeController1>().originPosController.CardDestroyer(cardB);
                        //cardB.GetComponent<PlanCubeController1>().activeSlot = null;
                        activeSlot = workingSlot;
                        //workingSlot.GetComponent<PlanSlotController1>().passenger.GetComponent<PlanCubeController1>().resetPlanCube(0.07f);
                        if (!isHomeTW && originPosController.storedCard == null)
                        {
                            if(!scheduleManager.isReset)
                            {
                                InstantiateCard(cardPrb);
                            }                        
                        }
                    }
                    
                    //현재 할당된 슬롯이 있어서 바꿔치기 하는 경우 // A큐브가 -> B슬롯으로 옮김, B큐브가 -> A슬롯으로 옮겨짐
                    else if (activeSlot != null)
                    {
                        if (activeSlot.GetComponent<PlanSlotController1>().passenger !=
                            workingSlot.GetComponent<PlanSlotController1>().passenger)
                        {
                            // 내 큐브를 tempCardA에 복사
                            var tempCardA = gameObject;
                            // A슬롯을 tempSlotA에 복사
                            var tempSlotA = activeSlot;
                            if (prevActSlot != null)
                            {
                                prevActSlot = activeSlot;
                            }
                            // B슬롯에 있는 passenger(B큐브)를 tempCard에 임시로 복사
                            var tempCardB = workingSlot.GetComponent<PlanSlotController1>().passenger;
                            // B큐브에 있는 activeSlot(B슬롯)을 tempSlot에 임시로 복사
                            var tempSlotB = tempCardB.GetComponent<PlanCubeController1>().activeSlot;
                            //이전 슬롯인 prevSlot의 포지션으로 B큐브를 옮김
                            tempCardB.transform.localPosition = tempSlotA.transform.localPosition;
                            // A가 원래 가지고 있던 activeSlot을 B에게 복사 밀어넣음 (내 activeSlot을 B로 아직 바꾸지 않음)
                            tempCardB.GetComponent<PlanCubeController1>().activeSlot = tempSlotA;
                            // A slot에 B카드를 넣음
                            tempSlotA.GetComponent<PlanSlotController1>().passenger = tempCardB;
                            // B card의 상태를 Done으로 변경 + intoSlot = null // B card 이동 완료
                            tempCardB.GetComponent<PlanCubeController1>().cardState = CardState.Done;
                            tempCardB.GetComponent<PlanCubeController1>().intoSlot = null;
                            // B카드 관련 후속 처리 
                            MeshRendererOff(tempSlotA);
                        
                            // A 카드 이동 로직 시작
                            // 내 actSlot에 B슬롯을 넣은 후 작업은 현재의 조건문 아래에서 처리됨
                            activeSlot = tempSlotB;
                            //activeSlot.GetComponent<PlanSlotController1>().passenger = tempCardB;
                        }
                    }
                    
                    // A card(this)의 위치를 actSlot(B slot)의 위치로 옮기고
                    transform.localPosition = activeSlot.transform.localPosition;
                    // actSlot(B slot)의 passenger에 A card(this)를 넣는다. 
                    activeSlot.GetComponent<PlanSlotController1>().passenger = gameObject;
                    // A card(this) 상태 변경해주고 이동 마무리
                    cardState = CardState.Done;
                    MeshRendererOff(activeSlot);
                    //activeSlot = intoSlot;
                }

                scheduleManager.CheckMovingCnt();
                scheduleManager.CheckAllScheduleOnSlot(); 
            }
            else
            {
                if (activeSlot != null)
                {
                    MeshRendererOff(activeSlot);
                    cardState = CardState.Done;
                    transform.localPosition = activeSlot.transform.localPosition;
                }
                else
                {
                    cardState = CardState.Idle;
                    transform.localPosition = startPos;
                }
            }

            nowClicked = false;
            intoSlot = null;
            workingSlot = null;
            //scheduleManager.InitGrpList();
            scheduleManager.ReleaseAllCollision(); //문제가 생기면 원래위치로 :138
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("SLOT"))
            {
                intoSlot = collision.gameObject;
                slots.Add(intoSlot);
            }

            if (collision.collider.CompareTag("POINTER"))
            {
                _pointerOnCube = true;
                scheduleManager.PlaySoundByTypes(ESoundType.In);
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            // 슬롯에 들어왔고 들어온 슬롯 갯수가 딱 1개만인지 확인
            if (!collision.collider.CompareTag("SLOT")) return;
            if (slots.Count != 1) return;
            workingSlot = collision.collider.gameObject;
            //isWorking = true;
            
            if(workingSlot != null && nowClicked)
            {
                cardState = CardState.Enter;
                cube = workingSlot.gameObject.transform.Find("Cube");
                cube.GetComponent<MeshRenderer>().material.color = new Color(0.67f, 0, 0.545f, 0.7f);
                Debug.Log("색을 진하게 바꿉니다");
            }
                    
            if(!nowClicked && cube.GetComponent<MeshRenderer>().enabled == false)
            {
                cardState = CardState.Done;
                if (prevActSlot == null)
                {
                    prevActSlot = activeSlot;
                    Debug.Log("첫번째 act슬롯을 가졌으니 prevActSlot에 복사합니다");
                }
            }

        }

        private void OnCollisionExit(Collision collision)
        {
            switch (collision.collider.tag)
            {
                case "SLOT":
                {
                    //isWorking = false;
                    prevSlot = collision.gameObject;
                    slots.Remove(prevSlot);
                    cube = prevSlot.gameObject.transform.Find("Cube");
                    cube.GetComponent<MeshRenderer>().material.color = new Color(0.67f, 0, 0.545f, 0.12f); 
                    Debug.Log("색을 연하게 바꿉니다");
                    cardState = CardState.Move;
                    if (slots.Count == 0)
                    {
                        intoSlot = null;
                        workingSlot = null;
                    }

                    break;
                }
                case "POINTER":
                    _pointerOnCube = false;
                    break;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Birthplace") && !isHomeTW)
            {
                isHomeTW = true;
                if(prevActSlot != null) return;
                activeSlot = null;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Birthplace") && isHomeTW)
            {
                isHomeTW = false;
            }
        }

        private void MeshRendererOn(GameObject actSlot)
        {
            cube = actSlot.gameObject.transform.Find("Cube");
            cube.GetComponent<MeshRenderer>().enabled = true;
        }
        
        private void MeshRendererOff(GameObject actSlot)
        {
            cube = actSlot.gameObject.transform.Find("Cube");
            cube.GetComponent<MeshRenderer>().material.color = new Color(0.67f, 0, 0.545f, 0.12f);
            cube.GetComponent<MeshRenderer>().enabled = false;
        }

        private void InstantiateCard(GameObject thisG)
        {
            var cloneCard = Instantiate(thisG);
            scheduleManager.grpList.Add(cloneCard.transform);
            cloneCard.GetComponent<PlanCubeController1>().activeSlot = null;
            cloneCard.transform.SetParent(grp);
            cloneCard.transform.localPosition = startPos;
            cloneCard.transform.localScale = new Vector3(1, 1, 1);
        }

        public IEnumerator ResetPlanCube(float wait)
        {
            if (activeSlot != null)
            {
                cardState = CardState.Idle;
                activeSlot = null;
            }
            
            intoSlot = null;
            workingSlot = null;
            slots.Clear();
            transform.localPosition = startPos;
            yield return new WaitForSeconds(wait);
        }
    }

}

