using System;
using Game.Action;
using Unity.Entities;

namespace Game.Modules.Components
{
    public struct LocomotionState : IComponentData
    {
        [Flags]
        public enum ELocomptionState
        {
            None = 0,
            Idle = 1<<1,
            Run = 1<<2,
            AttackNormal = 1<<3
        }

        public ELocomptionState ExcuateState;

        public ELocomptionState CurState;

        //动画组的值
        public int GroupValue;


        public LocomotionState(ELocomptionState excuateState,ELocomptionState curState,int groupValue = 0)
        {
            ExcuateState = excuateState;
            CurState = curState;
            GroupValue = groupValue;
        }
    }

    public struct LocomotionStateAction : IAction
    {
        public EActionName ActionName { get; }
        public DateTime ActionStartTime { get; set; }

        public LocomotionState.ELocomptionState State;

        public int GroupValue;
        
        public void Execute(EntityManager entityManager, Entity entity)
        {
            var locomotion = entityManager.GetComponentData<LocomotionState>(entity);
            locomotion.ExcuateState = State;
            locomotion.GroupValue = GroupValue;
            entityManager.SetComponentData(entity,locomotion);
        }
    }
}