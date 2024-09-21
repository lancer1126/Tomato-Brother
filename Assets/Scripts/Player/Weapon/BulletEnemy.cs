using UnityEngine;

namespace Player.Weapon
{
    public class BulletEnemy : Bullet
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                ReleaseAction.Invoke();
                return;
            }

            other.gameObject.GetComponent<PlayerController>()?.TakeDamage(damage);
            ReleaseAction.Invoke();
        }
    }
}