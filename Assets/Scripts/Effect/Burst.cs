using System;
using System.Collections;
using UnityEngine;

namespace Effect
{
    public class Burst : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem burstParticle;
        private Action _releaseAction;

        private void Awake()
        {
            burstParticle = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            StartCoroutine(RecycleBurst(burstParticle.main.duration));
        }

        public void Play()
        {
            burstParticle.Play();
        }
        
        public void SetDeactivateAction(Action releaseObj)
        {
            _releaseAction = releaseObj;
        }

        /// <summary>
        /// 在一定时间后进行回收
        /// </summary>
        /// <returns></returns>
        private IEnumerator RecycleBurst(float duration = 0.3f)
        {
            yield return new WaitForSeconds(duration);
            _releaseAction.Invoke();
        }
    }
}