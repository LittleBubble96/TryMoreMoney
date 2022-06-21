using System;
using Game.Action;
using Game.Modules.Components;
using Game.Modules.Components.AttackComponent;
using Unity.Collections;
using Unity.Entities;

namespace Game.Modules.Systems
{
    public class AttackUpdateSystem : ComponentSystem
    {
        private EntityQuery _entityQuery;

        protected override void OnCreate()
        {
            base.OnCreate();
            _entityQuery = GetEntityQuery(typeof(AttackComponent),typeof(AttackSMBLintener),typeof(LocomotionState));
        }

        protected override void OnUpdate()
        {
            var attackComponents = _entityQuery.ToComponentDataArray<AttackComponent>(Allocator.TempJob);
            var attackSmbLinteners = _entityQuery.ToComponentDataArray<AttackSMBLintener>(Allocator.TempJob);
            var locomations = _entityQuery.ToComponentDataArray<LocomotionState>(Allocator.TempJob);
            
            var entities = _entityQuery.ToEntityArray(Allocator.TempJob);
            
            for (int i = 0; i < attackComponents.Length; i++)
            {
                var ac = attackComponents[i];
                var aSmb = attackSmbLinteners[i];
               
                if (ac.ExcuteAttackType!=0)
                {
                    int aId = AttackHelper.GetAttackID(EWeaponType.Katana,ac.ExcuteAttackType,ac.ComboAttack + 1);
                    var matchAccordanceInfo =
                        AttackHelper.GetAttackInfoById(aId, out var comboInfo, out var attackBaseInfo);
                    //按下了  如果不是在 attack中  
                    if (ac.ComboAttack == 0 && aSmb.UpdateAnimationHash == 0)
                    {
                        ac.AttackId = AttackHelper.GetAttackID(EWeaponType.Katana,ac.ExcuteAttackType,ac.ComboAttack + 1);
                        if (ExcuteAttack(ac.AttackId,entities[i],locomations[i],ac.ComboAttack))
                        {
                            ac.Attacking = true;
                        }
                    }
                    else if (ac.ComboAttack >= 0 && //开始combo
                             AttackHelper.IsSameAttackType(ac.AttackId,aSmb.UpdateAnimationHash) && //是相同攻击类型
                             matchAccordanceInfo && //有符合combo对象
                             ac.ComboAttack < attackBaseInfo.ComboMaxCount && //combo次数小于最大次数
                             aSmb.ExcuteAnimationTime >= comboInfo.StartCheckFrameTime) //到了下一个技能施放的时间
                    {
                        ac.AttackId = AttackHelper.GetAttackID(EWeaponType.Katana,ac.ExcuteAttackType,ac.ComboAttack + 1);
                        ExcuteAttack(ac.AttackId, entities[i], locomations[i], ac.ComboAttack);
                        // DDebug.Log($"执行第{ac.AttackId}次,时间为{aSmb.ExcuteAnimationTime}，combo = {ac.ComboAttack}");
                    }
                }


                //动画监听
                
                
                if (aSmb.EnterAnimationHash!=0)
                {
                    EnterState(ref aSmb,ref ac);
                }

                if (aSmb.ExitStateMachineHash != 0)
                {
                    ExitSMB(ref aSmb , ref  ac);
                }

                if (aSmb.ExitAnimationHash!=0)
                {
                    ExitState(ref aSmb , ref  ac);
                }

                if (aSmb.UpdateAnimationHash!=0)
                {
                    UpdateState(ref aSmb);
                }
                EntityManager.SetComponentData(entities[i],aSmb); 
                EntityManager.SetComponentData(entities[i],ac); 
            }
            attackComponents.Dispose();
            entities.Dispose();
            locomations.Dispose();
            attackSmbLinteners.Dispose();
        }
        
        private void EnterState(ref AttackSMBLintener smbLintener,ref AttackComponent ac)
        {
            DDebug.Log($"进入{smbLintener.EnterAnimationHash},combo次数{ac.ComboAttack}");
            smbLintener.UpdateAnimationHash = smbLintener.EnterAnimationHash;
            smbLintener.EnterAnimationHash = 0;
            smbLintener.ExcuteAnimationTime = 0;
            ac.ComboAttack++;
        }
        
        private void ExitState(ref AttackSMBLintener smbLintener , ref  AttackComponent ac)
        {
            
            smbLintener.ExitAnimationHash = 0;
            smbLintener.UpdateAnimationHash = 0;
            smbLintener.ExcuteAnimationTime = 0;
            if (smbLintener.WaitAnimationHash != 0)
            {
                smbLintener.EnterAnimationHash = smbLintener.WaitAnimationHash;
                smbLintener.WaitAnimationHash = 0;
            }
        }

        private void ExitSMB(ref AttackSMBLintener smbLintener , ref  AttackComponent ac)
        {
            DDebug.Log($"退出攻击？{ac.AttackId} , Hash = {smbLintener.ExitStateMachineHash}，攻击时间为:{smbLintener.ExcuteAnimationTime}");
            if (AttackHelper.IsEqualAttackAndHash(ac.AttackId,smbLintener.ExitStateMachineHash))
            {
                //说明没有下一级combo 退出
                ac.ComboAttack = 0;
                ac.Attacking = false;
            }
            smbLintener.ExitStateMachineHash = 0;
        }

        private void UpdateState(ref AttackSMBLintener smbLintener)
        {
            if(smbLintener.UpdateAnimationHash==0) return;
            smbLintener.ExcuteAnimationTime += UnityEngine.Time.deltaTime;
        }

        private bool ExcuteAttack(int attackId , Entity entity , LocomotionState lo,int combo)
        {
            var attackType = (ERoleAttackType)(attackId % 10000 / 10);
            switch (attackType)
            {
                case ERoleAttackType.AttackNormal:
                    HelperAction.JudgeStateExcuteAction(EntityManager, entity, lo, LocomotionState.ELocomptionState.AttackNormal,combo);
                    return true;
            }
            return false;
        }

    }
}