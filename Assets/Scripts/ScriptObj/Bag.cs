using System.Collections.Generic;
using UnityEngine;

namespace ScriptObj
{
    [CreateAssetMenu(fileName = "Bag", menuName = "DataBase/Bag")]
    public class Bag : ScriptableObject
    {
        public Character character;
        public List<Item> itemList;
        
        public void Init()
        {
            character = null;
            itemList.Clear();
        }
    }
}