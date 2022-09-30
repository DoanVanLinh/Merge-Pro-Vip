#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            CPlayerPrefs.SetBool("Ancher", true);
        if (Input.GetKeyDown(KeyCode.S))
            CPlayerPrefs.SetBool("Aquaman", true);
    }
}
#endif