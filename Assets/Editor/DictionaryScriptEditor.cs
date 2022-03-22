using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEditor;

[CustomEditor((typeof(DictionaryScript)))]
public class DictionaryScriptEditor : Editor
{
  public override void OnInspectorGUI()
  {
      base.OnInspectorGUI();

      if (GUILayout.Button("Save changes"))
      {
          ((DictionaryScript)target).DeserializeDictionary();
      }
  }
    
}
