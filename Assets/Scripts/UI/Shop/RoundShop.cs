using System.Collections.Generic;
using System.Linq;
using ScriptObj;
using UnityEngine;

namespace UI.Shop
{
    public class RoundShop : MonoBehaviour
    {
        [SerializeField]
        private ShopProduct shopProduct;
        [SerializeField]
        private GameObject upgradeObj;
        [SerializeField]
        private GameObject upgradeItem;
        [SerializeField]
        private Bag playerBag;

        private void OnEnable()
        {
            ClearItem();
            InitItem();
        }

        /// <summary>
        /// 将选择的升级项添加到背包中
        /// </summary>
        /// <param name="item"></param>
        public void AddItemToBag(Item item)
        {
            if (item.itemType == ItemType.Weapon)
            {
                AddWeapon(item);
            }
        }

        private void InitItem()
        {
            var existWeapons = playerBag.weaponList.Select(w => w.itemName).ToList();
            foreach (var weapon in shopProduct.weaponList)
            {
                if (existWeapons.Contains(weapon.itemName))
                {
                    continue;
                }

                var upItem = Instantiate(upgradeItem, upgradeObj.transform);
                upItem.GetComponent<UpgradeItem>().Init(weapon);
            }
        }

        private void ClearItem()
        {
            for (var i = upgradeObj.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(upgradeObj.transform.GetChild(i));
            }
        }

        private void AddWeapon(Item item)
        {
            playerBag.weaponList.Add((Weapon)item);
        }

        private string GenUniqueName(string originalName, List<string> existingNames)
        {
            var candidate = originalName;
            var suffix = 1;
            while (existingNames.Contains(candidate))
            {
                candidate = $"{originalName}_{suffix}";
                suffix++;
            }

            return candidate;
        }
    }
}