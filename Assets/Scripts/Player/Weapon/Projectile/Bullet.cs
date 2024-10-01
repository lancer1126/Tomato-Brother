using System;
using UnityEngine;

namespace Player.Weapon.Projectile
{
    public class Bullet : MonoBehaviour
    {
        public int speed;

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
            transform.parent = null;
        }

        protected virtual void FixedUpdate()
        {
            if (IsBulletEnd)
            {
                Rb2.velocity = Vector2.zero;
            }
            else
            {
                Rb2.velocity = Forward * (speed * Time.deltaTime);
            }
        }

        public void SetDamage(float val)
        {
            Damage = val;
        }

        public void SetDeactivateAction(Action ra)
        {
            ReleaseAction = ra;
        }
    }
}