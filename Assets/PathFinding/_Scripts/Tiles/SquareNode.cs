using System.Collections.Generic;
using System.Linq;
using Tarodev_Pathfinding._Scripts.Grid;
using UnityEngine;

public class SquareNode : NodeBase
{
    private static List<Vector2> Dirs;
    private void Awake()
    {
        Dirs = new List<Vector2>() {
            new Vector2(0, 1*GridManager.Instance.size), new Vector2(-1*GridManager.Instance.size, 0), new Vector2(0, -1*GridManager.Instance.size), new Vector2(1*GridManager.Instance.size, 0),
            new Vector2(1*GridManager.Instance.size, 1*GridManager.Instance.size), new Vector2(1*GridManager.Instance.size, -1*GridManager.Instance.size), new Vector2(-1*GridManager.Instance.size, -1*GridManager.Instance.size), new Vector2(-1*GridManager.Instance.size, 1*GridManager.Instance.size)
        };
    }
    public override void CacheNeighbors()
    {
        Neighbors = new List<NodeBase>();

        foreach (var tile in Dirs.Select(dir => GridManager.Instance.GetTileAtPosition(Coords.Pos + dir)).Where(tile => tile != null))
        {
            Neighbors.Add(tile);
        }
    }

    public override void Init(bool walkable, SquareCoords coords)
    {
        base.Init(walkable, coords);
    }
}



