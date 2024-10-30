using System;
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

        private RoundShop _roundShop;
        private Item _item;

        private void Start()
        {
            _roundShop = GameObject.FindWithTag("RoundShop").GetComponent<RoundShop>();
        }

        public void Init(Item item)
        {
            iconImg.sprite = item.img;
            itemName.text = item.itemName;
            description.text = item.description;
            _item = item;
        }

        public void ChooseItem()
        {
            _roundShop.AddItemToBag(_item);
            gameObject.SetActive(false);
        }
    }
}