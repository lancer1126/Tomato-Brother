using Enemy;
using Player.Weapon.Ranged;
using UnityEngine;

namespace Player.Weapon.Projectile
{
    public class BulletPlayer : Bullet
    {
        protected int MaxPenetration; // 子弹最大贯通数
        protected int CurPenetration; // 当前剩余的贯通数
        protected float RepelPower; // 子弹把敌人击退的力
        protected Animator Anim;

        private void OnEnable()
        {
            Anim = GetComponent<Animator>();
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                if (CurPenetration <= 0)
                {
                    return;
                }

                CurPenetration--;
                var enemy = other.gameObject.GetComponent<BaseEnemy>();
                enemy.TakeDamage(Damage);
                enemy.TakeRepel(transform, RepelPower);

                if (CurPenetration <= 0)
                {
                    BulletEnd();
                }
                else
                {
                    // 每穿过一个敌人伤害就减半
                    Damage /= 2;
                }
            }
            else
            {
                BulletEnd();
            }
        }

        public override void InitFromWeapon(RangedWeapon weapon)
        {
            base.InitFromWeapon(weapon);
            MaxPenetration = weapon.bulletMaxPenetration;
            RepelPower = weapon.bulletRepelPower;
            CurPenetration = MaxPenetration;
        }

        private void BulletEnd()
        {
            IsBulletEnd = true;
            RecycleBullet();
        }

        private void RecycleBullet()
        {
            ReleaseAction?.Invoke();
        }
    }
}