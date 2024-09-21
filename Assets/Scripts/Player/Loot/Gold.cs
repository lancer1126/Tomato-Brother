using Pool;
using UnityEngine;

namespace Player.Loot
{
    public class Gold : MonoBehaviour
    {
        [SerializeField]
        private GameObject player;
        [SerializeField]
        private AudioClip gainAudio;
        [SerializeField]
        private int goldValue; // 此Gold的价值
        [SerializeField]
        private float speed; // 朝玩家移动的速度
        [SerializeField]
        private float pickUpRange = 2; // 自动拾取物品的距离
        private PlayerController _playerController;

        private void Start()
        {
            if (player != null)
            {
                return;
            }

            player = GameObject.FindWithTag("Player");
            _playerController = player.GetComponent<PlayerController>();
        }

        private void Update()
        {
            CheckPickUp();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            AudioSource.PlayClipAtPoint(gainAudio, transform.position);
            _playerController.AddGold(goldValue);
            GoldPool.Instance.ReleaseFromPool(this);
        }

        public void SetGoldValue(int value)
        {
            goldValue = value;
        }

        /// <summary>
        /// 检测玩家到达Gold一定距离内就自动拾取
        /// </summary>
        private void CheckPickUp()
        {
            var toPlayDistance = player.transform.position - transform.position;
            if (!(toPlayDistance.magnitude <= pickUpRange))
            {
                return;
            }

            // 玩家到达Gold一定范围内就自动朝玩家移动
            toPlayDistance.Normalize();
            transform.position += toPlayDistance * (speed * Time.deltaTime);
        }
    }
}