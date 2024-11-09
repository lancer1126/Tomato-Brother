using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ScriptObj
{
    [CreateAssetMenu(fileName = "GameStatus", menuName = "DataBase/GameStatus")]
    public class GameStatus : ScriptableObject
    {
        public int wave; // 第几波敌人
        public int roundTime; // 一局的默认时间
        public string overMenuText; // 暂停时的标题;
        public GameStatusEnum status; // 游戏状态

        public void Init()
        {
            wave = 1;
            overMenuText = "GAME OVER";
            status = GameStatusEnum.Playing;
        }

        public int GetRoundTime()
        {
            return roundTime != 0 ? roundTime : Random.Range(30, 46);
        }

        public int InitRoundEnemyType(int enemyPoolsCount)
        {
            var enemyTypes = wave switch
            {
                1 => 1,
                > 1 and <= 3 => 2,
                > 3 and <= 5 => 3,
                _ => enemyPoolsCount
            };

            return Math.Min(enemyTypes, enemyPoolsCount);
        }
    }

    public enum GameStatusEnum
    {
        Start,
        Playing,
        Failure,
        Victory,
        End
    }
}