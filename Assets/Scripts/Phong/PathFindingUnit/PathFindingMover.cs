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

    public Action OnStepDone;
    private void Start()
    {
        jumpLoc = transform.position;
    }

    public override void MoveToAttackPosition()
    {
        if (nextStep.x == -20)
            GridManager.Instance.UpdateGridNode(transform.position, false);

        if (moveSpeed == 0)
            return;

        canMove = Owner.Target != null;

        
        Owner.skeletonAnimation.SetUnitAni(Helper.MOVE_STATE_ANI, true);


        SetNextStep(NextLoc());
        StartCoroutine(IEMoving());
        //}
    }

    private IEnumerator IEMoving()
    {
        while (true)
        {
            yield return null;


            if (!Moving())
            {
                if (!canMove || isWait)
                {
                    GridManager.Instance.UpdateGridNode(transform.position, false);
                    if (nextStep != (Vector2)transform.position && nextStep != new Vector2(-20, -20))
                        GridManager.Instance.UpdateGridNode(nextStep, true);

                    nextStep = transform.position;
                }

                ((PathFindingUnit)Owner).unitNode = GridManager.Instance.Tiles.Where(t => t.Key == (Vector2)transform.position).First().Value;

                if (Owner.Target != null)
                    if (Owner.IsOnAttackRange())
                    {
                        GridManager.Instance.UpdateGridNode(transform.position, false);
                        if (nextStep != (Vector2)transform.position && nextStep != new Vector2(-20, -20))
                            GridManager.Instance.UpdateGridNode(nextStep, true);

                        nextStep = transform.position;
                        if (Owner.Target != null)
                            OnMoveDone?.Invoke();
                    }
                    else
                        OnStepDone?.Invoke();
                yield break;
            }
        }
    }
    public bool Moving()
    {
        if (!canMove)
            return false;

        if ((!canMove || isWait) && Owner.IsOnAttackRange())
        {
            return false;
        }

        //if (nextStep.x == -20)
        //    SetNextStep(NextLoc());

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


            return false;
        }
    }
    public Vector2 NextLoc()
    {
        if (GridManager.Instance.size < 1)
            targetNode = GridManager.Instance.Tiles[new Vector2((float)Math.Round(Owner.Target.transform.position.x * 2, MidpointRounding.AwayFromZero) / 2, (float)Math.Round(Owner.Target.transform.position.y * 2, MidpointRounding.AwayFromZero) / 2)];
        else
            targetNode = GridManager.Instance.Tiles[new Vector2Int((int)Owner.Target.transform.position.x, (int)Owner.Target.transform.position.y)];

        if (!canMove)
        {
            isWait = true;
            return transform.position;
        }

        if (targetNode == null)
        {
            isWait = true;
            return transform.position;
        }

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
                isWait = true;
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
                    if (!targetNode.Neighbors.Contains(targetNeighbor))
                    {
                        canMove = false;
                        isWait = true;
                        return transform.position;
                    }
                }
            }
            else
            {
                if (targetNode.Neighbors.Contains(targetNeighbor))
                {
                    isWait = true;
                    return transform.position;
                }
                else
                {
                    if (targetNode.Neighbors.Any(n => n.Walkable))
                        targetNeighbor = targetNode.Neighbors.Where(n => n.Walkable).OrderBy(r => Random.Range(-1f, 1f)).FirstOrDefault();
                }
            }
        }

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
        {
            this.nextStep = transform.position;
            return;
        }

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

