﻿using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;

namespace System
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField]
        private int enemyBornCount = 10; // 批次生成敌人数量
        [SerializeField]
        private int enemySpecies = 1; // 敌人种类数
        [SerializeField]
        private float gameTime = 30; // 游戏时间
        [SerializeField]
        private float enemyBornInterval = 3; // 敌人生成间隔
        [SerializeField]
        private float enemyBornTime; // 标记生成敌人
        [SerializeField]
        private GameObject bornAnimation;

        private const int MapScaleX = 32;
        private const int MapScaleY = 16;
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
                // 敌人生成范围
                var x = UnityEngine.Random.Range(-MapScaleX, MapScaleX);
                var y = UnityEngine.Random.Range(-MapScaleY, MapScaleY);
                
                // 敌人种类索引，按照索引的顺序生成不同种类的敌人
                var enemyIndex = UnityEngine.Random.Range(0, enemySpecies - 1);
                StartCoroutine(ToBorn(enemyIndex, new Vector2(x, y)));
            }
        }

        private IEnumerator ToBorn(int enemyIndex, Vector2 position)
        {
            // 敌人生成动画持续3秒
            var bornSign = Instantiate(bornAnimation, position, Quaternion.identity);
            yield return new WaitForSeconds(3);

            if (bornSign)
            {
                Destroy(bornSign);
            }

            // 3秒加载动画后生成敌人
            var enemyInstance = _enemyPools[enemyIndex].GetFromPool();
            enemyInstance.transform.position = position;
        }
    }
}