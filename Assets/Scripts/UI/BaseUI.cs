using System;
using UnityEngine;

namespace UI
{
    public class BaseUI : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            // _audioSource.volume = Setting.Instance.GetVolume();
        }
    }
}