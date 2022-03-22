using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveScoopData : MonoBehaviour
{
    FileStream DataInfo;
    StreamWriter DataWriter;
    string FilePath = Application.streamingAssetsPath + "/Hippo/";
    string SaveTime = DateTime.Now.ToString("yyyyMMddHHmmss");

    string SavePath;

    void Start()
    {
        SavePath = FilePath + /*SceneManager.GetActiveScene().name*/"Scoop" + "_" + SaveTime + "_DATA" + ".txt";
    }

    public void SaveTempSceneData(string myData)
    {
        DataInfo = new FileStream(SavePath, FileMode.Append, FileAccess.Write);
        DataWriter = new StreamWriter(DataInfo, System.Text.Encoding.Unicode);
        DataWriter.WriteLine(myData);
        DataWriter.Close();
    }
}
