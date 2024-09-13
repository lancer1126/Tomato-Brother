using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System
{
    public class GameOver : MonoBehaviour
    {

        [SerializeField]
        private TMP_Text title;

        private void OnEnable()
        {
            ToActiveMenu();
        }

        public void ToMainMenu()
        {
            SceneManager.LoadScene("Start");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void ToActiveMenu(bool isWin = false)
        {
            Time.timeScale = 0;
            title.text = isWin ? "VICTORY" : "GAME OVER";
        }
    }
}