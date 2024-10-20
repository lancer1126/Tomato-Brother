using System;
using System.Collections;
using DG.Tweening;
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
        public Vector3 forward;

        [SerializeField]
        protected float attackInterval = 1; // 攻击间隔
        [SerializeField]
        private float knockbackDuration = 0.2f; // 被击退的持续时间
        [SerializeField]
        private float deadDuration = 0.3f; // 死亡动画持续时间
        [SerializeField]
        protected AudioClip hurtSound;
        protected bool IsDead;
        protected bool IsHurt;
        protected float AttackTimer; // 攻击计时器
        protected Action ReleaseAction; // 将对象回收到对象池方法
        protected Func<BaseEnemy> GetAction; // 从对象池中获取对象方法
        protected Vector3 ToPlayerDir; // 敌人与玩家的向量
        protected Vector3 OriginalLs; // 初始LocalScale
        protected GameObject Player; // 玩家
        protected Rigidbody2D Rb2;
        protected SpriteRenderer SpriteR;
        protected Color OriginalColor;
        protected Coroutine MoveAnimCr;

        protected virtual void Awake()
        {
            Player = GameObject.FindWithTag("Player");
            Rb2 = GetComponent<Rigidbody2D>();
            SpriteR = GetComponent<SpriteRenderer>();
            OriginalLs = transform.localScale;
        }

        protected virtual void OnEnable()
        {
            IsDead = false;
            IsHurt = false;
            transform.localScale = OriginalLs;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            currentHealth = maxHealth;
            OriginalColor = SpriteR.color;
            StartAnimation();
        }

        protected virtual void FixedUpdate()
        {
            MoveToPlayer();
        }

        protected virtual void Update()
        {
            CheckOrientation();
            UpdateAnimation();
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Attack(other);
            }
        }

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
        
        /// <summary>
        /// 被子弹击中时的逻辑
        /// </summary>
        /// <param name="attacker">攻击者，一般是玩家</param>
        /// <param name="hurtDamage">伤害值</param>
        /// <param name="repelPower">受击的后退力</param>
        public virtual void TakeDamage(Transform attacker, float hurtDamage, float repelPower)
        {
            if (IsDead) return;

            IsHurt = true;
            // 播放受伤音效
            // AudioManager.Instance.Play(hurtSound, transform.position, 0.05f);
            // 受攻击时触发敌人颜色变色
            StartCoroutine(HitFlash());
            // 被击中后的特效
            HitEffect();
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
        /// 回收对象
        /// </summary>
        /// <param name="ra"></param>
        public void SetDeactivateAction(Action ra)
        {
            ReleaseAction = ra;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="getFunc"></param>
        public void SetActiveAction(Func<BaseEnemy> getFunc)
        {
            GetAction = getFunc;
        }

        /// <summary>
        /// 开启控制游戏动画的协程
        /// </summary>
        protected virtual void StartAnimation()
        {
            if (MoveAnimCr != null)
            {
                StopCoroutine(MoveAnimCr);
            }

            MoveAnimCr = StartCoroutine(MoveAnimation());
        }

        /// <summary>
        /// 判断游戏对象的朝向
        /// </summary>
        protected virtual void CheckOrientation()
        {
            var currentScale = transform.localScale;
            currentScale.x = Mathf.Sign(forward.x) * Mathf.Abs(currentScale.x);
            transform.localScale = currentScale;
            OriginalLs.x = Mathf.Sign(forward.x) * Mathf.Abs(OriginalLs.x);
        }

        /// <summary>
        /// 更新角色动画
        /// </summary>
        protected virtual void UpdateAnimation()
        {
            switch (IsDead)
            {
                case false when MoveAnimCr == null:
                    StartAnimation();
                    break;
                case true when MoveAnimCr != null:
                    StopCoroutine(MoveAnimCr);
                    MoveAnimCr = null;
                    break;
            }
        }

        /// <summary>
        /// 被击中后的特效
        /// </summary>
        protected virtual void HitEffect()
        {
            TriggerHit();
            TriggerBurst();
        }

        /// <summary>
        /// 敌人被子弹击中时触发击退效果
        /// </summary>
        protected virtual void TakeKnockBack(Transform attacker, float repelPower)
        {
            // 击退方向为当前正在前进的反方向
            // var knockbackDir = -forward;
            var knockbackDir = attacker.right.normalized;
            // 计算敌人被击退到的位置
            var knockbackTarget = transform.position + knockbackDir * repelPower;

            transform.DOMove(knockbackTarget, knockbackDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => { });
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
                Rb2.velocity = forward * moveSpeed;
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
            var sequence = DOTween.Sequence();
            sequence.Join(transform.DORotate(new Vector3(0, 0, 350), deadDuration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear));
            sequence.Join(transform.DOScale(0, deadDuration)
                .SetEase(Ease.Linear));
            sequence.Play();

            // 生成战利品
            CheckLoot();
            sequence.OnComplete(() => { StartCoroutine(RecycleEnemy()); });
        }

        /// <summary>
        /// 计算是否生成金币
        /// </summary>
        protected virtual void CheckLoot(int goldVal = 3)
        {
            var random = UnityEngine.Random.Range(1, 4);
            if (random != 1) return;

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
            if (!otherObj.tag.Equals("Player")) return;

            var playerController = other.gameObject.GetComponent<PlayerController>();
            playerController?.TakeDamage(damage);
        }
        
        /// <summary>
        /// 移动动画
        /// </summary>
        protected virtual IEnumerator MoveAnimation()
        {
            while (!IsDead)
            {
                yield return transform
                    .DOScale(new Vector3(OriginalLs.x * 1.2f, OriginalLs.y * 0.8f, OriginalLs.z), 0.3f)
                    .SetEase(Ease.InOutQuad)
                    .WaitForCompletion();
            
                yield return transform.DOScale(OriginalLs, 0.7f)
                    .SetEase(Ease.InOutQuad)
                    .WaitForCompletion();
            }
        }

        /// <summary>
        /// 使用协程在敌人死亡后回收实例
        /// </summary>
        protected IEnumerator RecycleEnemy()
        {
            yield return null;
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

        /// <summary>
        /// 触发击中动画 
        /// </summary>
        private void TriggerHit()
        {
            var hit = HitPool.Instance.GetFromPool();
            hit.transform.position = transform.position;
            hit.transform.localScale = transform.localScale;
        }

        /// <summary>
        /// 被击中时触发爆炸粒子
        /// </summary>
        private void TriggerBurst()
        {
            var burst = BurstPool.Instance.GetFromPool();
            burst.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
            burst.Play();
        }
    }
}