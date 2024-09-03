using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace Pool
{
    public class PoolController : MonoBehaviour
    {
        public static PoolController Instance { get; set; }

        [SerializeField]
        private BaseEnemy[] enemyList;
        private List<EnemyPool> _enemyPools;

        private void Awake()
        {
            InitEnemyPool();
        }

        private void InitEnemyPool()
        {
            // 遍历敌人列表，为每种敌人生成一个对象池，将其挂载到pool-enemy之下
            foreach (var enemy in enemyList)
            {
                var poolHolder = new GameObject($"pool-{enemy.name}")
                {
                    transform =
                    {
                        parent = transform.Find("pool-enemy"),
                        position = transform.position
                    }
                };
                var pool = poolHolder.AddComponent<EnemyPool>();
                pool.SetPrefab(enemy);
                
                _enemyPools.Add(pool);
            }
        }
    }
}