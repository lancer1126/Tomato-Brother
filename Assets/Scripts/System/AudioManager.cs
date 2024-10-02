using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace System
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField]
        private int poolSize = 32;
        [SerializeField]
        private float volume = 1;
        [SerializeField]
        private AudioSource audioSourcePrefab;
        [SerializeField]
        private List<AudioClip> audioClips;

        private Vector3 defaultPos; // 未指定音频位置时的默认位置
        private List<AudioSource> playingAudioSources; // 当前正在播放的AudioSource
        private Queue<AudioSource> idleAudioSources; // 当前可用的AudioSource
        private Dictionary<string, AudioClip> audioClipDict; // 预先设置的AudioClip字典

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                // 此类跨场景，所以可能多次实例化，若检测到已有实例则不再进行实例化
                Destroy(gameObject);
                return;
            }

            Instance = this;
            defaultPos = new Vector3(0, 0, 0);

            // 跨场景保留
            DontDestroyOnLoad(gameObject);
            // 初始化音频池和音频流
            InitAudioSourcePool();
            InitAudioClipDict();
        }

        public void Play(string clipName)
        {
            Play(clipName, defaultPos);
        }

        public void Play(string clipName, Vector3 position, float clipVolume = 1f)
        {
            if (!audioClipDict.TryGetValue(clipName, out var clip))
            {
                return;
            }

            Play(clip, position, clipVolume);
        }

        public void Play(AudioClip clip)
        {
            Play(clip, defaultPos);
        }

        public void Play(AudioClip clip, Vector3 position, float clipVolume = 1f)
        {
            var source = GetAudioSource();
            if (!source)
            {
                return;
            }

            source.clip = clip;
            source.volume = clipVolume;
            source.transform.position = position;
            source.Play();
            playingAudioSources.Add(source);

            StartCoroutine(ReleaseAudioSource(source, clip.length));
        }

        /// <summary>
        /// 初始化音频池，根据poolSize新建一定数量的AudioSource
        /// </summary>
        private void InitAudioSourcePool()
        {
            playingAudioSources = new List<AudioSource>();
            idleAudioSources = new Queue<AudioSource>();
            for (var i = 0; i < poolSize; i++)
            {
                var source = Instantiate(audioSourcePrefab, transform);
                source.volume = volume;
                source.playOnAwake = false;
                source.loop = false;
                idleAudioSources.Enqueue(source);
            }
        }

        /// <summary>
        /// 读取预先提供的AudioClip，将其放置在字典中用于后续使用
        /// </summary>
        private void InitAudioClipDict()
        {
            foreach (var audioClip in audioClips)
            {
                audioClipDict.Add(audioClip.name, audioClip);
            }
        }

        /// <summary>
        /// 获取一个有效的AudioSource
        /// 把AudioSource从队列中提取出来，若队列为空则表示没有有效的可使用
        /// </summary>
        /// <returns></returns>
        private AudioSource GetAudioSource()
        {
            return idleAudioSources.Count > 0 ? idleAudioSources.Dequeue() : null;
        }

        /// <summary>
        /// 停止播放所有音频
        /// </summary>
        private void StopAll()
        {
            foreach (var source in playingAudioSources)
            {
                source.Stop();
                source.clip = null;
                idleAudioSources.Enqueue(source);
            }

            playingAudioSources.Clear();
        }

        /// <summary>
        /// 播放完音频后使用协程进行回收
        /// </summary>
        /// <param name="source"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator ReleaseAudioSource(AudioSource source, float delay)
        {
            yield return new WaitForSeconds(delay);
            source.clip = null;
            playingAudioSources.Remove(source);
            idleAudioSources.Enqueue(source);
        }
    }
}