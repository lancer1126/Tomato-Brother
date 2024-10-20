using Player.Weapon.Projectile;
using Pool;
using UnityEngine;

namespace Enemy.Ranged
{
    public class RangedEnemy : BaseEnemy
    {
        public float bulletSpeed = 5f;
        public float bulletAliveTime = 5f;
        [SerializeField]
        protected Bullet bullet;
        
        protected override void FixedUpdate()
        {
            MoveToPlayer();
            if (AttackTimer > attackInterval)
            {
                RemoteAttack();
                AttackTimer = 0;
            }
            else
            {
                AttackTimer += Time.deltaTime;
            }
        }

        protected override void OnTriggerEnter2D(Collider2D other) { }
        
        protected override void OnTriggerStay2D(Collider2D other) { }

        protected virtual void RemoteAttack()
        {
            var bulletIns = PoolController.Instance.EnemyBulletDict[bullet.name].GetFromPool();
            // 将子弹的位置初始化为敌人的位置
            bulletIns.transform.position = transform.position;
            // 子弹的朝向设为对玩家的方向
            bulletIns.transform.right = forward;
            bulletIns.InitFromEnemy(this);
        }
    }
}