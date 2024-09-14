using ScriptObj;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField]
        private GameStatus gameStatus;

        public void ToFight()
        {
            var wave = gameStatus.wave;
            gameStatus.wave = wave + 1;
            SceneManager.LoadScene("Fight");
        }
    }
}