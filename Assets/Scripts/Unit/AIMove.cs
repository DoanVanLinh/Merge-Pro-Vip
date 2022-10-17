using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIMove : MonoBehaviour
{
    public bool canMove;
    public float speed;
    public bool isJump;

    public AnimationCurve moveCurve;
    public NodeBase target;
    public NodeBase targetNeighbor;
    public NodeBase cacheTarget;
    public NodeBase unitNode;

    public Vector2 nextStep = new Vector2(-20, -20);

    public bool isWait;

    private Vector2 jumpLoc;
    private void OnEnable()
    {
        isWait = false;
        nextStep = new Vector2(-20, -20);
        unitNode = GridManager.Instance.Tiles.Where(t => t.Key == (Vector2)transform.position).First().Value;
    }
    //private void Update()
    //{
    //    if (!canMove || isWait)
    //    {
    //        GridManager.Instance.UpdateGridNode(transform.position, false);
    //        if (nextStep != (Vector2)transform.position)
    //            GridManager.Instance.UpdateGridNode(nextStep, true);

    //        nextStep = transform.position;
    //    }
    //}
    public bool Moving()
    {
        if (!canMove)
        {
            if (!canMove || isWait)
            {
                GridManager.Instance.UpdateGridNode(transform.position, false);
                if (nextStep != (Vector2)transform.position && nextStep != new Vector2(-20, -20))
                    GridManager.Instance.UpdateGridNode(nextStep, true);

                nextStep = transform.position;
            }
            return false;
        }

        if (nextStep.x == -20)
        {
            jumpLoc = transform.position;
            SetNextStep(NextLoc());
        }

        if (Vector2.Distance(transform.position, nextStep) != 0)
        {
            //transform.position = Vector3.MoveTowards(transform.position, nextStep, Time.deltaTime * speed);
            if (!isJump)
                transform.position = Vector3.MoveTowards(transform.position, nextStep, Time.deltaTime * speed);
            else
            {
                jumpLoc = Vector2.MoveTowards(jumpLoc, nextStep, Time.deltaTime * speed);
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
            if (!canMove || isWait)
            {
                GridManager.Instance.UpdateGridNode(transform.position, false);
                if (nextStep != (Vector2)transform.position && nextStep != new Vector2(-20, -20))
                    GridManager.Instance.UpdateGridNode(nextStep, true);

                nextStep = transform.position;
            }
            return false;
        }
    }
    public Vector2 NextLoc()
    {
        if (!canMove)
        {
            isWait = true;
            return transform.position;
        }

        if (target == null)
        {
            isWait = true;
            return transform.position;
        }

        if (target != cacheTarget)
            if (target.Neighbors.Any(n => n.Walkable))
            {
                targetNeighbor = target.Neighbors.Where(n => n.Walkable).OrderBy(r => Random.Range(-1f, 1f)).FirstOrDefault();
                cacheTarget = target;
            }

        if (targetNeighbor == null)
            if (target.Neighbors.Any(n => n.Walkable))
                targetNeighbor = target.Neighbors.Where(n => n.Walkable).OrderBy(r => Random.Range(-1f, 1f)).FirstOrDefault();
            else
            {
                isWait = true;
                return transform.position;
            }

        if (!targetNeighbor.Walkable)
        {
            if (targetNeighbor.transform.position != transform.position)
            {

                if (target.Neighbors.Any(n => n.Walkable))
                    targetNeighbor = target.Neighbors.Where(n => n.Walkable).OrderBy(r => Random.Range(-1f, 1f)).FirstOrDefault();
                else
                {
                    canMove = false;
                    isWait = true;
                    return transform.position;
                }
            }
            else
            {
                if (target.Neighbors.Contains(targetNeighbor))
                {
                    isWait = true;
                    return transform.position;
                }
                else
                {
                    if (target.Neighbors.Any(n => n.Walkable))
                        targetNeighbor = target.Neighbors.Where(n => n.Walkable).OrderBy(r => Random.Range(-1f, 1f)).FirstOrDefault();
                }
            }
        }

        //if (target.Neighbors.Any(n => n.Walkable))
        //    targetNeighbor = target.Neighbors.Where(n => n.Walkable).OrderBy(r => Random.Range(-1f, 1f)).FirstOrDefault();


        unitNode = GridManager.Instance.Tiles.Where(t => t.Key == new Vector2((int)transform.position.x, (int)transform.position.y)).First().Value;

        var path = Pathfinding.FindPath(unitNode, targetNeighbor);
        if (path == null || path.Count == 0)
        {
            canMove = false;
            isWait = true;
            targetNeighbor = target.Neighbors.Where(n => n.Walkable && n != targetNeighbor).OrderBy(r => Random.Range(-1f, 1f)).FirstOrDefault();
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
        if (target != cacheTarget)
            if (target.Neighbors.Any(n => n.Walkable))
            {
                targetNeighbor = target.Neighbors.Where(n => n.Walkable).OrderBy(r => Random.Range(-1f, 1f)).FirstOrDefault();
                cacheTarget = target;
            }

        return target.Neighbors.Contains(targetNeighbor);
    }
    void SetNextStep(Vector2 nextStep)
    {
        if (nextStep == (Vector2)target.transform.position)
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
