using UnityEngine;

namespace ScriptObj
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "DataBase/Weapon")]
    public class Weapon : ScriptableObject
    {
        public Sprite weaponImg;
        public GameObject weaponPrefab;
    }
}