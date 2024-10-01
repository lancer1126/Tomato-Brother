using UnityEngine;

namespace ScriptObj
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "DataBase/Weapon")]
    public class Weapon : ScriptableObject
    {
        public string WeaponName;
        public Sprite weaponImg;
        public GameObject weaponPrefab;
    }
}