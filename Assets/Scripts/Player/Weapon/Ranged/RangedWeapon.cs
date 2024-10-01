using Pool;
using UnityEngine;

namespace Player.Weapon.Ranged
{
    public class RangedWeapon : BaseWeapon
    {
        public int bulletMaxPenetration;
        public float bulletSpeed;
        public float bulletRepelPower;
        [SerializeField]
        protected float recoil; // 后坐力
        [SerializeField]
        protected GameObject muzzle; // 发射子弹的枪口

        protected override void Awake()
        {
            haveBullet = true;
        }

        protected override void Init()
        {
            base.Init();
            if (muzzle == null)
            {
                muzzle = transform.Find("Muzzle").gameObject;
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
            AudioSource.PlayClipAtPoint(attackAudio, muzzle.transform.position);
            
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