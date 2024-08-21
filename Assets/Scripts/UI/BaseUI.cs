using System;
using LitJson;
using UnityEngine;

namespace UI
{
    public class BaseUI : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = Setting.Instance.GetVolume() / 100f;
            Setting.OnSettingChanged += ChangeSetting;
        }

        private void OnDestroy()
        {
            Setting.OnSettingChanged -= ChangeSetting;
        }

        private void ChangeSetting(JsonData obj)
        {
            _audioSource.volume = (int)obj[SettingProp.Volume.ToString()] / 100f;
        }
    }
}