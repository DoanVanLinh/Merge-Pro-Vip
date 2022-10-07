using System.Collections.Generic;
using UnityEngine;

namespace Tarodev_Pathfinding._Scripts.Grid.Scriptables
{
    [CreateAssetMenu(fileName = "New Scriptable Square Grid")]
    public class ScriptableSquareGrid : ScriptableGrid
    {
        [SerializeField, Range(3, 50)] private int _gridWidth = 16;
        [SerializeField, Range(3, 50)] private int _gridHeight = 9;

        public override Dictionary<Vector2, NodeBase> GenerateGrid()
        {
            var tiles = new Dictionary<Vector2, NodeBase>();
            var grid = new GameObject
            {
                name = "Grid"
            };
            for (int x = 0; x < _gridWidth; x++)
            {
                for (int y = 0; y < _gridHeight; y++)
                {

                    if (GridManager.Instance.size < 1)
                    {
                        int splitx = (int)(1 / GridManager.Instance.size);
                        int splity = (int)(1 / GridManager.Instance.size);

                        splitx = x == _gridWidth - 1 ? splitx - 1 : splitx;
                        splity = y == _gridHeight - 1 ? splity - 1 : splity;


                        for (int i = 0; i < splitx; i++)
                        {
                            for (int j = 0; j < splity; j++)
                            {
                                //float d = j % 2 == 1 ? GridManager.Instance.size / 2f : 0;
                                float d = 0;
                                var tile = Instantiate(nodeBasePrefab, grid.transform);
                                tile.Init(DecideIfObstacle(), new SquareCoords { Pos = new Vector3(x - d + i * GridManager.Instance.size, y + j * GridManager.Instance.size) });
                                tiles.Add(new Vector2(x - d + i * GridManager.Instance.size, y + j * GridManager.Instance.size), tile);
                            }
                        }
                    }
                    else
                    {
                        var tile = Instantiate(nodeBasePrefab, grid.transform);
                        tile.Init(DecideIfObstacle(), new SquareCoords { Pos = new Vector3(x, y) });
                        tiles.Add(new Vector2(x, y), tile);
                    }
                }
            }

            return tiles;
        }
    }
}
