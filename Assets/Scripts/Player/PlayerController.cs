using System.Collections.Generic;
using ScriptObj;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash("isMoving");

        [SerializeField]
        private Bag playerBag;
        [SerializeField]
        private PlayerStatus playerStatus;
        [SerializeField]
        private HealthBar healthBar;
        [SerializeField]
        private AudioClip hurtAudio;
        [SerializeField]
        private List<Vector2> weaponPosList;
        private bool _isDead;
        private float _speed;
        private Vector3 _moveDir;
        private Animator _animator;
        private Rigidbody2D _rb2;

        private void Start()
        {
            _speed = playerStatus.speed;
            _rb2 = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            LoadCharacter();
            LoadPlayerWeapon();
            InitStatusBar();
        }

        private void FixedUpdate()
        {
            Move();
        }

        public void TakeDamage(float damage)
        {
            var remainHealth = playerStatus.health - damage;
            playerStatus.health = remainHealth <= 0 ? 0 : remainHealth;
            healthBar.SetCurrentHealth(playerStatus.health);
            
            if (playerStatus.health == 0)
            {
                PlayerDie();
            }
        }

        /// <summary>
        /// 人物移动
        /// </summary>
        private void Move()
        {
            if (_isDead)
            {
                return;
            }
            
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
            _moveDir = new Vector3(h, v, 0).normalized;
            if (_moveDir != Vector3.zero)
            {
                _animator.SetBool(IsMoving, true);
                // 转向
                transform.rotation = h switch
                {
                    > 0 => Quaternion.Euler(0, 0, 0),
                    < 0 => Quaternion.Euler(0, 180, 0),
                    _ => transform.rotation
                };

                _rb2.velocity = _moveDir * (_speed * Time.deltaTime);
            }
            else
            {
                _animator.SetBool(IsMoving, false);
                _rb2.velocity = Vector2.zero;
            }
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

        private void InitStatusBar()
        {
            playerStatus.health = playerStatus.maxHealth;
            healthBar.SetCurrentHealth(playerStatus.health);
            healthBar.SetMaxHealth(playerStatus.maxHealth);
        }

        private void PlayerDie()
        {
            _isDead = true;
            _rb2.velocity = Vector2.zero;
            Time.timeScale = 0;
        }
    }
}