using ScriptObj;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System
{
    public class Menu : MonoBehaviour
    {
        public Bag bag;
        public GameObject settingUI;
        public GameStatus gameStatus;
        public PlayerStatus playerStatus;

        private void Awake()
        {
            settingUI.SetActive(false);
        }

        public void StartGame()
        {
            bag.Init();
            gameStatus.Init();
            playerStatus.Init();
            Time.timeScale = 1;
            SceneManager.LoadScene("Select");
        }

        public void OpenSetting()
        {
            Debug.Log("打开设置");
            settingUI.SetActive(true);
        }

        public void QuitGame()
        {
            Debug.Log("退出游戏");
            Application.Quit();
        }
    }
}