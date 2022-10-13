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
            base.Init();
        }
        protected override void OnDisable()
        {
            base.OnDisable();

            //if (unitNode != null)
            //    FieldManager.RemoveFromField(unitNode.transform.position, this);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (unitNode != null)
                FieldManager.RemoveFromField(unitNode.transform.position, this);
        }
    }
}