#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System;
using Sirenix.OdinInspector;

public class Test : MonoBehaviour
{
    public Test2 target;

    [Button("Excute")]
    public void Excute()
    {
        target.Stop2();
    }

}
#endif