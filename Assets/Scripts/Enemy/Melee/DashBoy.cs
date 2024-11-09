using DG.Tweening;
using UnityEngine;

namespace Enemy.Melee
{
    public class DashBoy : MeleeEnemy
    {
        [Header("Dash")]
        [SerializeField]
        private float dashDistance = 3f; // 冲锋距离
        [SerializeField]
        private float dashDuration = 0.5f; // 冲锋持续时间
        [SerializeField]
        private float dashInterval = 3f; // 多长时间冲锋一次

        private bool _isDashing;
        private float _dashTimer;

        protected override void Update()
        {
            base.Update();
            Dash();
        }

        private void Dash()
        {
            if (_dashTimer < dashInterval)
            {
                _dashTimer += Time.deltaTime;
                return;
            }
            // 有1/2的概率冲锋
            var randomNum = Random.Range(0, 2);
            if (randomNum != 1)
            {
                _dashTimer = 0;
                return;
            }
            
            var startPos = transform.position;
            transform.DOMove(startPos + forward * dashDistance, dashDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => { _isDashing = false; });

            _isDashing = true;
            _dashTimer = 0;
        }
    }
}