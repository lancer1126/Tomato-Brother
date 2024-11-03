using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            // 跨场景保留
            DontDestroyOnLoad(gameObject);
        }

        public void Load(string sceneName)
        {
            StartCoroutine(LoadAfter(sceneName));
        }

        private static IEnumerator LoadAfter(string sceneName)
        {
            DOTween.KillAll(true);
            yield return null;
            var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            if (asyncLoad == null) yield break;
            
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}