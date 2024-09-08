using System;
using UnityEngine;

namespace Enemy
{
    public class BaseEnemy : MonoBehaviour
    {
        public int level;
        public int attack;
        public float moveSpeed;
        public float maxHealth;
        public float currentHealth;
        public Vector2 forward;

        protected Action ReleaseAction; // 将对象回收到对象池方法
        protected Func<BaseEnemy> GetAction; // 从对象池中获取对象方法
        protected Vector3 ToPlayerDir; // 敌人与玩家的向量
        protected GameObject Player;
        protected Animator Anim;
        protected Rigidbody2D Rb2;

        private static readonly int IsRun = Animator.StringToHash("isRun");
        private bool _isHurt;
        private bool _isDead;

        protected virtual void Awake()
        {
            Player = GameObject.FindWithTag("Player");
            Anim = GetComponent<Animator>();
            Rb2 = GetComponent<Rigidbody2D>();
        }

        protected virtual void OnEnable()
        {
            currentHealth = maxHealth;
            _isDead = false;
            _isHurt = false;
        }

        protected virtual void FixedUpdate()
        {
            ToPlayerDir = Player.transform.position - transform.position;
            if (_isDead || _isHurt)
            {
                Rb2.velocity = Vector2.zero;
            }
            else
            {
                // 向玩家方向移动
                forward = ToPlayerDir.normalized;
                Move();
            }
        }

        public void SetDeactivateAction(Action ra)
        {
            ReleaseAction = ra;
        }

        public void SetActiveAction(Func<BaseEnemy> getFunc)
        {
            GetAction = getFunc;
        }

        public virtual void TakeDamage(int damage)
        {
            if (_isDead)
            {
                return;
            }

            if (currentHealth <= damage)
            {
                currentHealth = 0;
                Die();
            }
            else
            {
                currentHealth -= damage;
            }
        }

        protected virtual void Move()
        {
            if (ToPlayerDir.magnitude > 0.5f)
            {
                Anim.SetBool(IsRun, true);
                ToPlayerDir.Normalize();

                // 转向
                transform.rotation = ToPlayerDir.x switch
                {
                    > 0 => Quaternion.Euler(new Vector3(0, 0, 0)),
                    < 0 => Quaternion.Euler(new Vector3(0, 180, 0)),
                    _ => transform.rotation
                };
                Rb2.velocity = forward * (moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position += Vector3.forward;
            }
        }

        protected virtual void Die()
        {
            _isDead = true;
            ReleaseAction?.Invoke();
        }
    }
}