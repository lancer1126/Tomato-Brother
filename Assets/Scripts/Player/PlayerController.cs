using System.Collections.Generic;
using ScriptObj;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float speed = 300f;
        [SerializeField]
        private Bag playerBag;
        [SerializeField]
        private PlayerStatus playerStatus;
        [SerializeField]
        private List<Vector2> weaponPosList;

        private static readonly int IsMoving = Animator.StringToHash("isMoving");

        private Vector3 _moveDir;
        private Animator _animator;
        private Rigidbody2D _rb2;

        private void Start()
        {
            _rb2 = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            LoadCharacter();
            LoadPlayerWeapon();
        }

        private void FixedUpdate()
        {
            Move();
        }

        /// <summary>
        /// 人物移动
        /// </summary>
        private void Move()
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
            _moveDir = new Vector3(h, v, 0).normalized;
            if (_moveDir != Vector3.zero)
            {
                _animator.SetBool(IsMoving, true);
                var tls = transform.localScale;
                // 检查水平输入方向是否与当前物体方向不同，若不同则翻转
                if (!Mathf.Approximately(Mathf.Sign(h), Mathf.Sign(tls.x)))
                {
                    Flip(tls);
                }

                _rb2.velocity = _moveDir * (speed * Time.deltaTime);
            }
            else
            {
                _animator.SetBool(IsMoving, false);
                _rb2.velocity = Vector2.zero;
            }
        }

        /// <summary>
        /// 人物转向
        /// </summary>
        /// <param name="localScale"></param>
        private void Flip(Vector3 localScale)
        {
            transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        }

        /// <summary>
        /// 加载角色动画
        /// </summary>
        private void LoadCharacter()
        {
            if (playerBag.character)
            {
                _animator.runtimeAnimatorController = playerBag.character.characterAnimator;
            }
        }

        /// <summary>
        /// 加载武器并初始化武器位置
        /// </summary>
        private void LoadPlayerWeapon()
        {
            for (var i = 0; i < playerBag.weaponList.Count; i++)
            {
                var weapon = Instantiate(playerBag.weaponList[i].weaponPrefab, transform);
                weapon.transform.localPosition = weaponPosList[i];
            }
        }
    }
}