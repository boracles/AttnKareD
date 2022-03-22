using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HutongGames.PlayMaker;
using UserData;

class SizeOfData
{
    public int[,] userDataArr2;

    public SizeOfData(int x, int y)
    {
        this.userDataArr2 = new int[x, y];
    }
}

public class SetPlayerData : MonoBehaviour
{
    public GameDataManager gameDataManager;      
    public PlayMakerFSM fsm;

    [SerializeField] private int sceneFirstKey;
    [SerializeField] private int currentScene;
    [SerializeField] private int row;

    private int _key;
    private int _eachLastKey;
    private int _eachFirstKey;
    private int _keyLength;

    // 데이터의 인덱싱이 바뀔때 값을 바꿔야 하는 것들 -> TotalFirstKey에 대입값, sOd의 인자 2가지 값
    private const int TotalFirstKey = 101;
    private SizeOfData _sOd = new SizeOfData(7, 13);

    private int Row { get => row; set => row = value; }

    private int CurrentScene { get => currentScene; set => currentScene = value; }

    private int SceneFirstKey { get => sceneFirstKey; set => sceneFirstKey = value; }

    private void SetFirstKeyInScenes(int scene)
    {
       // 씬의 인덱싱이 바뀌면 스위치문의 대입값도 바뀌어야 한다.
        switch (scene)
        {
            case 1: //door lock
                Row = 0;
                SceneFirstKey = 101;
                CurrentScene = scene;                
                break;
            case 2: //schedule
                Row = 1;
                SceneFirstKey = 201;
                CurrentScene = scene;
                break;
            case 3: //BagPacking L
                Row = 4;
                SceneFirstKey = 501;
                CurrentScene = scene;
                break;
            case 4: //Scoop L
                Row = 5;
                SceneFirstKey = 601;
                CurrentScene = scene;
                break;
            case 5: //CRUM
                Row = 2;
                SceneFirstKey = 301;
                CurrentScene = scene;
                break;
            case 6: //PlayPaddle
                Row = 3;
                SceneFirstKey = 401;
                CurrentScene = scene;
                break;
            case 7: //BagPacking H
                Row = 4;
                SceneFirstKey = 501;
                CurrentScene = scene;
                break;
            case 8: //Scoop H
                Row = 5;
                SceneFirstKey = 601;
                CurrentScene = scene;
                break;
            case 9:
                Row = 6;
                SceneFirstKey = 701;
                CurrentScene = scene;
                break;
        }     
    }

    // 진단 Scene의 개수가 바뀌면 아래의 함수 내용을 변경해야 한다.
    private int SetFirstKey(int currentKey)
    {
        if (currentKey <= 199) _eachFirstKey = 101;
        else if (currentKey <= 299) _eachFirstKey = 201;
        else if (currentKey <= 399) _eachFirstKey = 301;
        else if (currentKey <= 499) _eachFirstKey = 401;
        else if (currentKey <= 599) _eachFirstKey = 501;
        else if (currentKey <= 699) _eachFirstKey = 601;
        else if (currentKey <= 799) _eachFirstKey = 701;
        return _eachFirstKey;
    }

    // 각 씬에서 추출하는 데이터의 수가 바뀌면 아래의 eachLastKey 변수를 해당 개수에 맞게 변경해야 한다.
    private int SetLastKey(int currentKey )
    {
        if (currentKey <= 199) _eachLastKey = 107;
        else if (currentKey <= 299) _eachLastKey = 213;
        else if (currentKey <= 399) _eachLastKey = 308;
        else if (currentKey <= 499) _eachLastKey = 413;
        else if (currentKey <= 599) _eachLastKey = 510;
        else if (currentKey <= 699) _eachLastKey = 611;
        else if (currentKey <= 799) _eachLastKey = 704;
        return _eachLastKey;
    }
       
    // 2차원 배열에 모든 키를 셋팅하는 함수
    private int[,] SetEachFirstKey(int[,] arr2)
    {
        var key = TotalFirstKey;                

        for (var i = 0; i < _sOd.userDataArr2.GetLength(0); i++)
        {            
            SetLastKey(key);

            for(var j = 0; j < _sOd.userDataArr2.GetLength(1); j++)
            {
                _sOd.userDataArr2[i, j] = key;
                Debug.Log(_sOd.userDataArr2[i, j]);
                if (key == _eachLastKey)
                {
                    key -= j;
                    key += 100;
                    break;
                }
                key++;
            }            
        }
        return _sOd.userDataArr2;
    }

    private int GetKeyLength(int eachLastKey)
    {
        var tempS = eachLastKey.ToString();
        tempS =  tempS.Remove(0, 1);
        _keyLength = int.Parse(tempS);
        return _keyLength;
    }

    public void ClearDataSetting()
    {
        DataManager.GetInstance().dataList.Clear();
    }    

    // 프로그램 시작 후 임의의 진단 Scene이 로드되면 단 한번만 아래의 함수대로 초기화된다
    public void InitialDataSetting()
    {       
        // 모든 데이터 초기값 -1
        float result = -1;
                
        _key = TotalFirstKey;

        for (var i = 1; i <= _sOd.userDataArr2.GetLength(0); i++)
        {                           
            SetLastKey(_key);
            while (_key <= _eachLastKey)
            {
                //Debug.Log(key);
                DataManager.GetInstance().dataList.Add(_key, new PlayerData("data_" + _key, result));
                _key++;
            }
            _key += 100;

            SetFirstKey(_key);
            _key = _eachFirstKey;
        }               
    }
                            
    // PlayMaker로 값을 전달하는 Scene은 아래의 함수로 오버로드 되어 사용된다
    public void SetSceneData()
    {              
        CurrentScene = SceneManager.GetActiveScene().buildIndex;                        
        SetFirstKeyInScenes(CurrentScene);
        SetEachFirstKey(_sOd.userDataArr2);

        // value check test
        {
            Debug.Log("sceneIndex = " + CurrentScene.ToString());
            Debug.Log("sceneFirstKey = " + SceneFirstKey.ToString());
            Debug.Log("row = " + Row.ToString());
        }

        SetLastKey(SceneFirstKey);
        GetKeyLength(_eachLastKey);
        Debug.Log("keyLength = " + _keyLength);

        //mapName 선언
        var mapName = new Dictionary<string, FsmFloat>();
        
        for (var i = 0; i < _keyLength; i++)
        {
            var arg0 = _sOd.userDataArr2[Row, i];
            mapName.Add(key: $"data_{arg0}", 
                value: fsm.FsmVariables.GetFsmFloat("data_" + arg0));

            Debug.Log("data_" + arg0 + " = " + mapName[string.Format("data_" + arg0)]);

            DataManager.GetInstance().dataList[arg0].Result = mapName["data_" + arg0].Value;            
        }        
    }

    // C# Script로 값을 전달하는 Scene은 아래의 함수로 오버로드 되어 사용된다
    public void SetSceneData(params float[] myVal)
    {
        CurrentScene = SceneManager.GetActiveScene().buildIndex;
        SetFirstKeyInScenes(CurrentScene);
        SetEachFirstKey(_sOd.userDataArr2);

        // value check test
        {
            Debug.Log("sceneIndex = " + CurrentScene.ToString());
            Debug.Log("sceneFirstKey = " + SceneFirstKey.ToString());
            Debug.Log("row = " + Row.ToString());
        }
        
        SetLastKey(SceneFirstKey);
        GetKeyLength(_eachLastKey);
        Debug.Log("keyLength = " + _keyLength);

        var mapName = new Dictionary<string, float>();

        for (var i = 0; i < _keyLength; i++)
        {
            var arg0 = _sOd.userDataArr2[Row, i];
            mapName.Add(key: $"data_{arg0}", value: myVal[i]);

            Debug.Log("data_" + arg0 + " = " + mapName[string.Format("data_" + arg0)]);

            DataManager.GetInstance().dataList[arg0].Result = mapName["data_" + arg0];
        }        
    }    
}

