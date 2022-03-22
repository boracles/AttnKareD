using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RemoveWord
{    
    public static bool EndsWithWord(string s, string keyword)
    {
        return s.EndsWith(keyword);        
    }

    public static bool StartsWithWord(string s, string keyword)
    {
        return s.StartsWith(keyword);
    }
}
