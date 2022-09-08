using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    #region Singleton
    public static Field Instance { get; set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    [SerializeField] private int row;
    [SerializeField] private int col;
    [SerializeField] private GameObject tile;


    private void Start()
    {
        row = GameManager.Instance.row;
        col = GameManager.Instance.col;
        SpawnField();
    }

    private void SpawnField()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                GameObject tileClone = Instantiate(tile, new Vector2(j, i), Quaternion.identity, transform);
                tileClone.name = $"Tile {i} - {j}";
                tileClone.transform.localScale = Vector2.one * GameManager.Instance.gridScale * 0.95f;
            }
        }
    }
}
