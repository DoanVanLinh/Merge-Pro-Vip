using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public static class Test2
{
    public static System.Action onDie;

  
    [Button("Remove Listener")]
    public static void RemoveListener()
    {
    }

    [Button("Excute")]
    public static void Excute()
    {
        onDie?.Invoke();
    }
}
