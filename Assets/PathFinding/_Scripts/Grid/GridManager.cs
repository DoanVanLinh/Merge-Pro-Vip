using System.Collections.Generic;
using System.Linq;
using Tarodev_Pathfinding._Scripts.Grid.Scriptables;
using Tarodev_Pathfinding._Scripts.Units;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private Unit2 unitPrefab;
    [SerializeField] private ScriptableGrid _scriptableGrid;
    [SerializeField] private bool drawConnections;
    [SerializeField] private NodeBase _goalNodeBase;
    [SerializeField, Range(0, 2)] public float size = 0.5f;


    public Dictionary<Vector2, NodeBase> Tiles { get; private set; }

    private NodeBase _playerNodeBase;
    private Unit2 _spawnedPlayer, _spawnedGoal;

    private void Awake()
    {
        Instance = this;
        Tiles = _scriptableGrid.GenerateGrid();

        foreach (var tile in Tiles.Values) tile.CacheNeighbors();

        SpawnUnits();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            var path = Pathfinding.FindPath(_playerNodeBase, _goalNodeBase);
        }
    }
    void SpawnUnits()
    {
        _playerNodeBase = Tiles.Where(t => t.Value.Walkable).OrderBy(t => Random.value).First().Value;
        _spawnedPlayer = Instantiate(unitPrefab, _playerNodeBase.Coords.Pos, Quaternion.identity);

        _spawnedGoal = Instantiate(unitPrefab, new Vector3(50, 50, 50), Quaternion.identity);
    }

    public NodeBase GetTileAtPosition(Vector2 pos) => Tiles.TryGetValue(pos, out var tile) ? tile : null;

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || !drawConnections) return;
        Gizmos.color = Color.red;
        foreach (var tile in Tiles)
        {
            if (tile.Value.Connection == null) continue;
            Gizmos.DrawLine((Vector3)tile.Key + new Vector3(0, 0, -1), (Vector3)tile.Value.Connection.Coords.Pos + new Vector3(0, 0, -1));
        }

        foreach (var item in Tiles)
        {
            if (item.Value.Walkable)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(item.Key, 0.05f);
            }
            else
            {
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(item.Key, 0.05f);
            }
        }
    }

    public void UpdateGridNode(Vector2 loc,bool isWalkable)
    {
        Tiles[loc].Walkable = isWalkable;
    }
}