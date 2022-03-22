using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryScript : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField] 
    private DictionaryScriptableObject dictionaryData;

    [SerializeField]
    private List<string> keys = new List<string>();
    [SerializeField]
    private List<float> values = new List<float>();

    public Dictionary<string, float> TxtDictionary = new Dictionary<string, float>();

    public bool modifyValues;
    
    public void OnBeforeSerialize()
    {
        if (modifyValues) return;
        keys.Clear();
        values.Clear();
    
        for (var i = 0; i != Math.Min(dictionaryData.Keys.Count, dictionaryData.Values.Count); i++)
        {
            keys.Add((dictionaryData.Keys[i]));
            values.Add(dictionaryData.Values[i]);
        }
    }

    public void OnAfterDeserialize()
    {
        
    }
    
    public void DeserializeDictionary()
    {
        Debug.Log("Deserialization");
        TxtDictionary = new Dictionary<string, float>();
        dictionaryData.Keys.Clear();
        dictionaryData.Values.Clear();

        for (var i = 0; i < Math.Min(keys.Count, values.Count); i++)
        {
            dictionaryData.Keys.Add(keys[i]);
            dictionaryData.Values.Add(values[i]);
            TxtDictionary.Add(keys[i], values[i]);
        }

        modifyValues = false;
    }
}
