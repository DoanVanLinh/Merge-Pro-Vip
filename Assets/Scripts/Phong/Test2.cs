using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class Test2 : MonoBehaviour
{
    public Test1 test1;
    public float time = 0;
    [Button("Add Listener")]
    public void AddListener()
    {
        test1.actionTest1 += Debuging;
    }
    [Button("Remove Listener")]
    public void RemoveListener()
    {
        test1.actionTest1 -= Debuging;
    }
    void Debuging()
    {
        //test1.actionTest1 -= Debuging;
        Debug.Log(transform.position);
    }

    
    //{
    //    StartCoroutine(IETest());
    //}
    //IEnumerator IETest()
    //{
    //    //yield return null;
    //    while (true)
    //    {
    //        if (Test())
    //        {
    //            Debug.Log("To Attack");
    //            yield break;
    //        }
    //    }
    //}
    //bool Test()
    //{
    //    time += Time.deltaTime;
    //    if (time >= 1)
    //        return true;
    //    Debug.Log("Moving");
    //    return false;
    //}
}
