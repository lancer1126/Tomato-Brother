using UnityEngine;

namespace Enemy
{
    public class Slime : BaseEnemy
    {
        private const float ChildScaleFactor = 0.7f; // 生成的敌人大小相对于母体的比例

        [SerializeField]
        private int bornChildCount; // 死亡后生成子敌人

        protected override void Die()
        {
            ReleaseAction.Invoke();
            if (bornLevel < 1)
            {
                BornChild();
            }
            else
            {
                IsDead = true;
                CheckLoot(2);
            }
        }

        private void BornChild()
        {
            var childScale = new Vector2(
                transform.localScale.x * ChildScaleFactor, transform.localScale.y * ChildScaleFactor
            );

            var basePos = transform.position;
            for (var i = 0; i < bornChildCount; i++)
            {
                var child = GetAction.Invoke();
                var randomX = Random.Range(-2, 2);
                child.transform.position = basePos + new Vector3(randomX, 0, 0);
                child.transform.localScale = childScale;
                child.currentHealth = maxHealth / 2;
                child.bornLevel++;
            }
        }
    }
}