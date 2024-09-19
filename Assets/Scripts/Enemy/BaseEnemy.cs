using System;
using System.Collections;
using Player;
using UnityEngine;

namespace Enemy
{
    public class BaseEnemy : MonoBehaviour
    {
        private static readonly int IsRun = Animator.StringToHash("isRun");
        private static readonly int IsDead = Animator.StringToHash("isDead");

        public int level;
        public int damage = 10;
        public float moveSpeed;
        public float maxHealth;
        public float currentHealth;
        public Vector2 forward;

        [SerializeField]
        protected float attackInterval = 1;
        [SerializeField]
        public float enemyPushForce = 100;
        protected Action ReleaseAction; // 将对象回收到对象池方法
        protected Func<BaseEnemy> GetAction; // 从对象池中获取对象方法
        protected Vector3 ToPlayerDir; // 敌人与玩家的向量
        protected GameObject Player;
        protected Animator Anim;
        protected Rigidbody2D Rb2;
        protected bool IsHurt;
        protected bool Dead;
        protected float AttackTimer;

        protected virtual void Awake()
        {
            Player = GameObject.FindWithTag("Player");
            Anim = GetComponent<Animator>();
            Rb2 = GetComponent<Rigidbody2D>();
        }

        protected virtual void OnEnable()
        {
            currentHealth = maxHealth;
            Dead = false;
            IsHurt = false;
        }

        protected virtual void FixedUpdate()
        {
            ToPlayerDir = Player.transform.position - transform.position;
            if (Dead || IsHurt)
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

        /// <summary>
        /// 触碰到玩家时进行攻击
        /// </summary>
        /// <param name="other"></param>
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Attack(other);
            }
        }

        /// <summary>
        /// 当持续触碰到玩家时，隔一段时间进行攻击
        /// </summary>
        /// <param name="other"></param>
        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            if (AttackTimer >= attackInterval)
            {
                Attack(other);
                AttackTimer = 0;
            }
            else
            {
                AttackTimer += Time.deltaTime;
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

        /// <summary>
        /// 被子弹击中时扣除血量
        /// </summary>
        /// <param name="hurtDamage"></param>
        public virtual void TakeDamage(int hurtDamage)
        {
            if (Dead)
            {
                return;
            }

            if (currentHealth <= hurtDamage)
            {
                currentHealth = 0;
                Die();
            }
            else
            {
                currentHealth -= hurtDamage;
            }
        }

        /// <summary>
        /// 敌人被子弹击中时触发击退效果
        /// </summary>
        public virtual void TakeRepel(Transform attacker, float repelPower)
        {
            if (currentHealth == 0)
            {
                return;
            }

            var dir = (transform.position - attacker.position).normalized;
            StartCoroutine(ToRepel(dir * repelPower));
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
            Dead = true;
            Anim.SetBool(IsDead, true);
        }

        /// <summary>
        /// 对玩家进行攻击
        /// </summary>
        protected virtual void Attack(Collider2D other)
        {
            var otherObj = other.gameObject;
            if (!otherObj.tag.Equals("Player"))
            {
                return;
            }

            var playerController = other.gameObject.GetComponent<PlayerController>();
            playerController?.TakeDamage(damage);
        }

        protected void RecycleEnemy()
        {
            ReleaseAction?.Invoke();
        }

        private IEnumerator ToRepel(Vector3 power)
        {
            IsHurt = true;
            Rb2.AddForce(power, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.1f);
            IsHurt = false;
        }
    }
}