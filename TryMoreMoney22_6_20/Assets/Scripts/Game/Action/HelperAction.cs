using Game.Modules.Components;
using Unity.Entities;

namespace Game.Action
{
    public class HelperAction
    {
        public static void JudgeStateExcuteAction(EntityManager entityManager, Entity entity ,LocomotionState locomotion
            , LocomotionState.ELocomptionState newState
            , int groupValue = 0)
        {
            if(locomotion.ExcuateState != newState || 
               locomotion.ExcuateState == LocomotionState.ELocomptionState.AttackNormal)
                ActionMgr.Instance.ExecuteAction(entityManager,entity,new LocomotionStateAction()
                {
                    State =  newState,
                    GroupValue = groupValue,
                });
        }
    }
}