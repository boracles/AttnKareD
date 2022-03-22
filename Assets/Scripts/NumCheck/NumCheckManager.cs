using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BNG;
using UnityEngine.UI;

public class NumCheckManager : MonoBehaviour
{
    public GameObject RighthandPointer;
    public DataCheck_NumMatch DataCheck;
    public GrabbedTimer GrabTimer;
    public Grabber numGrabber;
    public GameObject Triggers;
    public GameObject hitCollision = null;
    public GameObject[] arrCards;
    
    [Header("UI")]
    public Canvas finCanvas;
    public Canvas startCanvas;
    public TextMeshProUGUI answerText;
    [Tooltip("Add SpriteImage for Distraction")]
    public Sprite[] DistracImage;
    public GameObject ImagePrefab;

    [Header("Debug")]
    public int answerInt = 0;
    [Tooltip("Current Array of Answers")]
    public string[] arrOrder;
    string[] arrAnswer = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };
    public GameObject[] arrGb;

    Transform _lastCollision;
    Grabbable PrevGrabbable;
    GameObject prevhitCollision;
    IEnumerator currentCoroutine;
    float accumTime = 0f;
    float fadeTime = 1f;
    int sprite = 0;
    bool fade;


    // Start is called before the first frame update
    void Start()
    {
        arrOrder = new string[15];
    

        string[] arrNum = new string[arrCards.Length];

        for (int i = 0; i < arrCards.Length; i++)
        {
            arrNum[i] = (i + 1).ToString();
        }

        ShuffleNum(arrNum); //shuffle numbers

        for (int count = 0; count < arrCards.Length; count++)
        {
            string num_s = arrNum[count];
            int num = int.Parse(num_s);
            arrCards[count].GetComponent<NumCard>().cardNum = num_s;
            arrCards[count].GetComponent<NumCard>().SetCardNum();
            if(num > arrCards.Length - DistracImage.Length)
            {
                SetSprite(arrCards[count]);
            }
            
            Debug.Log(arrCards[count].transform.name);

        }
        
    }

    private void SetSprite(GameObject card)
    {
        GameObject image = Instantiate(ImagePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        image.transform.SetParent(card.GetComponentInChildren<Canvas>().transform);
        RectTransform imageRect = image.GetComponent < RectTransform >();
        imageRect.anchoredPosition3D = new Vector3(0, 0, 0);
        imageRect.localEulerAngles = new Vector3(0, 0, -90);
        imageRect.localScale = new Vector3(1, 1, 1);
        imageRect.sizeDelta = new Vector2(0.11f, 0.17f);
        image.GetComponent<Image>().sprite = DistracImage[sprite];
        sprite++;

    }

    private void Update()
    {
       
    }

    public string[] ShuffleNum(string[] arrNum)
    {
        for(int i =0; i < arrNum.Length; i++)
        {
            int rnd_n = Random.Range(0, arrNum.Length);
            string temp = arrNum[i];
            arrNum[i] = arrNum[rnd_n];
            arrNum[rnd_n]=temp;
        }
        return arrNum;
    }
    
    public void DisableCollision(Transform colObject)
    {

        if (_lastCollision != null && _lastCollision != colObject)
        {
           
            colObject.gameObject.layer = LayerMask.NameToLayer("boxCard");
            _lastCollision = colObject;
   
        }
        if (_lastCollision == null)
        {
            _lastCollision = colObject;
            colObject.gameObject.layer = LayerMask.NameToLayer("boxCard");
        }
        else
        {
            return;
        }

    }

    public void getGrabbable()
    {
        
        PrevGrabbable = numGrabber.HeldGrabbable;
        if(PrevGrabbable.tag == "Necessary")
        {
            DataCheck.GrabbedCount += 1;
            GrabTimer.NcsryB = true;
            
    

        }if(PrevGrabbable.tag == "Unnecessary")
        {
            DataCheck.UnGrabbedCount += 1;
            GrabTimer.UncsryB = false;
        }
        
    }

    public void EnableCollision()
    {
      
        if(PrevGrabbable && PrevGrabbable.CompareTag( "Necessary") )
        {
            PrevGrabbable.gameObject.layer = LayerMask.NameToLayer("numCard");

            return;
        }
        if(hitCollision && prevhitCollision != hitCollision)
        {
            Debug.Log("enter");
            hitCollision.layer = LayerMask.NameToLayer("numCard");
            prevhitCollision = hitCollision;
            hitCollision = null;
            return;
        }
       

    }
    

    public void CompareArr()
    {
        Debug.Log("inside");
        
       for(int i =0;i<arrOrder.Length;i++)
        {
            if(arrOrder[i]==null)
            {
                return;
            }

        }
       
            fade = true;
            StartCoroutine(FadeInOut(fade, finCanvas));

            RighthandPointer.SetActive(true);
            answerText.text = "이렇게 마무리 할게요!";
        
      
    }

    public void ResetCoroutine()
    {
        StopAllCoroutines();
    }    
    public void CoroutineWrapper(string name)
    {
        string corname = name;
        StartCoroutine(corname);

    }

    public IEnumerator FinishAnswer()
    {
        DataCheck.ShowDebug();
        GrabTimer.ShowDebug();
        yield return new WaitForSeconds(1.5f);
        
        for(int i = 0; i < arrOrder.Length; i++)
        {
            if(!arrOrder[i].Equals(arrAnswer[i]))
            {
                Debug.Log("not equal");
                answerText.text = "정답이 맞나요? \n다시 한번 확인해보세요!";
                yield return new WaitForSeconds(3.0f);
                fade = false;
                yield return StartCoroutine(FadeInOut(fade, finCanvas));

                ResetCards();
                yield break;
            }
        }
        answerText.text = "고생했어요!\n다음으로 넘어갑니다";


        
        RighthandPointer.SetActive(false);

        ResetCoroutine();

    }

    public void ResetCards()
    {
        for (int i = 0; i < arrOrder.Length; i++)
        {
            if (!arrOrder[i].Equals(arrAnswer[i]))
            {
                arrGb[i].GetComponent<SwapPossible>().CardReset();
            }
        }
    }
    public IEnumerator AgainAnswer()
    {
        yield return new WaitForSeconds(0.9f);
        fade = false;
        yield return StartCoroutine(FadeInOut(fade, finCanvas));
    }

    public IEnumerator OpeningCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        accumTime = 0f;

        fade = false;
        yield return StartCoroutine(FadeInOut(fade,startCanvas));
        startCanvas.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.8f);
        RighthandPointer.SetActive(false);


        for (int i =0;i <4;i++)
        {
            Triggers.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            Triggers.SetActive(false);
            yield return new WaitForSeconds(0.5f);

            
        }

        Triggers.SetActive(true);
    }

 
    

    private IEnumerator FadeInOut(bool fade,Canvas canvas) // true = fade in / false = fade out
    {
        if(fade)//fade in
        {
          //  canvas.gameObject.SetActive(true);
            accumTime = 0f;

            while (accumTime < fadeTime)
            {
                canvas.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, accumTime / fadeTime);
                yield return 0;
                accumTime += Time.deltaTime;
            }
            canvas.GetComponent<CanvasGroup>().alpha = 1f;

        }
        if(!fade)//fade out
        {
            while (accumTime < fadeTime)
            {
                canvas.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, accumTime / fadeTime);
                yield return 0;
                accumTime += Time.deltaTime;
            }
            canvas.GetComponent<CanvasGroup>().alpha = 0f;
           // canvas.gameObject.SetActive(false);

        }

    }
}
