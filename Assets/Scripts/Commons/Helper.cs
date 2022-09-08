using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public static class Helper
{
    public const string PLAYER_UNIT_TAG = "Player Unit";
    public const string ENEMY_UNIT_TAG = "Enemy Unit";
    public const string UNIT_BULLET_TAG = "Unit Bullet";
    public const int PLAYER_TEAM_LAYER = 6;
    public const int ENEMY_TEAM_LAYER = 7;

    //Skeleton State
    public const string IDLE_STATE_ANI = "Idle";
    public const string MOVE_STATE_ANI = "Move";
    public const string ATTACK_STATE_ANI = "Attack";
    public const string DEAD_STATE_ANI = "Dead";

    public static float ParseFloat(string data)
    {
#if UNITY_ANDROID
            return float.Parse(data);
#else
        float f = float.Parse(data, CultureInfo.InvariantCulture);
        return f;
#endif
    }

#if UNITY_EDITOR
    public static IEnumerator IELoadData(string urlData, System.Action<string> actionComplete, bool showAlert = false)
    {
        var www = new WWW(urlData);
        float time = 0;
        //TextAsset fileCsvLevel = null;
        while (!www.isDone)
        {
            time += 0.001f;
            if (time > 10000)
            {
                yield return null;
                Debug.Log("Downloading...");
                time = 0;
            }
        }
        if (!string.IsNullOrEmpty(www.error))
        {
            UnityEditor.EditorUtility.DisplayDialog("Notice", "Load CSV Fail", "OK");
            yield break;
        }
        yield return null;
        actionComplete?.Invoke(www.text);
        yield return null;
        UnityEditor.AssetDatabase.SaveAssets();
        if (showAlert)
            UnityEditor.EditorUtility.DisplayDialog("Notice", "Load Data Success", "OK");
        else
            Debug.Log("<color=yellow>Download Data Complete</color>");
    }
#endif
}

