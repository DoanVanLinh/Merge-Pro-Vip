using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class Test1 : MonoBehaviour
{
    public string debug = "call Test 1";
    public System.Action actionTest1;
    [Button("Call Action")]
    public void CallAction()
    {
        actionTest1?.Invoke();
    }
}
