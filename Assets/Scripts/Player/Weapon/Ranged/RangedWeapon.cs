using Pool;
using UnityEngine;

namespace Player.Weapon.Ranged
{
    public class RangedWeapon : BaseWeapon
    {
        [SerializeField]
        protected float recoil; // 后坐力
        [SerializeField]
        protected GameObject muzzle; // 发射子弹的枪口
        [SerializeField]
        protected GameObject bulletPoolObj;    // 子弹池子对象
        [SerializeField]
        protected GameObject bullet;
        protected BulletPool BulletPool; // 子弹池

        protected override void OnEnable()
        {
            BulletPool = bulletPoolObj.GetComponent<BulletPool>();
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
            
            var bulletIns = BulletPool.GetFromPool();
            bulletIns.transform.position = muzzle.transform.position;
            bulletIns.transform.rotation = muzzle.transform.rotation;
            bulletIns.SetDamage(damage);
        }

        /// <summary>
        /// 应用武器的后坐力
        /// </summary>
        protected virtual void RecoilAct() { }
    }
}