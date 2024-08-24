using System.Collections.Generic;
using UnityEngine;

namespace ScriptObj
{
    [CreateAssetMenu(fileName = "ShopProduct", menuName = "DataBase/ShopProduct")]
    public class ShopProduct : ScriptableObject
    {
        public List<Character> characterList;
        public List<Weapon> weaponList;
    }
}