using UnityEngine;

namespace System
{
    public class Menu : MonoBehaviour
    {
        public GameObject settingUI;

        private void Awake()
        {
            settingUI.SetActive(false);
        }

        public void StartGame()
        {
            Debug.Log("开始游戏");
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