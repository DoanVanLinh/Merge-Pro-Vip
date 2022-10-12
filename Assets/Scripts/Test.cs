#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Test : MonoBehaviour
{
    public AnimationCurve moveCurve;

    public GameObject unit;
    public GameObject unitTarget;
    public GameObject unit2;
    public float speed;

    Vector2 jumpLoc;

    private void Start()
    {
        jumpLoc = unit2.transform.position;
    }
    private void Update()
    {
        //jumpLoc = Vector2.MoveTowards(jumpLoc, unitTarget.transform.position, Time.deltaTime * speed);
        //if(unitTarget.transform.position.x !=jumpLoc.x)
        //    unit2.transform.position = new Vector2(jumpLoc.x, moveCurve.Evaluate(jumpLoc.x)+ jumpLoc.y);
        //else
        //    unit2.transform.position = new Vector2(jumpLoc.x, moveCurve.Evaluate(jumpLoc.y) + jumpLoc.y);
    }
}
#endif