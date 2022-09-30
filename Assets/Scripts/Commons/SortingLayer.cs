using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SortingLayer : MonoBehaviour
{
    public MeshRenderer meshRender;
    public void Sorting()
    {
        meshRender.sortingOrder = -(int)(transform.position.y * 10);
    }
}
