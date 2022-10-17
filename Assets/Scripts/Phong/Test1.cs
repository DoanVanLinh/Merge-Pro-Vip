using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class Test1 : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(IETest1());
    }
    public virtual void Stop()
    {
        StopAllCoroutines();
    }
    IEnumerator IETest1()
    {
        while (true)
        {
            yield return null;
            Debug.Log("Test 1");
        }
    }
}
