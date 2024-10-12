using System;
using System.Collections.Generic;
using Cinemachine;
using Player.Weapon;
using Pool;
using ScriptObj;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }
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
        private GameObject gameOverMenu;
        [SerializeField]
        private GameStatus gameStatus;
        [SerializeField]
        private TMP_Text goldText;
        [SerializeField]
        private List<Vector2> weaponPosList;

        private bool _isFacingRight = true;
        private bool _isDead;
        private float _speed;
        private Vector3 _moveDir;
        private Animator _animator;
        private Rigidbody2D _rb2;
        private CinemachineImpulseSource _impulseSource;

        private void Awake()
        {
            Instance = this;
            _rb2 = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        private void Start()
        {
            _speed = playerStatus.speed;
            LoadCharacter();
            LoadWeapon();
            InitStatusBar();
        }

        private void FixedUpdate()
        {
            Move();
        }

        public void TakeDamage(float damage)
        {
            AudioManager.Instance.Play(hurtAudio, transform.position);
            // 受攻击时镜头抖动
            _impulseSource.GenerateImpulse(0.2f);
            // 出现生命减少提示
            TextPool.Instance.GetText(transform.position, "-" + damage, 6);

            var remainHealth = playerStatus.health - damage;
            playerStatus.health = remainHealth <= 0 ? 0 : remainHealth;
            healthBar.SetCurrentHealth(playerStatus.health);

            if (playerStatus.health == 0)
            {
                PlayerDie();
            }
        }

        /// <summary>
        /// 拾取金币
        /// </summary>
        public void AddGold(int value)
        {
            playerStatus.goldValue += value;
            goldText.SetText(playerStatus.goldValue.ToString());
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
                // 切换到移动动画
                _animator.SetBool(IsMoving, true);
                // 转向
                var curFacingRight = h > 0;
                if (_isFacingRight != curFacingRight)
                {
                    Flip();
                }

                // 移动
                _rb2.velocity = _moveDir * _speed;
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
        private void LoadWeapon()
        {
            for (var i = 0; i < playerBag.weaponList.Count; i++)
            {
                var weapon = Instantiate(playerBag.weaponList[i].weaponPrefab, transform);
                weapon.transform.localPosition = weaponPosList[i];
                var baseWeapon = weapon.GetComponent<BaseWeapon>();
                baseWeapon.weaponIndex = i;
                baseWeapon.weaponName = playerBag.weaponList[i].WeaponName;
            }
        }

        /// <summary>
        /// 初始化状态条
        /// </summary>
        private void InitStatusBar()
        {
            playerStatus.health = playerStatus.maxHealth;
            healthBar.SetCurrentHealth(playerStatus.health);
            healthBar.SetMaxHealth(playerStatus.maxHealth);
            goldText.SetText(playerStatus.goldValue.ToString());
        }

        /// <summary>
        /// 玩家死亡
        /// </summary>
        private void PlayerDie()
        {
            _isDead = true;
            _rb2.velocity = Vector2.zero;
            gameStatus.overMenuText = "GAME OVER";
            gameOverMenu.SetActive(true);
        }

        /// <summary>
        /// 翻转方向
        /// </summary>
        private void Flip()
        {
            _isFacingRight = !_isFacingRight;
            var originLs = transform.localScale;
            originLs.x *= -1;
            transform.localScale = originLs;
        }
    }
}