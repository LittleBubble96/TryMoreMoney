using Game.Modules.Components.AttackComponent;
using Unity.Entities;
using UnityEngine;

namespace Game.StateMachineBehaviour
{
    public class StateMachineMono : MonoBehaviour
    {

        private Entity _entity;

        private EntityManager _entityManager;
        
        public void RegisterEntiy(Entity entity , EntityManager entityManager)
        {
            _entity = entity;
            _entityManager = entityManager;
            var anim = GetComponent<Animator>();

            //获取所有动画状态机 的 单独状态
            AttackStateMachineBehaviour[] animStateMachines = anim.GetBehaviours<AttackStateMachineBehaviour>();
            for (int i = 0; i < animStateMachines.Length; i++)
            {
                //设置所有动画状态进入委托
                animStateMachines[i].EnterStateCallBack = SetCurrentAnimatorState;
                //设置所有动画状态退出委托
                animStateMachines[i].ExitStateCallBack = SetCurrentAnimtorExit;
               
            }
            
            AttackSMB[] attackSmbs = anim.GetBehaviours<AttackSMB>();
            for (int i = 0; i < attackSmbs.Length; i++)
            {
                //设置所有动画状态机进入委托
                attackSmbs[i].EnterStateMachineCallBack = SetCurrentStateMachineState;
                //设置所有动画状态机退出委托
                attackSmbs[i].ExitStateMachineCallBack = SetCurrentStateMachineExit;
            }
            
          
        }

        private void SetCurrentAnimtorExit(int obj)
        {
            var has = _entityManager.HasComponent<AttackSMBLintener>(_entity);
            if (has)
            {
                var smb = _entityManager.GetComponentData<AttackSMBLintener>(_entity);
                smb.ExitAnimationHash = obj;
                _entityManager.SetComponentData(_entity,smb);
            }
        }

        private void SetCurrentAnimatorState(int obj)
        {
            var has = _entityManager.HasComponent<AttackSMBLintener>(_entity);
            if (has)
            {
                var smb = _entityManager.GetComponentData<AttackSMBLintener>(_entity);
                if (smb.UpdateAnimationHash!=0)
                {
                    smb.WaitAnimationHash = obj;
                }else
                    smb.EnterAnimationHash = obj;
                _entityManager.SetComponentData(_entity,smb);
            }
        }
        
        private void SetCurrentStateMachineExit(int obj)
        {
            var has = _entityManager.HasComponent<AttackSMBLintener>(_entity);
            if (has)
            {
                var smb = _entityManager.GetComponentData<AttackSMBLintener>(_entity);
                smb.ExitStateMachineHash = obj;
                _entityManager.SetComponentData(_entity,smb);
            }
        }

        private void SetCurrentStateMachineState(int obj)
        {
            var has = _entityManager.HasComponent<AttackSMBLintener>(_entity);
            if (has)
            {
                var smb = _entityManager.GetComponentData<AttackSMBLintener>(_entity);
                smb.EnterStateMachineHash = obj;
                _entityManager.SetComponentData(_entity,smb);
            }
        }
    }
}