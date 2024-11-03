using UnityEngine;

namespace ScriptObj
{
    [CreateAssetMenu(fileName = "PlayerStatus", menuName = "DataBase/PlayerStatus")]
    public class PlayerStatus : ScriptableObject
    {
        [Header("血量")]
        public float maxHealth; //最大生命值
        public float health; //当前生命值
        public float healthRecover; //生命恢复量

        [Header("经验")]
        public uint level; //等级
        public int maxExp; //经验值上限
        public int currentExp; //当前经验值

        [Header("攻击")]
        public float attack; //伤害加成%
        public float attackSpeed; //攻击速度%
        public float criticalRate; //暴击率%
        public float criticalDamage; //暴击伤害%
        public float attackRange; //攻击范围%

        [Header("防御")]
        public float armor; //护甲
        public float dodgeRate; //闪避几率%

        [Header("其他")]
        public float speed; //移动速度%
        public float pickUpRange; //拾取范围%
        public int goldValue; //金币
        public bool isAttackWithPoison; //攻击是否带毒
        public int weaponSlot = 6; //武器槽位
        public int hurtTextSize = 10; // 受伤字体大小

        public void Init()
        {
            goldValue = 0;
        }
    }
}