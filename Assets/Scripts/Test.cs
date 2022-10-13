#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System;
using Sirenix.OdinInspector;

public class Test : MonoBehaviour
{
    public Test1 target;

    [Button("Excute")]
    public void Excute()
    {
        Test2.Excute();
    }

    private void Start()
    {
        //target.OnDie += OnDie;
        StartCoroutine(IEMove("Moving 1"));
    }

    private void OnDie()
    {
        //target.OnDie -= OnDie;
        //StopAllCoroutines();
        //StartCoroutine(IEMove("Moving 2"));
    }

    IEnumerator IEMove(string text)
    {
        //yield return null;
        while (true)
        {
            yield return null;
            while (true)
            {
                yield return null;
                if (Input.GetKeyDown(KeyCode.P))
                    break;
            }
            Debug.Log(Time.deltaTime);
            //if (TestMove(text))
            //{
            //    StartCoroutine(IEAttack(text + " Attack 1"));
            //    yield break;
            //}
        }
    }

    IEnumerator IEAttack(string text)
    {
        //yield return null;
        while (true)
        {
            yield return null;
            if (target != null)
            {
                Debug.Log(text);
            }
            else
                yield break;
        }
    }
    bool TestMove(string text)
    {
        if (Input.GetKeyDown(KeyCode.M))
            return true;
        Debug.Log(text);
        return false;
    }
}
#endif