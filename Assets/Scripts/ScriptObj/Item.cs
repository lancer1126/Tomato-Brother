using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace ScriptObj
{
    [CreateAssetMenu(fileName = "Item", menuName = "DataBase/Item")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Item : ScriptableObject
    {
        public Sprite img;
        public string itemName;
        public string description;
        public ItemType itemType;
        public GameObject prefab;
    }

    public enum ItemType
    {
        Weapon,
        Robot
    }
}