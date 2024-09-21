using System;
using ScriptObj;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField]
        private GameStatus gameStatus;
        [SerializeField]
        private PlayerStatus playerStatus;
        [SerializeField]
        private TMP_Text goldText;

        private void Start()
        {
            goldText.SetText(playerStatus.goldValue.ToString());
        }

        public void ToFight()
        {
            var wave = gameStatus.wave;
            gameStatus.wave = wave + 1;
            SceneManager.LoadScene("Fight");
        }
    }
}