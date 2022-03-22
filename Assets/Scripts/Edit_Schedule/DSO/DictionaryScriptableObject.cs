using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dictionary Storage", menuName = "Data Objects/Dictionary Storage Object")]
public class DictionaryScriptableObject : ScriptableObject
{
    [SerializeField]
    private List<string> keys = new List<string>();
    [SerializeField] 
    private List<float> values = new List<float>();

    public List<string> Keys
    {
        get => keys;
        set => keys = value;
    }

    public List<float> Values
    {
        get => values;
        set => values = value;
    }
}
