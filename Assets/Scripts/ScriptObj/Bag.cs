using System.Collections.Generic;
using UnityEngine;

namespace ScriptObj
{
    [CreateAssetMenu(fileName = "Bag", menuName = "DataBase/Bag")]
    public class Bag : ScriptableObject
    {
        public Character character;
        public List<Weapon> weaponList;
        
        public void Init()
        {
            character = null;
            weaponList.Clear();
        }
    }
}