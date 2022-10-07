using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Helper
{
    public const string PLAYER_UNIT_TAG = "Player Unit";
    public const string ENEMY_UNIT_TAG = "Enemy Unit";
    public const string UNIT_BULLET_TAG = "Unit Bullet";
    public const string UNIT_EXTRA_TAG = "Wheel Element";
    public const int PLAYER_TEAM_LAYER = 6;
    public const int ENEMY_TEAM_LAYER = 7;

    //Skeleton State
    public const string IDLE_STATE_ANI = "Idle";
    public const string MOVE_STATE_ANI = "Move";
    public const string ATTACK_STATE_ANI = "Attack";
    public const string DEAD_STATE_ANI = "Dead";

    //Sound Name
    public const string SOUND_BUTTON_CLICK = "Button Click";

    public static Camera mainCam = Camera.main;

    public const string OPEN_LINK_RATE = "market://details?id=" + "com.drag.basket.ball.battle";

    public static string EnumToString<T>(T enumT)
    {
        return enumT.ToString();
    }
    public static T StringToEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value);
    }
    public static bool IsOverUI(int layer = 5)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Where(r => r.gameObject.layer == layer).ToList().Count > 0;
    }
    public static string ConvertCoins(int value)
    {
        int integeral = value;
        int diveTime = 0;

        while (integeral != 0)
        {
            diveTime++;
            integeral /= 1000;
        }

        switch (diveTime)
        {
            case 0:
            case 1:
                return value.ToString();
            case 2:
                return value/1000 + " K";
            case 3:
                return value / 1000000 + " M";
            default:
                return value / 1000000000 + " B";
        }
    }
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

