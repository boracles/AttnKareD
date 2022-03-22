using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;

namespace dw.game.doorlock
{
    public partial class GameDoorLock : MonoBehaviour
    {
        const int GAME_PASS_WORD = 5885;
        const int GAME_PASS_WORD_LENGTH = 4;

        public Canvas mainCanvas;
        public Canvas debugCanvas;
        public GameObject doorlockModel;
        public GameObject doorlockCap;
        public BNG.BNGPlayerController playerController;

        float firstinfoWaitTime = 5f;
        float fadeDuration = 1f;

        public GameObject[] showInGameGroup;
        List<Transform> pads = new List<Transform>();
        List<Transform> numbers = new List<Transform>();

        IEnumerator Start()
        {
            var firstInfo_Panel = mainCanvas.transform.Find("firstInfo_Panel");
            {
                firstInfo_Panel.GetComponent<BoxCollider>().enabled = true;
            }

            var audioSource = playerController.GetComponent<AudioSource>();

            var mainGroup = mainCanvas.GetComponent<CanvasGroup>();

            var modelDoorLock = transform.Find("modelDoorLock");
            var doorLockBody = modelDoorLock.Find("doorlock_body_02");
            var doorLockCap = modelDoorLock.Find("doorlock_body_03");
            doorLockCap.gameObject.SetActive(false);
            var doorLockdest = transform.Find("doorLockdest");

            audioSource.PlayOneShot(clipFirst);

            foreach (Transform trans in doorlockCap.transform) 
            {
                if (trans.name.Contains("number_")) 
                {
                    trans.name = trans.name.Replace("number_", string.Empty);
                    numbers.Add(trans);
                }
                else if (trans.name.Contains("pad_")) 
                {
                    trans.name = trans.name.Replace("pad_", string.Empty);
                    pads.Add(trans);
                }
            }

            pads.ForEach(_ =>
            {
                var btn = _.transform.Find("Canvas/Button").GetComponent<Button>();
                btn.onClick.AddListener(() => 
                {
                    Debug.Log("" + _.name);
                    var index = int.Parse(_.name);
                    var tween = numbers[index].GetComponent<MeshRenderer>().material.DOColor(Color.red, 1f);
                    tween.OnComplete(() =>
                    {
                        numbers[index].GetComponent<MeshRenderer>().material.DOColor(Color.gray, 1f);
                    });
                });
            });

            showInGameGroup.ToList().ForEach(_ => { _.gameObject.SetActive(false); });

            void ShowGroup(bool show)
            {
                if (show)
                {
                    mainGroup.DOFade(1, fadeDuration);
                    mainGroup.transform.DOScale(1, fadeDuration);
                }
                else
                {
                    mainGroup.DOFade(0, fadeDuration);
                    mainGroup.transform.DOScale(0, fadeDuration);
                }
            }

            ShowGroup(true);
            yield return new WaitForSeconds(firstinfoWaitTime);

            //Fly to User
            {
                ShowGroup(false);
                yield return new WaitForSeconds(fadeDuration);
                doorLockCap.gameObject.SetActive(true);

                var flyDuration = 1.5f;
                var tween = doorLockCap.DOMove(doorLockdest.position, 1.5f).OnComplete(() =>{});

                yield return new WaitForSeconds(0.5f);
                doorLockCap.DOPunchScale(new Vector3(0.1f, 0.1f, 0.05f), flyDuration, 3);

                var originScale = doorLockCap.transform.localScale;
                doorLockCap.DOScale(originScale * 3f, flyDuration);

                showInGameGroup.ToList().ForEach(_ => { _.gameObject.SetActive(true); });
            }

            //touchable


            yield return null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O)) 
            {
                //pads.ForEach(_ =>
                //{
                //    var btn = _.transform.Find("Canvas/Button").GetComponent<Button>();
                //    Debug.Log("" + _.name);
                //    var index = int.Parse(_.name);
                //    var tween = numbers[index].GetComponent<MeshRenderer>().material.DOColor(Color.red, 1.5f);
                //    tween.OnComplete(() =>
                //    {
                //        numbers[index].GetComponent<MeshRenderer>().material.DOColor(Color.green, 1.5f);
                //    });
                //});

                //numbers[0].GetComponent<MeshRenderer>().material.color = Color.red;

                //var tween = numbers[0].GetComponent<MeshRenderer>().material.DOColor(Color.red, 1.5f);
                //tween.OnComplete(() =>
                //{
                //    numbers[0].GetComponent<MeshRenderer>().material.DOColor(Color.green, 1.5f);
                //});

                //var tween = numbers[0].GetComponent<MeshRenderer>().material.DOColor(Color.red, 1.5f);
                //tween.OnComplete(() =>
                //{
                //    numbers[0].GetComponent<MeshRenderer>().material.DOColor(Color.green, 1.5f);
                //});

            }
            if (Input.GetKeyDown(KeyCode.P))
            {

            }

        }
    }

    public partial class GameDoorLock//+Resources
    {
        [Header("voice")]
        public AudioClip clipFirst;
        public AudioClip clipTimeOver;
        public AudioClip clipPassword;
        public AudioClip clipFinal;

        [Header("sfx")]
        public AudioClip clipComplete;
        public AudioClip clipIncorrect;
        public AudioClip clipDoorCliderEnter;
        public AudioClip clipClick;
    }
}
