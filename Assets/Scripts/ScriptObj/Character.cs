using UnityEditor.Animations;
using UnityEngine;

namespace ScriptObj
{
    [CreateAssetMenu(fileName = "Character", menuName = "DataBase/Character")]
    public class Character : ScriptableObject
    {
        public string characterName;
        public Sprite characterImg;
        public AnimatorController characterAnimator;
    }
}