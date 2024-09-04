using System.Collections.Generic;
using Pool;
using UnityEngine;

namespace System
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField]
        private int enemyBornCount = 10; // 生成敌人数量
        [SerializeField]
        private float gameTime = 30; // 游戏时间
        [SerializeField]
        private float enemyBornInterval = 3; // 敌人生成间隔
        [SerializeField]
        private float enemyBornTime; // 标记生成敌人
        private List<EnemyPool> _enemyPools; // 敌人对象池

        private void Awake()
        {
            enemyBornTime = enemyBornInterval;
        }

        private void Start()
        {
            _enemyPools = PoolController.Instance.enemyPools;
        }

        private void Update()
        {
            enemyBornTime += Time.deltaTime;
            gameTime -= Time.deltaTime;

            if (enemyBornTime > enemyBornInterval)
            {
                EnemyBorn();
                enemyBornTime = 0;
            }
        }

        private void EnemyBorn()
        {
            for (var i = 0; i < enemyBornCount; i++)
            {
                var x = UnityEngine.Random.Range(-16, 16);
                var y = UnityEngine.Random.Range(-7, 7);
                var enemyIndex = UnityEngine.Random.Range(0, 3);
            }
        }
    }
}