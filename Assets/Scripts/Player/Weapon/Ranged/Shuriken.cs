using UnityEngine;

namespace Player.Weapon.Ranged
{
    public class Shuriken : RangedWeapon
    {
        [Header("手里剑")]
        [SerializeField]
        private float rebornInterval = 0.5f; // 手里剑刷新时间
        
        private bool _launched; // 手里剑是否已发射
        private float _rebornTimer; // 手里剑刷新计时器

        protected override void Start()
        {
            transform.localScale = new Vector3(0, 0, 0);
            base.Start();
        }

        protected override void FixedUpdate()
        {
            var proceed = true;
            if (_launched)
            {
                if (_rebornTimer < rebornInterval)
                {
                    _rebornTimer += Time.deltaTime;
                    proceed = false;
                }
                else
                {
                    _launched = false;
                    transform.localScale = DefaultLocalScale;
                }
            }

            if (!proceed) return;

            base.FixedUpdate();
        }
    }
}