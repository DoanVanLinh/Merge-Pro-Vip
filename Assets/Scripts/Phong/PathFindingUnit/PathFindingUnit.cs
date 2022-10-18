using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace WE.Unit
{
    public class PathFindingUnit : BaseUnit
    {
        public NodeBase unitNode;

        public override void Init()
        {
            FieldManager.AddToField(transform.position, this);
            unitNode = GridManager.Instance.Tiles.Where(t => t.Key == (Vector2)transform.position).First().Value;
            GridManager.Instance.UpdateGridNode(transform.position, false);

            base.Init();
        }
        public override void Die()
        {
            if (unitNode != null)
            {
                FieldManager.RemoveFromField(unitNode.transform.position, this);
                GridManager.Instance.UpdateGridNode(unitNode.transform.position, true);
            }
            base.Die();
        }
    }
}