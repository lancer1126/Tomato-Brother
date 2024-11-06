using System;
using Player.Weapon.Projectile;
using Pool;
using UnityEngine;
using Util;

namespace Player.Weapon.Ranged
{
    public class RangedWeapon : BaseWeapon
    {
        [Header("子弹相关")]
        public int bulletMaxPenetration; // 子弹最大穿透力
        public int bulletsPerShot; // 每次攻击多少发子弹
        public float bulletSpeed; // 子弹速度
        public float bulletAliveTime; // 子弹存活时间
        

        [SerializeField]
        protected float recoil; // 后坐力
        [SerializeField]
        protected GameObject muzzle; // 发射子弹的枪口

        protected override void Init()
        {
            base.Init();
            if (muzzle == null)
            {
                muzzle = transform.Find("Muzzle").gameObject;
            }

            if (bulletAliveTime == 0)
            {
                bulletAliveTime = 1;
            }

            if (bulletsPerShot == 0)
            {
                bulletsPerShot = 1;
            }
        }

        protected override void Attack()
        {
            if (!AttackTarget)
            {
                return;
            }

            if (AttackTimer < attackInterval)
            {
                AttackTimer += Time.deltaTime;
                return;
            }

            OpenFire();
            AttackTimer = 0;
        }

        /// <summary>
        /// 实现远程武器开火
        /// </summary>
        protected virtual void OpenFire()
        {
            AudioManager.Instance.Play(attackAudio, 0.1f);
            for (var i = 1; i <= bulletsPerShot; i++)
            {
                var bulletIns = PoolController.Instance.BulletDict[weaponName].GetFromPool();
                bulletIns.transform.position = muzzle.transform.position;
                bulletIns.transform.rotation = muzzle.transform.rotation;
                bulletIns.InitFromWeapon(this);
                if (i <= 1) continue;

                // 一次发射多发子弹的时候，设置弹道偏移
                var offset = i % 2 == 0 ? UnityEngine.Random.Range(-12, 0) : UnityEngine.Random.Range(0, 12);
                var eulerAngles = bulletIns.transform.eulerAngles;
                eulerAngles.z += offset;
                bulletIns.transform.eulerAngles = eulerAngles;
            }
        }

        /// <summary>
        /// 应用武器的后坐力
        /// </summary>
        protected virtual void RecoilAct() { }
    }
}