using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private TMP_Text currentHealthText;
        [SerializeField]
        private TMP_Text maxHealthText;

        public void SetCurrentHealth(int health)
        {
            slider.value = health;
            currentHealthText.SetText(health.ToString());
        }

        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            maxHealthText.SetText(maxHealth.ToString());
        }
    }
}