using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AttackSMB : StateMachineBehaviour
{
    public Action<int> EnterStateMachineCallBack;
    public Action<int> ExitStateMachineCallBack;

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller)
    {
        base.OnStateMachineEnter(animator, stateMachinePathHash, controller);
        DDebug.Log("进入动画状态机的PathHash"+stateMachinePathHash);

        base.OnStateMachineEnter(animator, stateMachinePathHash);

        if (EnterStateMachineCallBack != null)
        {
            EnterStateMachineCallBack(stateMachinePathHash);
        }
    }
    
   

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {        
        DDebug.Log("进入动画状态机的PathHash"+stateMachinePathHash);

        base.OnStateMachineExit(animator, stateMachinePathHash);

        if (ExitStateMachineCallBack != null)
        {
            ExitStateMachineCallBack(stateMachinePathHash);
            // DDebug.Log("离开动画状态机的PathHash"+stateMachinePathHash);
        }
    }
    
  
}
