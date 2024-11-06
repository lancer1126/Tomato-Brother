using System;
using System.Collections.Generic;
using Player.Weapon.Projectile;
using Pool;
using UnityEngine;

namespace Player.Weapon.Melee
{
    public class LightingShiv : MeleeWeapon
    {
        [Header("子弹")]
        public int maxPenetration;
        public float bulletSpeed;
        public float bulletDamage;
        public float bulletAliveDuration;
        public float bulletRepelPower;

        [SerializeField]
        private int bulletDetectRange = 12;
        [SerializeField]
        private GameObject muzzle;
        [SerializeField]
        private AudioClip lightingSound;

        private const float LimitDistance = 3; // 子弹检测目标时，不检测该范围之内的

        protected override void MeleeAttackCollide(Collider2D other)
        {
            base.MeleeAttackCollide(other);
            LightingShot();
        }

        /// <summary>
        /// 碰撞到第一个敌人后生成一个闪电子弹
        /// </summary>
        private void LightingShot()
        {
            if (!bullet || !AttackTarget) return;
            
            // 子弹起始位置
            var originPos = AttackTarget.transform.position;
            // 检测周围的敌人
            var colliders = ColliderPool.Get(enemyDetectCount);
            var enemyCount = Physics2D.OverlapCircleNonAlloc(originPos, bulletDetectRange, colliders, EnemyLayer);
            if (enemyCount == 0) return;

            var validEnemyColliders = new List<Collider2D>();
            for (var i = 0; i < enemyCount; i++)
            {
                var cPos = colliders[i].bounds.center;
                var distance = Vector2.Distance(cPos, originPos);
                if (distance > LimitDistance)
                {
                    validEnemyColliders.Add(colliders[i]);
                }
            }
            if (validEnemyColliders.Count <= 0) return;
            
            // 选取随机一个敌人
            var index = UnityEngine.Random.Range(0, validEnemyColliders.Count);
            var bulletTarget = validEnemyColliders[index].gameObject;
            if (!bulletTarget) return;
            
            AudioManager.Instance.Play(lightingSound, 0.1f);
            var bulletIns = PoolController.Instance.BulletDict[weaponName].GetFromPool();
            var bl = (BulletLighting)bulletIns;
            bl.Init(this);

            // 设置初始位置
            bulletIns.transform.position = originPos;
            // 设置旋转角度
            var dir = (bulletTarget.transform.position - bulletIns.transform.position).normalized;
            bulletIns.transform.right = dir;
        }
    }
}