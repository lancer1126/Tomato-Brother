using DG.Tweening;
using UnityEngine;

namespace Player.Weapon.Projectile
{
    public class ShurikenBullet : BulletPlayer
    {
        private Tweener _rotationTweener;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            StartRotation();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StopRotation();
        }
        
        private void StartRotation()
        {
            // 设置每0.3秒旋转360度
            _rotationTweener = transform
                .DORotate(new Vector3(0, 0, 360), 0.3f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Incremental); // -1 表示无限循环
        }

        private void StopRotation()
        {
            _rotationTweener?.Kill();
        }
    }
}