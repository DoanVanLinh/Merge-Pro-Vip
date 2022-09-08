using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBullet : MonoBehaviour
{
    public Unit target;
    public float speed;
    public int dame;
    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, (Vector2)target.transform.position + Vector2.up, Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != target.gameObject)
            return;

        Dame();
    }

    void Dame()
    {
        target.TakeDame(dame);

        Destroy(gameObject);
    }

    public void SetDataBullet(Unit target, int dame, float speed = -1)
    {
        this.target = target;
        this.speed = speed == -1 ? this.speed : speed;
        this.dame = dame;
    }
}
