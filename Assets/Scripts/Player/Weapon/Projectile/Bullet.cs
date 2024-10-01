using System;
using Player.Weapon.Ranged;
using UnityEngine;

namespace Player.Weapon.Projectile
{
    public class Bullet : MonoBehaviour
    {
        protected float Speed;
        protected bool IsBulletEnd;
        protected float Damage;
        protected Vector2 Forward;
        protected Action ReleaseAction;
        protected Rigidbody2D Rb2;

        protected virtual void Awake()
        {
            Rb2 = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            Forward = transform.right.normalized;
        }

        protected virtual void FixedUpdate()
        {
            if (IsBulletEnd)
            {
                Rb2.velocity = Vector2.zero;
            }
            else
            {
                Rb2.velocity = Forward * (Speed * Time.deltaTime);
            }
        }

        public virtual void InitFromWeapon(RangedWeapon weapon)
        {
            Speed = weapon.bulletSpeed;
            Damage = weapon.damage;
        }

        public void SetDeactivateAction(Action ra)
        {
            ReleaseAction = ra;
        }
    }
}