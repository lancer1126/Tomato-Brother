using Player.Weapon.Projectile;
using UnityEngine;

namespace Pool
{
    public class BulletPool : BasePool<Bullet>
    {
        private void Start()
        {
            InitPool();
        }

        protected override Bullet ToCreate()
        {
            var ins = Instantiate(prefab, transform);
            ins.transform.SetParent(transform, true);
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