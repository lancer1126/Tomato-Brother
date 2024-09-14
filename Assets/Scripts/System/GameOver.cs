using ScriptObj;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System
{
    public class GameOver : MonoBehaviour
    {

        [SerializeField]
        private TMP_Text title;
        [SerializeField]
        private GameStatus gameStatus;

        private void OnEnable()
        {
            ToActiveMenu();
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
        }

        public void ToMainMenu()
        {
            SceneManager.LoadScene("Start");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void ToActiveMenu()
        {
            Time.timeScale = 0;
            title.text =  gameStatus.overMenuText;
        }
    }
}