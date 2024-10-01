using System.Collections.Generic;
using Enemy;
using Player.Weapon.Projectile;
using UnityEngine;

namespace Pool
{
    public class PoolController : MonoBehaviour
    {
        public static PoolController Instance { get; private set; }

        public List<EnemyPool> enemyPools;
        public List<BulletPool> bulletPools;
        public List<BulletPool> enemyBulletPools;

        [SerializeField]
        private Transform poolEnemyGroup;
        [SerializeField]
        private Transform poolBulletGroup;
        [SerializeField]
        private BaseEnemy[] enemyTypes;
        [SerializeField]
        private Bullet[] bulletTypes;
        [SerializeField]
        private Bullet[] enemyBulletTypes;

        private void Awake()
        {
            Instance = this;
            InitPoolGroup();
            InitPool();
        }

        private void InitPoolGroup()
        {
            if (poolEnemyGroup == null)
            {
                poolEnemyGroup = transform.Find("Group-Enemy");
            }

            if (poolBulletGroup == null)
            {
                poolBulletGroup = transform.Find("Group-Bullet");
            }
        }

        private void InitPool()
        {
            InitEnemyPool();
            // InitBulletPool();
            InitEnemyBulletPool();
        }

        private void InitEnemyPool()
        {
            enemyPools = new List<EnemyPool>();
            // 遍历敌人列表，为每种敌人生成一个对象池，将其挂载到pool-enemy之下
            foreach (var enemy in enemyTypes)
            {
                var poolHolder = new GameObject($"Pool-{enemy.name}")
                {
                    transform =
                    {
                        parent = poolEnemyGroup,
                        position = transform.position
                    }
                };
                var pool = poolHolder.AddComponent<EnemyPool>();
                pool.SetPrefab(enemy);

                enemyPools.Add(pool);
            }
        }

        private void InitBulletPool()
        {
            bulletPools = new List<BulletPool>();
            foreach (var bullet in bulletTypes)
            {
                var poolHolder = new GameObject($"Pool-{bullet.name}")
                {
                    transform =
                    {
                        parent = poolBulletGroup,
                        position = transform.position
                    }
                };
                var pool = poolHolder.AddComponent<BulletPool>();
                pool.SetPrefab(bullet);
                
                bulletPools.Add(pool);
            }
        }

        private void InitEnemyBulletPool()
        {
            enemyBulletPools = new List<BulletPool>();
            foreach (var bullet in enemyBulletTypes)
            {
                var poolHolder = new GameObject($"Pool-{bullet.name}")
                {
                    transform =
                    {
                        parent = poolBulletGroup,
                        position = transform.position
                    }
                };
                var pool = poolHolder.AddComponent<BulletPool>();
                pool.SetPrefab(bullet);
                
                enemyBulletPools.Add(pool);
            }
        }
    }
}