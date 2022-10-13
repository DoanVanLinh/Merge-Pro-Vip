using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class Test1 : MonoBehaviour
{
    //public System.Action OnDie;

    //private void OnDestroy()
    //{
    //    OnDie?.Invoke();
    //}
    private void Start()
    {
        Test2.onDie += Ondie;
    }
    [Button("Add Listener")]
    public void AddListener()
    {
        Test2.onDie += Ondie;
    }

    private void Ondie()
    {
        Debug.Log(gameObject.name);
    }
}
