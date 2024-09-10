using Enemy;
using UnityEngine;

namespace Player.Weapon
{
    public class BulletPlayer : Bullet
    {
        [SerializeField]
        protected int maxPenetration = 1; // 子弹最大贯通数
        protected int CurPenetration; // 当前剩余的贯通数
        [SerializeField]
        protected float repelPower; // 子弹把敌人击退的力
        protected Animator Anim;
        
        private static readonly int Explosion = Animator.StringToHash("explosion");

        private void OnEnable()
        {
            CurPenetration = maxPenetration;
            Anim = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                if (CurPenetration <= 0) return;
                CurPenetration--;
                var enemy = other.gameObject.GetComponent<BaseEnemy>();
                enemy.TakeDamage(damage);
                enemy.TakeRepel(transform, repelPower);

                if (CurPenetration > 0) return;
                BulletExplosion();
            }
            else
            {
                BulletExplosion();
            }
        }

        private void BulletExplosion()
        {
            BulletEnd = true;
            Anim.SetTrigger(Explosion);
        }

        private void RecycleBullet()
        {
            ReleaseAction?.Invoke();
        }
    }
}