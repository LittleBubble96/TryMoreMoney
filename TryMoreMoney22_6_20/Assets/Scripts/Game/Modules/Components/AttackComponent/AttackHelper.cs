using UnityEngine;

namespace Game.Modules.Components.AttackComponent
{
    public enum ERoleAttackType
    {
        AttackNormal = 1,   //1-10 1是第一下 ，后续combo累加
    }
    public enum EWeaponType
    {
        Katana = 1, //太刀
        Epee = 2,   //重剑
    }

    public class AttackHelper
    {
        private static readonly int AttackNormal1Hash = Animator.StringToHash("AttackNormal");
        private static readonly int AttackNormal2Hash = Animator.StringToHash("AttackNormal1");
        private static readonly int AttackNormal3Hash = Animator.StringToHash("AttackNormal2");
        private static readonly int AttackNormal4Hash = Animator.StringToHash("AttackNormal3");
        private static readonly int AttackNormal5Hash = Animator.StringToHash("AttackNormal4");
        private static readonly int AttackNormalStateMachineHash = Animator.StringToHash("Base Layer.AttackNormalSM");

        public static int GetAttackID(EWeaponType weaponType,int attackType  ,int combo = 0)
        {
            return (int) weaponType * 10000 + attackType * 10  + combo;
        }


        public static bool IsAttackNormal(int attackId)
        {
            return (attackId % 10000) / 10 <= 1 && (attackId % 10000) % 10 >= 0;
        }

        public static bool IsSameAttackType(int attackId, int animatorHash)
        {
            if ((EWeaponType)(attackId / 10000)  == EWeaponType.Katana && IsAttackNormal(attackId))
            {
                if (animatorHash == AttackNormal1Hash || 
                    animatorHash == AttackNormal2Hash || 
                    animatorHash == AttackNormal3Hash || 
                    animatorHash == AttackNormal4Hash ||
                    animatorHash == AttackNormal5Hash)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsEqualAttackAndHash(int attackId, int animatorHash)
        {
            DDebug.Log("离开动画状态的animatorHash"+AttackNormalStateMachineHash + "pathHash:" + animatorHash);

            if ((EWeaponType)(attackId / 10000)  == EWeaponType.Katana)
            {
                if (attackId %10000 % 10<= 10 && animatorHash == AttackNormalStateMachineHash) 
                  
                {
                    return true;
                }
            }
            return false;
        }

        public static bool GetAttackInfoById(int attackId , out AttackComboInfo comboInfo, out AttackBaseInfo attackBaseInfo)
        {
            comboInfo = new AttackComboInfo(){StartCheckFrameTime = 0.25f};
            attackBaseInfo = new AttackBaseInfo(){ComboMaxCount = 5};
            return true;
        }
    }

}