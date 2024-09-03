using UnityEngine;

namespace Enemy
{
    public class BaseEnemy : MonoBehaviour
    {
        public int level;
        public int attack;
        public float moveSpeed;
        public float maxHealth;
        public float currentHealth;
        public Vector2 forward;
        
        private bool _isHurt;
        private bool _isDead;
    }
}