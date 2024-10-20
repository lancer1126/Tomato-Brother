using Pool;
using UnityEngine;

namespace Enemy
{
    public class Bird : BaseEnemy
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            forward = GetRandomV2();
        }

        protected override void FixedUpdate()
        {
            MoveToPlayer();
            // 远程敌人，不需要触碰到才攻击，隔一段时间就进行攻击
            if (AttackTimer > attackInterval)
            {
                RemoteAttack();
                AttackTimer = 0;
            }
            else
            {
                AttackTimer += Time.deltaTime;
            }
        }

        protected override void OnTriggerEnter2D(Collider2D other) { }

        protected override void OnTriggerStay2D(Collider2D other) { }

        protected override void Move()
        {
            if (ToPlayerDir.magnitude > 5)
            {
                Rb2.velocity = forward * (moveSpeed * Time.deltaTime);
            }
            else
            {
                forward = (transform.position - Player.transform.position).normalized;
            }
        }

        private void RemoteAttack() { }
    }
}