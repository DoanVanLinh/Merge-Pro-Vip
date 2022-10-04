using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
    public abstract class NodeBase : MonoBehaviour {

        public SquareCoords Coords;
        public float GetDistance(NodeBase other) => Coords.GetDistance(other.Coords);
        public bool Walkable { get; set; }

        public virtual void Init(bool walkable, SquareCoords coords) {
            Walkable = walkable;

            Coords = coords;
            transform.position = Coords.Pos;
        }

        #region Pathfinding
        public List<NodeBase> Neighbors { get; protected set; }
        public NodeBase Connection { get; private set; }
        public float G { get; private set; }
        public float H { get; private set; }
        public float F => G + H;

        public abstract void CacheNeighbors();

        public void SetConnection(NodeBase nodeBase) {
            Connection = nodeBase;
        }

        public void SetG(float g) {
            G = g;
        }

        public void SetH(float h) {
            H = h;
        }
        #endregion
    }

public class SquareCoords
{

    public float GetDistance(SquareCoords other)
    {
        var dist = new Vector2Int(Mathf.Abs((int)Pos.x - (int)other.Pos.x), Mathf.Abs((int)Pos.y - (int)other.Pos.y));

        var lowest = Mathf.Min(dist.x, dist.y);
        var highest = Mathf.Max(dist.x, dist.y);

        var horizontalMovesRequired = highest - lowest;

        return lowest * 14 + horizontalMovesRequired * 10;
    }

    public Vector2 Pos { get; set; }
}