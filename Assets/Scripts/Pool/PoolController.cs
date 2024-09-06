using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace Pool
{
    public class PoolController : MonoBehaviour
    {
        public static PoolController Instance { get; set; }

        public List<EnemyPool> enemyPools;

        [SerializeField]
        private BaseEnemy[] enemyList;

        private void Awake()
        {
            Instance = this;
            InitEnemyPool();
        }

        private void InitEnemyPool()
        {
            enemyPools = new List<EnemyPool>();
            // 遍历敌人列表，为每种敌人生成一个对象池，将其挂载到pool-enemy之下
            foreach (var enemy in enemyList)
            {
                var poolHolder = new GameObject($"Pool-{enemy.name}")
                {
                    transform =
                    {
                        parent = transform.Find("Pool-Enemy"),
                        position = transform.position
                    }
                };
                var pool = poolHolder.AddComponent<EnemyPool>();
                pool.SetPrefab(enemy);

                enemyPools.Add(pool);
            }
        }
    }
}