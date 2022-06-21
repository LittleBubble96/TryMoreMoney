using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateMachineBehaviour : StateMachineBehaviour
{
    public Action<int> EnterStateCallBack;
    public Action<int> ExitStateCallBack;
    

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        if (EnterStateCallBack != null)
        {
            EnterStateCallBack(stateInfo.shortNameHash);
            // DDebug.Log("进入动画状态的PathHash"+stateInfo.shortNameHash);
        }
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        if (ExitStateCallBack != null)
        {
            ExitStateCallBack(stateInfo.shortNameHash);
            // DDebug.Log("离开动画状态的PathHash"+stateInfo.shortNameHash );
        }
    }
    
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateMove(animator, stateInfo, layerIndex);
        animator.ApplyBuiltinRootMotion();
    }
}
