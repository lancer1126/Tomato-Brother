using UnityEngine;

namespace ScriptObj
{
    [CreateAssetMenu(fileName = "Character", menuName = "DataBase/Character")]
    public class Character : ScriptableObject
    {
        public Sprite characterImg;
        public AnimatorOverrideController characterAnimator;
    }
}