using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMove : MonoBehaviour
{
    public int row;
    public int col;
    [Range(0.01f,1f)]
    public float gridSize = 1;


    public static bool[,] gridMove;
    private void Awake()
    {
        gridMove = new bool[(int)(col / gridSize), (int)(row / gridSize)];

        for (int i = 0; i < row / gridSize; i++)
        {
            for (int j = 0; j < col / gridSize; j++)
            {
                gridMove[j, i] = false;
            }
        }

    }
    public static bool IsStand(int x, int y)
    {
        return gridMove[x, y];
    }
    public static void SetGrid(Vector2 loc, bool isStand)
    {
        gridMove[(int)loc.x, (int)loc.y] = isStand;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < (row / gridSize) - (gridSize < 1 ? 1 : 0); i++)
        {
            Gizmos.DrawLine(new Vector2(0, i * gridSize), new Vector2(col - 1, i * gridSize));
        }
        for (int j = 0; j < (col / gridSize) - (gridSize < 1 ? 1 : 0); j++)
        {
            Gizmos.DrawLine(new Vector2(j * gridSize, 0), new Vector2(j * gridSize, row - 1));
        }
    }
#endif
}
