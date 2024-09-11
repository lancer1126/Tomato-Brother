using System;
using System.Collections.Generic;
using Pool;
using UnityEngine;

namespace Player.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        private static readonly int OpenFire = Animator.StringToHash("fire");

        [SerializeField]
        private int bulletIndex;
        [SerializeField]
        private int enemyCollisionFactor = 20;
        [SerializeField]
        private float attackRange = 10;
        [SerializeField]
        private float attackInterval = 1;
        [SerializeField]
        private float targetChangeInterval = 0.5f; // 武器瞄准重新检测最近敌人的时间间隔
        [SerializeField]
        private GameObject muzzle;
        [SerializeField]
        private AudioClip fireAudio;
        private bool _findEnemy;
        private int _enemyLayer;
        private float _lsY;
        private float _attackTimer; // 两次攻击之间的计时器
        private float _targetChangeTimer;
        private Vector3 _defaultLocalScale;
        private Vector3 _defaultRotate;
        private Animator _weaponAnimator;
        private GameObject _nearestEnemy;
        private List<BulletPool> _bulletPools;

        private void Start()
        {
            if (muzzle == null)
            {
                muzzle = transform.Find("Muzzle").gameObject;
            }

            _lsY = transform.localScale.y;
            _defaultLocalScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
            _defaultRotate = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 1);
            _weaponAnimator = GetComponent<Animator>();
            _bulletPools = PoolController.Instance.bulletPools;
            _enemyLayer = LayerMask.GetMask("Enemy");
        }

        private void FixedUpdate()
        {
            FindNearEnemy();
            WeaponRotate();
            CheckFire();
        }

        /// <summary>
        /// 隔一段时间检测周围最近的敌人
        /// </summary>
        private void FindNearEnemy()
        {
            _targetChangeTimer += Time.deltaTime;
            if (_targetChangeTimer < targetChangeInterval)
            {
                return;
            }

            // 检测最近的敌人碰撞体
            var colliders = ColliderPool.Get(enemyCollisionFactor);
            var hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, attackRange, colliders, _enemyLayer);

            // 从附近的敌人中挑选最近的
            _nearestEnemy = null;
            var shortestDir = Mathf.Infinity;
            for (var i = 0; i < hitCount; i++)
            {
                var c = colliders[i];
                var distance = Vector3.SqrMagnitude(transform.position - c.transform.position);
                if (!(distance < shortestDir))
                {
                    continue;
                }

                shortestDir = distance;
                _nearestEnemy = c.gameObject;
            }

            ColliderPool.Release(colliders);
            _findEnemy = _nearestEnemy;
            _targetChangeTimer -= targetChangeInterval;
        }

        /// <summary>
        /// 检测到周围最近的敌人后，武器转向敌人
        /// </summary>
        private void WeaponRotate()
        {
            if (_findEnemy && _nearestEnemy)
            {
                var enemyPos = _nearestEnemy.transform.position;
                var dir = (enemyPos - transform.position).normalized;
                var relativePosY = enemyPos.x < transform.position.x ? -_lsY : _lsY;

                transform.right = dir;
                transform.localScale = new Vector3(_lsY, relativePosY, 1);
            }
            else
            {
                transform.localScale = _defaultLocalScale;
                transform.localEulerAngles = _defaultRotate;
            }
        }

        /// <summary>
        /// 判断是否需要开火
        /// </summary>
        private void CheckFire()
        {
            if (!_findEnemy)
            {
                return;
            }

            // 若计时器已经大于等于攻击间隔，则进行攻击
            if (_attackTimer >= attackInterval)
            {
                Fire();
                _attackTimer -= attackInterval;
            }
            else
            {
                _attackTimer += Time.deltaTime;
            }
        }

        /// <summary>
        /// 开火
        /// </summary>
        private void Fire()
        {
            _weaponAnimator.SetTrigger(OpenFire);
            AudioSource.PlayClipAtPoint(fireAudio, muzzle.transform.position);

            var bulletIns = _bulletPools[bulletIndex].GetFromPool();
            bulletIns.transform.position = muzzle.transform.position;
            bulletIns.transform.rotation = muzzle.transform.rotation;
        }

        private static class ColliderPool
        {
            private static readonly Queue<Collider2D[]> Pool = new();

            public static Collider2D[] Get(int size)
            {
                if (Pool.Count <= 0)
                {
                    return new Collider2D[size];
                }

                var arr = Pool.Dequeue();
                return arr.Length >= size ? arr : new Collider2D[size];
            }

            public static void Release(Collider2D[] arr)
            {
                Array.Clear(arr, 0, arr.Length);
                Pool.Enqueue(arr);
            }
        }
    }
}