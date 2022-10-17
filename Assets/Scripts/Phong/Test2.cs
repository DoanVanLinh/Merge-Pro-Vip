using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class Test2 : Test1
{
    private void Start()
    {
        StartCoroutine(IETest1());
    }
    public void Stop2()
    {
        Debug.Log("Stop1");

        base.Stop();
        Debug.Log("Stop2");
    }
    IEnumerator IETest1()
    {
        while (true)
        {
            yield return null;
            Debug.Log("Test 2");
        }

    }
}
