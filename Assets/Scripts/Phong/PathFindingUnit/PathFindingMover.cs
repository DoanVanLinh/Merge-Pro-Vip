using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using WE.Unit.Move;
using WE.Unit;
using System.Collections;

public class PathFindingMover : BaseMover
{
    public bool canMove;
    public Vector2 nextStep = new Vector2(-20, -20);
    public NodeBase cacheTarget;
    public NodeBase targetNode;
    public NodeBase targetNeighbor;

    public bool isWait;
    public bool isJump;
    public AnimationCurve moveCurve;
    Vector2 jumpLoc;
    Vector2 cacheLoc;
    private void Start()
    {
        jumpLoc = transform.position;
    }
    public override void MoveToAttackPosition()
    {
        StartCoroutine(IEMoving());
    }
    private IEnumerator IEMoving()
    {
        while (true)
        {
            yield return null;
            if (Owner.Target == null || isWait)
            {
                while (Owner.Target == null || isWait)
                {
                    if (GameManager.Instance.GetFightStatus() == FightStatus.Null)
                    {
                        Owner.targeter.GetTarget();
                        isWait = Owner.Target == null;
                        canMove = Owner.Target != null;
                    }
                    else
                        yield break;
                }
            }
            else
            {
                canMove = true;
                isWait = false;
            }

            if (!Moving())
            {
                if (Owner.Target != null)
                {
                    cacheLoc = transform.position;
                    float distanceToTaret = Vector2.Distance(transform.position, Owner.Target.transform.position);

                    if (distanceToTaret <= Owner.CurrentAttackRange)
                        canMove = false;
                }
                if (!canMove || isWait)
                {
                    GridManager.Instance.UpdateGridNode(cacheLoc, false);
                    if (nextStep != cacheLoc && nextStep != new Vector2(-20, -20))
                        GridManager.Instance.UpdateGridNode(nextStep, true);

                    nextStep = cacheLoc;
                }
                base.MoveToAttackPosition();
                yield break;
            }
        }
    }
    bool LastStep()
    {
        if (nextStep != new Vector2(-20, -20) && nextStep != (Vector2)transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextStep, Time.deltaTime * moveSpeed);
            return false;
        }

        return true;
    }
    public bool Moving()
    {
        if (isWait)
        {
            while ( isWait)
            {
                if (GameManager.Instance.GetFightStatus() == FightStatus.Null)
                {
                    Owner.targeter.GetTarget();
                    isWait = Owner.Target == null;
                    canMove = Owner.Target != null;
                }
                else
                    break;
            }
        }

        if (GridManager.Instance.size < 1)
            targetNode = GridManager.Instance.Tiles[new Vector2((float)Math.Round(Owner.Target.transform.position.x * 2, MidpointRounding.AwayFromZero) / 2, (float)Math.Round(Owner.Target.transform.position.y * 2, MidpointRounding.AwayFromZero) / 2)];
        else
            targetNode = GridManager.Instance.Tiles[new Vector2Int((int)Owner.Target.transform.position.x, (int)Owner.Target.transform.position.y)];

        if (!canMove)
            return false;

        if (nextStep.x == -20)
            SetNextStep(NextLoc());

        if (Vector2.Distance(transform.position, nextStep) != 0)
        {
            if (!isJump)
                transform.position = Vector3.MoveTowards(transform.position, nextStep, Time.deltaTime * moveSpeed);
            else
            {
                jumpLoc = Vector2.MoveTowards(jumpLoc, nextStep, Time.deltaTime * moveSpeed);
                if (nextStep.x != jumpLoc.x)
                    transform.position = new Vector2(jumpLoc.x, moveCurve.Evaluate(jumpLoc.x) + jumpLoc.y);
                else
                    transform.position = new Vector2(jumpLoc.x, moveCurve.Evaluate(jumpLoc.y) + jumpLoc.y);
            }
            return true;
        }
        else
        {
            SetNextStep(NextLoc());
            return false;
        }
    }
    public Vector2 NextLoc()
    {
        if (!canMove)
            return transform.position;

        if (targetNode == null)
            return transform.position;

        if (targetNode != cacheTarget)
            if (targetNode.Neighbors.Any(n => n.Walkable))
            {
                targetNeighbor = targetNode.Neighbors.Where(n => n.Walkable).OrderBy(r => Random.Range(-1f, 1f)).FirstOrDefault();
                cacheTarget = targetNode;
            }

        if (targetNeighbor == null)
            if (targetNode.Neighbors.Any(n => n.Walkable))
                targetNeighbor = targetNode.Neighbors.Where(n => n.Walkable).OrderBy(r => Random.Range(-1f, 1f)).FirstOrDefault();
            else
            {
                return transform.position;
            }

        if (!targetNeighbor.Walkable)
        {
            if (targetNeighbor.transform.position != transform.position)
            {

                if (targetNode.Neighbors.Any(n => n.Walkable))
                    targetNeighbor = targetNode.Neighbors.Where(n => n.Walkable).OrderBy(r => Random.Range(-1f, 1f)).FirstOrDefault();
                else
                {
                    canMove = false;
                    isWait = true;
                    return transform.position;
                }
            }
            else
            {
                if (targetNode.Neighbors.Contains(targetNeighbor))
                    return transform.position;
                else
                {
                    if (targetNode.Neighbors.Any(n => n.Walkable))
                        targetNeighbor = targetNode.Neighbors.Where(n => n.Walkable).OrderBy(r => Random.Range(-1f, 1f)).FirstOrDefault();
                }
            }
        }

        //if (target.Neighbors.Any(n => n.Walkable))
        //    targetNeighbor = target.Neighbors.Where(n => n.Walkable).OrderBy(r => Random.Range(-1f, 1f)).FirstOrDefault();


        ((PathFindingUnit)Owner).unitNode = GridManager.Instance.Tiles.Where(t => t.Key == new Vector2((int)transform.position.x, (int)transform.position.y)).First().Value;

        var path = Pathfinding.FindPath(((PathFindingUnit)Owner).unitNode, targetNeighbor);
        if (path == null || path.Count == 0)
        {
            canMove = false;
            isWait = true;
            targetNeighbor = targetNode.Neighbors.Where(n => n.Walkable && n != targetNeighbor).OrderBy(r => Random.Range(-1f, 1f)).FirstOrDefault();
            return transform.position;
        }
        else
        {
            canMove = true;
            isWait = false;
            return path.Last().transform.position;
        }
    }
    public bool IsNeighborOfTarget()
    {
        if (targetNode != cacheTarget)
            if (targetNode.Neighbors.Any(n => n.Walkable))
            {
                targetNeighbor = targetNode.Neighbors.Where(n => n.Walkable).OrderBy(r => Random.Range(-1f, 1f)).FirstOrDefault();
                cacheTarget = targetNode;
            }

        return targetNode.Neighbors.Contains(targetNeighbor);
    }
    void SetNextStep(Vector2 nextStep)
    {
        if (nextStep == (Vector2)targetNode.transform.position)
            return;

        if (nextStep != (Vector2)transform.position)
        {
            GridManager.Instance.UpdateGridNode(new Vector2((int)transform.position.x, (int)transform.position.y), true);
            GridManager.Instance.UpdateGridNode(nextStep, false);
        }

        this.nextStep = nextStep;
    }
    private void OnDestroy()
    {
        if (nextStep.x != -20)
            GridManager.Instance.UpdateGridNode(nextStep, true);
    }
}

