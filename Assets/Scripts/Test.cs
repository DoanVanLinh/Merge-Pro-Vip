#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Test : MonoBehaviour
{
    public int coins;

    void Update()
    {
        Debug.Log(Helper.ConvertCoins(coins));
    }
}
#endif