using System;
using Game.Action;
using Unity.Entities;
using UnityEngine;

namespace Game.Modules.Components.AttackComponent
{
    public struct AttackComponent : IComponentData
    {
        public int ExcuteAttackType;
        
        public int AttackId;
        
        public int ComboAttack;

        public bool Attacking;
        
    }

    // public struct AttackAction : IAction
    // {
    //     public EActionName ActionName { get; }
    //     public DateTime ActionStartTime { get; set; }
    //     
    //     public int AttackType;
    //     
    //     public int ComboAttack;
    //    
    //     public void Execute(EntityManager entityManager, Entity entity)
    //     {
    //         var attackComponent = entityManager.GetComponentData<AttackComponent>(entity);
    //         attackComponent.AttackType = AttackType;
    //         attackComponent.ComboAttack = ComboAttack;
    //         entityManager.SetComponentData(entity,attackComponent);
    //     }
    // }

    //连击信息
    public struct AttackComboInfo
    {
        public float StartCheckFrameTime;
        
        public AttackComboInfo(float startCheckFrameTime)
        {
            StartCheckFrameTime = startCheckFrameTime;
        }
    }

    public struct AttackBaseInfo
    {
        public int ComboMaxCount;
    }

    public struct CancleInfo
    {
        
    }

    //攻击伤害模型
    public interface IAttackDamageModel
    {
        
    }
    
}