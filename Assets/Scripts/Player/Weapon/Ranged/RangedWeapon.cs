using System;
using Player.Weapon.Projectile;
using Pool;
using UnityEngine;

namespace Player.Weapon.Ranged
{
    public class RangedWeapon : BaseWeapon
    {
        public int bulletMaxPenetration; // 子弹最大穿透力
        public float bulletSpeed; // 子弹速度
        public float bulletAliveTime; // 子弹存活时间
        public Bullet bullet; //子弹组件

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
            AttackTimer -= attackInterval;
        }

        /// <summary>
        /// 实现远程武器开火
        /// </summary>
        protected virtual void OpenFire()
        {
            AudioManager.Instance.Play(attackAudio, muzzle.transform.position, 0.1f);

            var bulletIns = PoolController.Instance.BulletDict[weaponName].GetFromPool();
            bulletIns.transform.position = muzzle.transform.position;
            bulletIns.transform.rotation = muzzle.transform.rotation;
            bulletIns.InitFromWeapon(this);
        }

        /// <summary>
        /// 应用武器的后坐力
        /// </summary>
        protected virtual void RecoilAct() { }
    }
}