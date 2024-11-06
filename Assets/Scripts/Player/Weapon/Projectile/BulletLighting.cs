using Player.Weapon.Melee;
using UnityEngine;

namespace Player.Weapon.Projectile
{
    public class BulletLighting : BulletPlayer
    {
        [SerializeField]
        private float coldTime = 0.2f; // 刚生成时有一定冷却时间，这时间内不攻击敌人
        private float _coldTimer;

        protected override void FixedUpdate()
        {
            _coldTimer += Time.deltaTime;
            base.FixedUpdate();
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (_coldTimer < coldTime) return;
            base.OnTriggerEnter2D(other);
        }

        public void Init(LightingShiv ls)
        {
            Speed = ls.bulletSpeed;
            Damage = ls.bulletDamage;
            AliveDuration = ls.bulletAliveDuration;
            MaxPenetration = ls.maxPenetration;
            CurPenetration = MaxPenetration;
            RepelPower = ls.bulletRepelPower;
        }
    }
}