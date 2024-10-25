using ScriptObj;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    public class UpgradeItem : MonoBehaviour
    {
        [SerializeField]
        private Image iconImg;
        [SerializeField]
        private TMP_Text itemName;
        [SerializeField]
        private TMP_Text description;
        [SerializeField]
        private Button button;

        public void Init(Item item)
        {
            iconImg.sprite = item.img;
            itemName.text = item.itemName;
            description.text = item.description;
        }
    }
}