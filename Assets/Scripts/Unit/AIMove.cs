using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIMove : MonoBehaviour
{
    public bool canMove;
    public float speed;

    public NodeBase target;

    private Vector2 nextStep;

    private void OnEnable()
    {
        GridManager.Instance.UpdateGridNode(transform.position, false);
        SetNextStep(NextLoc());
    }
    private void Start()
    {
        GridManager.Instance.UpdateGridNode(transform.position, false);
        SetNextStep(NextLoc());
    }
    private void Update()
    {
        if (!canMove)
            return;

        Move();
    }
    void Move()
    {
        if (Vector2.Distance(transform.position, nextStep) != 0)
            transform.position = Vector3.MoveTowards(transform.position, nextStep, Time.deltaTime * speed);
        else
            SetNextStep(NextLoc());
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, nextStep);
    }
    public Vector2 NextLoc()
    {
        NodeBase unitNode = GridManager.Instance.Tiles.Where(t => t.Key == (Vector2)transform.position).First().Value;
        
        var path = Pathfinding.FindPath(unitNode, target);
        if (path == null||path.Count==0)
            return transform.position;
        else
            return path.Last().transform.position;
    }
    void SetNextStep(Vector2 nextStep)
    {
        if (!canMove)
            return;

        GridManager.Instance.UpdateGridNode(transform.position, true);
        GridManager.Instance.UpdateGridNode(nextStep, false);
        this.nextStep = nextStep;
    }
}
