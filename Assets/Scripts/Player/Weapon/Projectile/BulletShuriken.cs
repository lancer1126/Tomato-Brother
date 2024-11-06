using DG.Tweening;
using Enemy;
using UnityEngine;

namespace Player.Weapon.Projectile
{
    public class BulletShuriken : BulletPlayer
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

        protected override void CollideToEnemy(Collider2D other)
        {
            if (CurPenetration <= 0)
            {
                return;
            }

            CurPenetration--;
            StopRotation();
            transform.right = Forward;
            
            var enemy = other.gameObject.GetComponent<BaseEnemy>();
            enemy.TakeDamage(transform, Damage, RepelPower);

            if (CurPenetration <= 0)
            {
                BulletEnd();
            }
            else
            {
                // 每穿过一个敌人伤害就减半
                Damage /= 2;
            }
        }
        
        private void StartRotation()
        {
            // 设置每0.3秒旋转360度
            _rotationTweener = transform
                .DORotate(new Vector3(0, 0, 360), 0.2f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Incremental); // -1 表示无限循环
        }

        private void StopRotation()
        {
            _rotationTweener?.Kill();
        }
    }
}