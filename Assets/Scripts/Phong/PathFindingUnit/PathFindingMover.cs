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
    public Vector2 nextStep = new Vector2(-20, -20);
    public NodeBase cacheTarget;
    public NodeBase targetNode;
    public NodeBase targetNeighbor;

    public bool isJump;
    public bool isStepDone;
    public bool isMoveDone;
    public AnimationCurve moveCurve;
    Vector2 jumpLoc;

    public Action OnStepDone;
    private void Start()
    {
        jumpLoc = transform.position;
        isStepDone = true;
        isMoveDone = false;
    }

    public override void MoveToAttackPosition()
    {
        //if (nextStep.x == -20)
        //    GridManager.Instance.UpdateGridNode(transform.position, false);
        if (!isStepDone)
            return;

        if (moveSpeed == 0)
            return;

        Owner.skeletonAnimation.SetUnitAni(Helper.MOVE_STATE_ANI, true);


        SetNextStep(NextLoc());
        StartCoroutine(IEMoving());
    }

    private IEnumerator IEMoving()
    {
        //int delayCounter = (int)transform.position.y;
        //for (int i = 0; i < delayCounter; i++)
        //{
        //    yield return null;
        //}


        while (true)
        {
            yield return null;


            if (!Moving())
            {
                if (gameObject.name.Equals("GameObject (37)"))
                    Debug.Log(Time.deltaTime);
                ((PathFindingUnit)Owner).unitNode = GridManager.Instance.Tiles.Where(t => t.Key == (Vector2)transform.position).First().Value;

                if (Owner.Target != null)
                    if (Owner.IsOnAttackRange())
                    {
                        nextStep = transform.position;
                        if (Owner.Target != null)
                        {
                            OnMoveDone?.Invoke();
                            isMoveDone = true;
                            Owner.transform.localScale = new Vector3((transform.position - Owner.Target.transform.position).x > 0 ? 1 : -1, 1, 1);
                            Owner.healthBar.gameObject.transform.localScale = Owner.transform.localScale;
                        }
                    }
                    else
                        OnStepDone?.Invoke();
                else
                    OnStepDone?.Invoke();
                yield break;
            }
        }
    }
    public bool Moving()
    {
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
            GridManager.Instance.UpdateGridNode(nextStep, false);
            isStepDone = false;
            isMoveDone = false;
            return true;
        }
        else
        {
            isStepDone = true;
            return false;
        }
    }
    public Vector2 NextLoc()
    {
        if (GridManager.Instance.size < 1)
            targetNode = GridManager.Instance.Tiles[new Vector2((float)Math.Round(Owner.Target.transform.position.x * 2, MidpointRounding.AwayFromZero) / 2, (float)Math.Round(Owner.Target.transform.position.y * 2, MidpointRounding.AwayFromZero) / 2)];
        else
            targetNode = GridManager.Instance.Tiles[new Vector2Int((int)Owner.Target.transform.position.x, (int)Owner.Target.transform.position.y)];

        ((PathFindingUnit)Owner).unitNode = GridManager.Instance.Tiles.Where(t => t.Key == new Vector2((int)transform.position.x, (int)transform.position.y)).First().Value;


        targetNeighbor = targetNode.Neighbors.Where(n => n == ((PathFindingUnit)Owner).unitNode || n.Walkable)
                                             .OrderBy(o => Vector2.Distance(Owner.transform.position, o.transform.position))
                                             .FirstOrDefault();
        if (targetNeighbor == null)
            return transform.position;


        var path = Pathfinding.FindPath(((PathFindingUnit)Owner).unitNode, targetNeighbor);
        if (path == null || path.Count == 0)
        {
            return transform.position;
        }
        else
        {
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

        this.nextStep = nextStep;

        Owner.transform.localScale = new Vector3(((Vector2)transform.position - nextStep).x > 0 ? 1 : -1, 1, 1);

        GridManager.Instance.UpdateGridNode(transform.position, true);
        GridManager.Instance.UpdateGridNode(nextStep, false);

    }
    //private void OnDestroy()
    //{
    //    GridManager.Instance.UpdateGridNode(((PathFindingUnit)Owner).unitNode.transform.position, true);
    //}
}

