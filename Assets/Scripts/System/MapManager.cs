﻿using System.Collections;
using System.Collections.Generic;
using Pool;
using ScriptObj;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField]
        private int enemyBornCount = 10; // 批次生成敌人数量
        [SerializeField]
        private float gameTime = 30; // 一局游戏时间
        [SerializeField]
        private float enemyBornInterval = 3; // 敌人生成间隔
        [SerializeField]
        private float enemyBornTime; // 标记生成敌人
        [SerializeField]
        private float clockStartTime = 5; // 倒计时声音开始的节点
        [SerializeField]
        private GameObject bornAnimation; // 敌人出生动画
        [SerializeField]
        private GameObject gameOverMenu; // 游戏结束菜单
        [SerializeField]
        private GameObject shopObj; // 商店页面
        [SerializeField]
        private TMP_Text gameTimeText; // 游戏倒计时文本
        [SerializeField]
        private TMP_Text waveText; // 当前第几波敌人
        [SerializeField]
        private GameStatus gameStatus; // 游戏状态
        [SerializeField]
        private AudioClip shopClip; // 打开商店页面的音乐
        [SerializeField]
        private AudioClip clockClip; // 倒计时的声音

        private const int MapScaleX = 24;
        private const int MapScaleY = 16;
        private bool _isPauseOpen;
        private bool _shopOpened;
        private int _enemyType;
        private float _clockTimer;
        private AudioSource _bgm;
        private List<EnemyPool> _enemyPools; // 敌人对象池

        private void Awake()
        {
            enemyBornTime = enemyBornInterval;
            _bgm = GetComponent<AudioSource>();
            if (gameStatus.wave == 0)
            {
                gameStatus.wave = 1;
            }

            Time.timeScale = 1;
        }

        private void Start()
        {
            _enemyPools = PoolController.Instance.enemyPools;
            _enemyType = _enemyPools.Count;
            _bgm.ignoreListenerPause = true;
            InitWave();
            EnemyBorn();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CheckKeyEsc();
            }

            gameTime -= Time.deltaTime;
            if (gameTime <= 0)
            {
                if (!_shopOpened)
                {
                    RoundOver();
                }
            }
            else
            {
                RoundUpdate();
            }
        }

        /// <summary>
        /// 加载下一轮游戏
        /// </summary>
        public void NextRound()
        {
            gameStatus.wave += 1;
            shopObj.SetActive(false);
            SceneManager.LoadScene("Fight");
        }

        /// <summary>
        /// 初始化这一轮游戏
        /// </summary>
        private void InitWave()
        {
            if (gameTime == 0)
            {
                gameTime = 30;
            }

            enemyBornCount *= gameStatus.wave;
            if (enemyBornCount > 20)
            {
                enemyBornCount = 20;
            }

            enemyBornInterval += gameStatus.wave - 1;
            if (enemyBornInterval > 5)
            {
                enemyBornInterval = 5;
            }

            waveText.text = "第" + gameStatus.wave + "轮";
        }

        /// <summary>
        /// 按下Esc键后的逻辑
        /// </summary>
        private void CheckKeyEsc()
        {
            _isPauseOpen = !_isPauseOpen;
            if (_isPauseOpen)
            {
                GamePause();
                return;
            }

            // 继续游戏
            gameOverMenu.SetActive(false);
        }

        /// <summary>
        /// 计算敌人定时出生
        /// </summary>
        private void EnemyBorn()
        {
            for (var i = 0; i < enemyBornCount; i++)
            {
                // 敌人生成范围
                var x = UnityEngine.Random.Range(-MapScaleX, MapScaleX);
                var y = UnityEngine.Random.Range(-MapScaleY, MapScaleY);

                // 敌人种类索引，按照索引的顺序生成不同种类的敌人
                var enemyIndex = UnityEngine.Random.Range(0, _enemyType);
                StartCoroutine(ToBorn(enemyIndex, new Vector2(x, y)));
            }
        }

        /// <summary>
        /// 游戏暂停后的逻辑
        /// </summary>
        private void GamePause()
        {
            gameStatus.overMenuText = "PAUSE";
            gameOverMenu.SetActive(true);
        }

        /// <summary>
        /// 单局游戏Update逻辑
        /// </summary>
        private void RoundUpdate()
        {
            gameTimeText.text = ((int)gameTime + 1).ToString();
            gameTimeText.color = gameTime <= 10 ? Color.red : Color.white;
            if (gameTime <= clockStartTime + 1)
            {
                if (_clockTimer >= 1f)
                {
                    // 播放倒计时音效
                    AudioManager.Instance.Play(clockClip, 0.5f);
                    _clockTimer = 0;
                }
                _clockTimer += Time.deltaTime;
            }

            // 计算敌人出生
            enemyBornTime += Time.deltaTime;
            if (enemyBornTime > enemyBornInterval)
            {
                EnemyBorn();
                enemyBornTime = 0;
            }
        }

        /// <summary>
        /// 使用协程异步生成敌人
        /// </summary>
        /// <param name="enemyIndex"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private IEnumerator ToBorn(int enemyIndex, Vector2 position)
        {
            // 敌人生成动画持续3秒
            var bornSign = Instantiate(bornAnimation, position, Quaternion.identity);
            yield return new WaitForSeconds(enemyBornInterval);

            if (bornSign)
            {
                Destroy(bornSign);
            }

            // 3秒加载动画后生成敌人
            var enemyInstance = _enemyPools[enemyIndex].GetFromPool();
            enemyInstance.transform.position = position;
        }

        /// <summary>
        /// 单局游戏结束
        /// </summary>
        private void RoundOver()
        {
            _shopOpened = true;
            _bgm.clip = null;
            AudioManager.Instance.Play(shopClip, transform.position, 0.3f);
            gameTimeText.gameObject.SetActive(false);
            shopObj.SetActive(true);

            Time.timeScale = 0;
        }
    }
}