using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

namespace WE.Unit.Attack
{
    public class RangeAttack : BaseAttack
    {
        public GameObject bulletPrefab;
        Sprite spriteBullet;
        public override void Init(BaseUnit _owner)
        {
            base.Init(_owner);
            spriteBullet = Resources.LoadAll<Sprite>("Sprites/").Where(s => s.name == Owner.unitStats.unitName + " Bullet").FirstOrDefault();
        }
        public override void ExcuteAttack()
        {
            Owner.skeletonAnimation.OnStateEvent += OnAttack;
        }

        private void OnAttack()
        {
            Owner.skeletonAnimation.OnStateEvent -= OnAttack;
            Instantiate(bulletPrefab, Owner.ShotLoc, Quaternion.identity).GetComponent<UnitBullet>().SetDataBullet(Owner.Target, Owner, Owner.CurrentDamage, 1, spriteBullet);
        }
    }
}
