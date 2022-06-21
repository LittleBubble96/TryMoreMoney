using System;
using System.Collections.Generic;
using Bubble.Tools;
using Game.Modules.Components;
using Unity.Collections;
using Unity.Entities;
using UnityEditor.Animations;
using UnityEngine;
/*
 *如果 下一个动画和这个动画有拼接 获取拼接动画的 名字 ，将参数设成true   ， 将下一个拼接动画的值设成false
 *如果没有就直接播放动画
    引入动作组的概念 ， 连续播放等待的上一个动画播放结束

 */
namespace Game.Modules.Systems
{
    public class AnimatorUpdateSystem : ComponentSystem
    {
        
        private EntityQuery _entityGroup;

        private Dictionary<LocomotionState.ELocomptionState, AnimatorState> _animationStatesDic;

        private AnimatorController _animatorController;

        private Dictionary<LocomotionState.ELocomptionState, string[]> _aniGroup;


        protected override void OnCreate()
        {
            base.OnCreate();
            _entityGroup = GetEntityQuery(typeof(LocomotionState),typeof(LookComponent));
        }

        protected override void OnUpdate()
        {
            var locomotions = _entityGroup.ToComponentDataArray<LocomotionState>(Allocator.TempJob);
            var lookComponents = _entityGroup.ToComponentDataArray<LookComponent>(Allocator.TempJob);
            var entities = _entityGroup.ToEntityArray(Allocator.TempJob);
            for (int i = 0; i < locomotions.Length; i++)
            {
                var animator = EntityManager.GetComponentObject<Animator>(lookComponents[i].LookAnimator);
                var lc = locomotions[i];
                if (animator)
                {
                    UpdateAnimator(animator,lc,entities[i]);
                }
            }

            locomotions.Dispose();
            lookComponents.Dispose();
            entities.Dispose();
        }

        private void UpdateAnimator(Animator animator ,LocomotionState locomotionState,Entity entity)
        {
            if(locomotionState.ExcuateState == LocomotionState.ELocomptionState.None) return;
            if (locomotionState.ExcuateState != locomotionState.CurState)
            {
                var toConditions = GetCombineNameByState(locomotionState.ExcuateState);

                var toState = MatchAnimationState(animator, 0, locomotionState.ExcuateState);
                foreach (var t in toConditions)
                {
                    SetTransition(animator,toState,t,false);
                }
                //如果是需要 中间过渡动画
                if (HasTransformAnimation(locomotionState.CurState,locomotionState.ExcuateState))
                {
                    var fromState = MatchAnimationState(animator, 0, locomotionState.CurState);
                    if (fromState)
                    {
                        SetTransition(animator,fromState,CombineTransformAnimatorName(locomotionState.CurState,locomotionState.ExcuateState),true);
                    }
                    locomotionState.CurState = locomotionState.ExcuateState;
                    locomotionState.ExcuateState = LocomotionState.ELocomptionState.None;
                    EntityManager.SetComponentData(entity,locomotionState);
                }
                else
                {
                    animator.ForceCrossFade(locomotionState.ExcuateState+"",0.2f,0);
                    locomotionState.CurState = locomotionState.ExcuateState;
                    locomotionState.ExcuateState = LocomotionState.ELocomptionState.None;
                    EntityManager.SetComponentData(entity,locomotionState);
                }
            }
            else
            {
                //如果是动画组的
                var getValueState = TryGetValueInGroup(locomotionState.ExcuateState, locomotionState.GroupValue,
                    out string aniName);
                switch (getValueState)
                {
                    case EGetGroupValue.Success:
                        animator.SetInteger(locomotionState.ExcuateState+"",locomotionState.GroupValue);
                        break;
                    case EGetGroupValue.OutCapcity:
                        break;
                    case EGetGroupValue.NotExist:
                      
                        break;
                }
            }

        }
        
        private bool HasTransformAnimation(LocomotionState.ELocomptionState from , LocomotionState.ELocomptionState to)
        {
            var idleToRun = (from & LocomotionState.ELocomptionState.Idle) != 0 &&
                            (to & LocomotionState.ELocomptionState.Run) != 0;
            var runToIdle = (from & LocomotionState.ELocomptionState.Run) != 0 &&
                            (to & LocomotionState.ELocomptionState.Idle) != 0;
            return idleToRun || runToIdle;
        }

        private string[] GetCombineNameByState(LocomotionState.ELocomptionState from)
        {
            switch (from)
            {
                case LocomotionState.ELocomptionState.Idle:
                    return new[] {CombineTransformAnimatorName(from, LocomotionState.ELocomptionState.Run)};
                case LocomotionState.ELocomptionState.Run:
                    return new[] {CombineTransformAnimatorName(from, LocomotionState.ELocomptionState.Idle)};
            }
            return Array.Empty<string>();
        }

        //匹配合适的AnimatorState
        private AnimatorState MatchAnimationState(Animator animator , int layer,LocomotionState.ELocomptionState from)
        {
            if (_animationStatesDic == null)
            {
                _animationStatesDic = new Dictionary<LocomotionState.ELocomptionState, AnimatorState>();
            }

            if (_animationStatesDic.TryGetValue(from, out var aState)) return aState;
            
            _animatorController = animator.runtimeAnimatorController as AnimatorController;
            if (!_animatorController) return null;
            var machines = _animatorController.layers[layer].stateMachine.stateMachines;
            for (int i = 0; i < machines.Length; i++)
            {
                var states = machines[i].stateMachine.states;
                for (int j = 0; j < states.Length; j++)
                {
                    var state = states[j].state;
                    if (state.name ==from.ToString())
                    {
                        _animationStatesDic.Add(from,state);
                        return state;
                    }
                }
            }
            return null;
        }

        private string CombineTransformAnimatorName(LocomotionState.ELocomptionState from , LocomotionState.ELocomptionState to)
        {
            return from + "To" + to;
        }

        private void SetTransition(Animator animator , AnimatorState from , string combineName , bool value)
        {
            var transitions = from.transitions;
            foreach (var transition in transitions)
            {
                if (transition.destinationState.nameHash == Animator.StringToHash(combineName))
                {
                    foreach (var t in transition.conditions)
                    {
                        animator.SetBool(t.parameter,value);
                    }

                    break;
                }
            }
        }

        #region 动画组

        private enum EGetGroupValue
        {
            Success,
            OutCapcity, //超出大小
            NotExist,   //不存在
        }
        

        //下标从0开始吧、
        private EGetGroupValue TryGetValueInGroup(LocomotionState.ELocomptionState state , int index , out string aniName)
        {
            aniName = "";
            if (_aniGroup == null)
            {
                InitAniGroup();
            }

            if (_aniGroup.TryGetValue(state , out var aniNames))
            {
                if (index < aniNames.Length)
                {
                    aniName = aniNames[index];
                    return EGetGroupValue.Success;
                }

                return EGetGroupValue.OutCapcity;
            }

            return EGetGroupValue.NotExist;
        }

        #endregion

        //初始化动作组
        private void InitAniGroup()
        {
            _aniGroup = new Dictionary<LocomotionState.ELocomptionState, string[]>();
            _aniGroup.Add(LocomotionState.ELocomptionState.AttackNormal,
                new string[] {"AttackNormal1", "AttackNormal2", "AttackNorma3", "AttackNormal4", "AttackNormal5"});
        }
    }
}