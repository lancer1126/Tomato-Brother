using System.Collections.Generic;
using Enemy;
using Player.Weapon.Projectile;
using Player.Weapon.Ranged;
using ScriptObj;
using UnityEngine;

namespace Pool
{
    public class PoolController : MonoBehaviour
    {
        public static PoolController Instance { get; private set; }

        public List<EnemyPool> enemyPools;
        public Dictionary<string, BulletPool> BulletDict;
        public Dictionary<string, BulletPool> EnemyBulletDict;

        [SerializeField]
        private Bag playerBag;
        [SerializeField]
        private Transform poolEnemyGroup;
        [SerializeField]
        private Transform poolBulletGroup;
        [SerializeField]
        private BaseEnemy[] enemyTypes;
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
            InitBulletPool();
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
            BulletDict = new Dictionary<string, BulletPool>();
            foreach (var weapon in playerBag.weaponList)
            {
                var rangedWeapon = weapon.prefab.GetComponent<RangedWeapon>();
                if (!rangedWeapon)
                {
                    continue;
                }

                var bullet = rangedWeapon.bullet;
                if (!bullet)
                {
                    continue;
                }

                var poolHolder = new GameObject($"Pool-{weapon.name}")
                {
                    transform =
                    {
                        parent = poolBulletGroup,
                        position = transform.position
                    }
                };
                var pool = poolHolder.AddComponent<BulletPool>();
                pool.SetPrefab(bullet);

                BulletDict.Add(weapon.itemName, pool);
            }
        }

        private void InitEnemyBulletPool()
        {
            EnemyBulletDict = new Dictionary<string, BulletPool>();
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

                EnemyBulletDict.Add(bullet.name, pool);
            }
        }
    }
}