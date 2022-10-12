using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WE.Unit;

public class UnitBullet : MonoBehaviour
{
    public Unit target;
    public float speed;
    public int dame;
    public SpriteRenderer spriteBullet;

    public BoxCollider2D bulletCollider;

    void FixedUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, (Vector2)target.transform.position + Vector2.up, Time.deltaTime * speed);

        if (Vector2.Distance(transform.position, (Vector2)target.transform.position + Vector2.up) <= 0.15f)
            bulletCollider.enabled = true;
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

    public void SetDataBullet(Unit target, int dame, float speed = -1,Sprite bullet = null)
    {
        this.target = target;
        this.speed = speed == -1 ? this.speed : speed;
        this.dame = dame;

        if (bullet != null)
            spriteBullet.sprite = bullet;
    }
    public void SetDataBullet(BaseUnit target,BaseUnit source, float dame, float speed = -1, Sprite bullet = null)
    {
        //this.baseTarget = target;
        //this.speed = speed == -1 ? this.speed : speed;
        //this.dame = dame;
        //this.source = source;

        //if (bullet != null)
        //    spriteBullet.sprite = bullet;
    }
}
