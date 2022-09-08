using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color selectedColor;

    private SpriteRenderer tileRender;
    private Color defaultColor;

    private void Start()
    {
        tileRender = GetComponent<SpriteRenderer>();
        defaultColor = tileRender.color;
    }

    private void OnMouseEnter()
    {
        tileRender.color = selectedColor;
    }

    private void OnMouseExit()
    {
        tileRender.color = defaultColor;
    }

}

