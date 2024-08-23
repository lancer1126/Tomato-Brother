using UnityEngine;

namespace ScriptObj
{
    [CreateAssetMenu(fileName = "GameStatus", menuName = "DataBase/GameStatus")]
    public class GameStatus : ScriptableObject
    {
        public int wave; // 第几波敌人
        public GameStatusEnum status; // 游戏状态

        public void Init()
        {
            wave = 1;
            status = GameStatusEnum.Playing;
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