using Enemy;
using UnityEngine;

namespace Player.Weapon
{
    public class BulletPlayer : Bullet
    {
        [SerializeField]
        protected int maxPenetration;   // 子弹最大贯通数
        protected int CurPenetration;   // 当前剩余的贯通数
        [SerializeField]
        protected float repelPower; // 子弹把敌人击退的力

        private void OnEnable()
        {
            CurPenetration = maxPenetration;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                if (CurPenetration > 0)
                {
                    CurPenetration--;
                    var enemy = other.gameObject.GetComponent<BaseEnemy>();
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}