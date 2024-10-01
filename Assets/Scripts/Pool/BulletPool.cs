using Player.Weapon.Projectile;
using UnityEngine;

namespace Pool
{
    public class BulletPool : BasePool<Bullet>
    {
        private Transform storeTo;

        private void Start()
        {
            InitPool();
        }

        /// <summary>
        /// 设置子弹对象生成的父对象
        /// </summary>
        /// <param name="t"></param>
        public void SetStoreTo(Transform t)
        {
            storeTo = t;
        }

        protected override Bullet ToCreate()
        {
            var ins = Instantiate(prefab, transform);
            ins.SetDeactivateAction(delegate { ToRelease(ins); });

            return ins;
        }

        protected override void ToRelease(Bullet obj)
        {
            base.ToRelease(obj);
            obj.transform.position = new Vector3(-40, 0);
        }
    }
}