using System;
using System.Collections;
using DG.Tweening;
using Enemy;
using UnityEngine;

namespace Player.Weapon.Melee
{
    public class MeleeWeapon : BaseWeapon
    {
        protected const float AttackReturnDuration = 0.2f;

        [SerializeField]
        protected float meleeAttackDistance;
        [SerializeField]
        protected float meleeAttackDuration;
        protected bool IsAttacking;

        protected override void FixedUpdate()
        {
            if (IsAttacking)
            {
                return;
            }

            base.FixedUpdate();
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            // 仅在攻击时有效
            if (!other.gameObject.CompareTag("Enemy") || !IsAttacking)
            {
                return;
            }

            // 击中敌人时给敌人造成伤害
            var enemy = other.gameObject.GetComponent<BaseEnemy>();
            enemy.TakeDamage(transform, damage, repelPower);
        }

        protected override void Attack()
        {
            if (!AttackTarget || IsAttacking)
            {
                return;
            }

            if (AttackTimer < attackInterval)
            {
                AttackTimer += Time.deltaTime;
                return;
            }

            MeleeAttack();
            AttackTimer = 0;
        }

        /// <summary>
        /// 近战攻击
        /// </summary>
        protected virtual void MeleeAttack()
        {
            AudioManager.Instance.Play(attackAudio, transform.position, 0.2f);
            StartCoroutine(AttackCoroutine());
        }

        /// <summary>
        /// 使用协程来执行近战武器攻击动画
        /// </summary>
        /// <returns></returns>
        private IEnumerator AttackCoroutine()
        {
            IsAttacking = true;
            var attackDir = (AttackTarget.transform.position - transform.position).normalized;
            if (!PlayerFacingRight())
            {
                attackDir.x = -attackDir.x;
            }

            // 武器最终要攻击到的位置
            var attackEndPos = OriginalLocalPos + attackDir * meleeAttackDistance;

            // 武器出击
            yield return transform
                .DOLocalMove(attackEndPos, meleeAttackDuration)
                .SetEase(Ease.OutQuad)
                .WaitForCompletion();

            // 返回原位置
            yield return transform
                .DOLocalMove(OriginalLocalPos, AttackReturnDuration)
                .SetEase(Ease.InOutQuad)
                .WaitForCompletion();
            
            IsAttacking = false;
        }
    }
}