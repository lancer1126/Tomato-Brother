using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        public void SetCurrentHealth(float health)
        {
            slider.value = health;
          
        }

        public void SetMaxHealth(float maxHealth)
        {
            slider.maxValue = maxHealth;
        }
    }
}