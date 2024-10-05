using System;
using System.Collections;
using Effect;
using Player;
using Pool;
using UnityEngine;

namespace Enemy
{
    public class BaseEnemy : MonoBehaviour
    {
        protected static readonly int AnimIsRun = Animator.StringToHash("isRun");
        protected static readonly int AnimIsDead = Animator.StringToHash("isDead");

        public int level;
        public int damage = 10;
        public int bornLevel; // 当前父对象已生成第几级子对象
        public float moveSpeed;
        public float maxHealth;
        public float currentHealth;
        public Vector2 forward;
        public GameObject burstParticles;

        [SerializeField]
        protected float attackInterval = 1;
        [SerializeField]
        protected float enemyPushForce = 100;
        [SerializeField]
        protected AudioClip hurtSound;
        protected bool IsDead;
        protected bool IsHurt;
        protected float AttackTimer; // 攻击计时器
        protected Action ReleaseAction; // 将对象回收到对象池方法
        protected Func<BaseEnemy> GetAction; // 从对象池中获取对象方法
        protected Vector3 ToPlayerDir; // 敌人与玩家的向量
        protected GameObject Player; // 玩家
        protected Animator Anim;
        protected Rigidbody2D Rb2;
        protected SpriteRenderer SpriteR;
        protected Color OriginalColor;

        protected virtual void Awake()
        {
            Player = GameObject.FindWithTag("Player");
            Anim = GetComponent<Animator>();
            Rb2 = GetComponent<Rigidbody2D>();
            SpriteR = GetComponent<SpriteRenderer>();
        }

        protected virtual void OnEnable()
        {
            IsDead = false;
            IsHurt = false;
            currentHealth = maxHealth;
            OriginalColor = SpriteR.color;
        }

        protected virtual void FixedUpdate()
        {
            MoveToPlayer();
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
        /// 被子弹击中时的逻辑
        /// </summary>
        /// <param name="attacker">攻击者，一般是玩家</param>
        /// <param name="hurtDamage">伤害值</param>
        /// <param name="repelPower">受击的后退力</param>
        public virtual void TakeDamage(Transform attacker, float hurtDamage, float repelPower)
        {
            if (IsDead)
            {
                return;
            }

            IsHurt = true;
            // 播放受伤音效
            // AudioManager.Instance.Play(hurtSound, transform.position, 0.05f);
            // 受攻击时触发敌人颜色变色
            StartCoroutine(HitFlash());
            // 爆炸粒子
            TriggerBurst();
            // 受攻击后退
            TakeKnockBack(attacker, repelPower);
            IsHurt = false;
            
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
        /// 被击中时触发爆炸粒子
        /// </summary>
        protected virtual void TriggerBurst()
        {
            var burst = BurstPool.Instance.GetFromPool();
            burst.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
            burst.Play();
        }

        /// <summary>
        /// 敌人被子弹击中时触发击退效果
        /// </summary>
        protected virtual void TakeKnockBack(Transform attacker, float repelPower)
        {
            var originalSpeed = moveSpeed;
            moveSpeed = 0;
            var dir = (transform.position - attacker.position).normalized;
            Rb2.AddForce(dir * repelPower, ForceMode2D.Impulse);
            moveSpeed = originalSpeed;
        }
        

        /// <summary>
        /// 朝玩家方向移动
        /// </summary>
        protected virtual void MoveToPlayer()
        {
            ToPlayerDir = Player.transform.position - transform.position;
            if (IsDead || IsHurt)
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
        /// 移动
        /// </summary>
        protected virtual void Move()
        {
            if (ToPlayerDir.magnitude > 0.5f)
            {
                Anim.SetBool(AnimIsRun, true);
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

        /// <summary>
        /// 死亡逻辑
        /// </summary>
        protected virtual void Die()
        {
            IsDead = true;
            Anim.SetBool(AnimIsDead, true);
            CheckGetGold();
            StartCoroutine(RecycleEnemy());
        }

        /// <summary>
        /// 计算是否生成金币
        /// </summary>
        protected virtual void CheckGetGold(int goldVal = 3)
        {
            var random = UnityEngine.Random.Range(1, 4);
            if (random != 1)
            {
                return;
            }

            var gold = GoldPool.Instance.GetFromPool();
            gold.SetGoldValue(goldVal);
            gold.transform.position = transform.position;
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

        /// <summary>
        /// 使用协程在敌人死亡后回收实例
        /// </summary>
        protected IEnumerator RecycleEnemy()
        {
            yield return new WaitForSeconds(0.5f);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            ReleaseAction?.Invoke();
        }

        /// <summary>
        /// 获取随机方向
        /// </summary>
        /// <returns></returns>
        protected Vector2 GetRandomV2()
        {
            var x = UnityEngine.Random.Range(-1, 2);
            var y = UnityEngine.Random.Range(-1, 2);
            return new Vector2(x, y);
        }

        /// <summary>
        /// 设置被攻击后的无敌时间
        /// </summary>
        /// <param name="time">无敌时间</param>
        /// <returns></returns>
        private IEnumerator SetInvulnerable(float time = 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
            IsHurt = false;
        }

        /// <summary>
        /// 受到攻击时变色，持续数帧
        /// </summary>
        /// <returns></returns>
        private IEnumerator HitFlash(int frames = 5)
        {
            SpriteR.color = Color.red;
            for (var i = 0; i < frames; i++)
            {
                yield return null;
            }

            SpriteR.color = OriginalColor;
        }
    }
}