using System;
using System.Collections.Generic;
using Player.Weapon.Projectile;
using UnityEngine;
using Util;

namespace Player.Weapon
{
    public class BaseWeapon : MonoBehaviour
    {
        public bool haveBullet; // 是否有子弹组件
        public int level; // 等级
        public int weaponIndex = -1; // 当前武器在背包中的索引
        public float damage; // 伤害
        public float repelPower; // 攻击对敌人的击退力
        public string weaponName; // 武器名称
        public Bullet bullet; //子弹组件

        [SerializeField]
        protected int enemyDetectCount; // 一次检测敌人的数量
        [SerializeField]
        protected float detectRange; // 检测敌人的范围
        [SerializeField]
        protected float attackInterval; // 攻击间隔
        [SerializeField]
        protected float detectInterval; // 检测敌人的间隔
        [SerializeField]
        protected AudioClip attackAudio; // 攻击音效
        protected bool FindEnemy; // 当前是否检测到敌人
        protected int EnemyLayer; // 敌人的Layer
        protected float AttackTimer; // 两次攻击之间的计时器
        protected float DetectTimer; // 检测敌人的计时器
        protected float CurLsY; // 武器当前在y轴上的LocalScale属性，用于控制旋转
        protected Vector3 DefaultLocalScale; // 武器默认的大小和方向
        protected Vector3 DefaultRotate; // 武器默认的旋转角度
        protected Vector3 OriginalLocalPos; // 初始的相对位置
        protected GameObject AttackTarget; // 攻击的目标

        protected virtual void Awake() { }

        protected virtual void Start()
        {
            Init();
        }

        protected virtual void FixedUpdate()
        {
            DetectEnemy();
            WeaponSighting();
            Attack();
        }

        protected virtual void Update() { }

        protected virtual void OnTriggerEnter2D(Collider2D other) { }

        /// <summary>
        /// 初始化属性
        /// </summary>
        protected virtual void Init()
        {
            CurLsY = transform.localScale.y;
            DefaultLocalScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
            DefaultRotate = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 1);
            OriginalLocalPos = transform.localPosition;
            EnemyLayer = LayerUtil.GetEnemy();

            if (detectInterval == 0)
            {
                detectInterval = 0.2f;
            }

            if (AttackTimer == 0)
            {
                AttackTimer = 1;
            }

            if (detectInterval == 0)
            {
                detectInterval = attackInterval;
            }
        }

        /// <summary>
        /// 检测离玩家最近的敌人
        /// </summary>
        protected virtual void DetectEnemy()
        {
            DetectTimer += Time.deltaTime;
            if (DetectTimer < detectInterval)
            {
                return;
            }

            // 获取一个存放周围敌人碰撞体的数组
            var colliders = ColliderPool.Get(enemyDetectCount);
            // 检测周围的敌人
            var enemyCount = Physics2D.OverlapCircleNonAlloc(transform.position, detectRange, colliders, EnemyLayer);

            // 选取最近的一个敌人
            AttackTarget = null;
            var shortestDir = Mathf.Infinity;
            for (var i = 0; i < enemyCount; i++)
            {
                var c = colliders[i];
                var distance = Vector3.SqrMagnitude(transform.position - c.transform.position);
                if (!(distance < shortestDir))
                {
                    continue;
                }

                shortestDir = distance;
                AttackTarget = c.gameObject;
            }

            ColliderPool.Release(colliders);
            FindEnemy = AttackTarget;
            DetectTimer = 0;
        }

        /// <summary>
        ///  将武器指向检测到的敌人
        /// </summary>
        protected virtual void WeaponSighting()
        {
            if (FindEnemy && AttackTarget)
            {
                // 敌人位置
                var enemyPos = AttackTarget.transform.position;
                // 敌人对玩家的方向
                var toDir = (enemyPos - transform.position).normalized;
                // 旋转角度
                var rotateAngle = Mathf.Atan2(toDir.y, toDir.x) * Mathf.Rad2Deg;
                // 武器朝向左侧时控制武器是否翻转
                var lsY = enemyPos.x < transform.position.x ? -CurLsY : CurLsY;
                // 玩家是否面向右边
                var playerToRight = PlayerFacingRight();
                var lsX = transform.localScale.x;
                // 判断是否需要翻转
                if ((playerToRight && lsX < 0) || (!playerToRight && lsX > 0))
                {
                    lsX = -lsX;
                }

                transform.rotation = Quaternion.Euler(0, 0, rotateAngle);
                transform.localScale = new Vector3(lsX, lsY, 1);
            }
            else
            {
                // 没检测到敌人就将武器复原
                transform.localScale = DefaultLocalScale;
                transform.localEulerAngles = DefaultRotate;
            }
        }

        /// <summary>
        /// 进行攻击
        /// </summary>
        protected virtual void Attack() { }

        /// <summary>
        /// 玩家是否朝向右边
        /// </summary>
        /// <returns></returns>
        protected bool PlayerFacingRight()
        {
            return transform.parent.localScale.x >= 0;
        }

        protected static class ColliderPool
        {
            private static readonly Queue<Collider2D[]> Pool = new();

            public static Collider2D[] Get(int size)
            {
                if (Pool.Count <= 0)
                {
                    return new Collider2D[size];
                }

                var arr = Pool.Dequeue();
                return arr.Length >= size ? arr : new Collider2D[size];
            }

            public static void Release(Collider2D[] arr)
            {
                Array.Clear(arr, 0, arr.Length);
                Pool.Enqueue(arr);
            }
        }
    }
}