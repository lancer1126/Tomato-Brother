using System.Collections;
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
        private GameObject bornAnimation; // 敌人出生动画
        [SerializeField]
        private GameObject gameOverMenu;
        [SerializeField]
        private TMP_Text gameTimeText;
        [SerializeField]
        private TMP_Text waveText;
        [SerializeField]
        private GameStatus gameStatus;

        private const int MapScaleX = 24;
        private const int MapScaleY = 16;
        private bool _isPauseOpen;
        private int _enemyType;
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
        }

        private void Start()
        {
            _enemyPools = PoolController.Instance.enemyPools;
            _enemyType = _enemyPools.Count;
            InitWave();
            EnemyBorn();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _isPauseOpen = !_isPauseOpen;
                if (_isPauseOpen)
                {
                    GamePause();
                    return;
                }

                GameProceeding();
            }

            gameTime -= Time.deltaTime;
            if (gameTime <= 0)
            {
                NextLevel();
            }
            else
            {
                gameTimeText.text = ((int)gameTime + 1).ToString();
                gameTimeText.color = gameTime <= 10 ? Color.red : Color.white;
            }

            // 计算敌人出生
            enemyBornTime += Time.deltaTime;
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
                var enemyIndex = UnityEngine.Random.Range(0, _enemyType);
                StartCoroutine(ToBorn(enemyIndex, new Vector2(x, y)));
            }
        }

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

        private void GamePause()
        {
            gameStatus.overMenuText = "PAUSE";
            gameOverMenu.SetActive(true);
        }

        private void GameProceeding()
        {
            gameOverMenu.SetActive(false);
        }

        private void NextLevel()
        {
            gameTimeText.gameObject.SetActive(false);
            SceneManager.LoadScene("Shop");
        }

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
    }
}