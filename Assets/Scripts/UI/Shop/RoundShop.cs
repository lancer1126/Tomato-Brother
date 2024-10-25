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
        
        private void OnEnable()
        {
            ClearItem();
            InitItem();
        }

        private void InitItem()
        {
            foreach (var weapon in shopProduct.weaponList)
            {
                var item = Instantiate(upgradeItem, upgradeObj.transform);
                item.GetComponent<UpgradeItem>().Init(weapon);
            }
        }

        private void ClearItem()
        {
            for (var i = upgradeObj.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(upgradeObj.transform.GetChild(i));
            }
        }
    }
}