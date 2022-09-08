using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Test : MonoBehaviour
{
    private SkeletonAnimation ani;
    void Start()
    {
        ani = GetComponent<SkeletonAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetAni("Idle", true);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetAni("Attack", true);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetAni("Move", true);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            SetAni("Dead", false);
    }
    void SetAni(string name, bool loop, float timeScale = 1f)
    {
        ani.state.SetAnimation(0,name,loop).TimeScale = timeScale;
    }
}
