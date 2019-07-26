using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Prefs
{
    public static int HighScore
    {
        get => PlayerPrefs.GetInt("HighScore", 0);
        set => PlayerPrefs.SetInt("HighScore", value);
    }
}
