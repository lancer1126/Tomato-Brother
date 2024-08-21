using System;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingUI : MonoBehaviour
    {
        [SerializeField]
        private Slider musicSlider;
        [SerializeField]
        private Slider volumeSlider;

        private void OnEnable()
        {
            musicSlider.value = Setting.Instance.GetMusic();
            volumeSlider.value = Setting.Instance.GetVolume();
        }

        /// <summary>
        /// 保存配置更改
        /// </summary>
        public void SaveSetting()
        {
            var setting = new JsonData
            {
                [SettingProp.Music.ToString()] = (int)musicSlider.value,
                [SettingProp.Volume.ToString()] = (int)volumeSlider.value
            };

            Setting.Instance.SaveJson(setting);
            gameObject.SetActive(false);
        }
    }
}