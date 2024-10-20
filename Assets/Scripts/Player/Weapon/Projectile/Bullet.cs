using System;
using Enemy.Ranged;
using Player.Weapon.Ranged;
using UnityEngine;

namespace Player.Weapon.Projectile
{
    public class Bullet : MonoBehaviour
    {
        protected bool IsBulletEnd;
        protected float Speed;
        protected float Damage;
        protected float AliveDuration; // 子弹存活时间
        protected Vector2 Forward;
        protected Action ReleaseAction;
        protected Rigidbody2D Rb2;
        
        private float _aliveTimer; // 子弹存活计时器

        protected virtual void Awake()
        {
            Rb2 = GetComponent<Rigidbody2D>();
        }

        protected virtual void OnEnable()
        {
            _aliveTimer = 0;
        }

        protected virtual void Start()
        {
            Forward = transform.right.normalized;
        }

        protected virtual void FixedUpdate()
        {
            _aliveTimer += Time.deltaTime;
            if (_aliveTimer >= AliveDuration)
            {
                IsBulletEnd = true;
                _aliveTimer = 0;
            }
            
            if (IsBulletEnd)
            {
                Rb2.velocity = Vector2.zero;
                ToRecycle();
            }
            else
            {
                Rb2.velocity = Forward * Speed;
            }
        }

        public virtual void InitFromWeapon(RangedWeapon weapon)
        {
            Speed = weapon.bulletSpeed;
            Damage = weapon.damage;
            AliveDuration = weapon.bulletAliveTime;
        }

        public virtual void InitFromEnemy(RangedEnemy enemy)
        {
            Speed = enemy.bulletSpeed;
            Damage = enemy.damage;
            AliveDuration = enemy.bulletAliveTime;
        }

        public void SetDeactivateAction(Action ra)
        {
            ReleaseAction = ra;
        }

        protected virtual void ToRecycle()
        {
            ReleaseAction?.Invoke();
        }
    }
}