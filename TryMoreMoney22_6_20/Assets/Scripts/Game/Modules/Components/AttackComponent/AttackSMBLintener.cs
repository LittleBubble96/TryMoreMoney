using Unity.Entities;

namespace Game.Modules.Components.AttackComponent
{
    public struct AttackSMBLintener : IComponentData
    {
        public int EnterAnimationHash;

        public int UpdateAnimationHash;

        public int ExitAnimationHash;

        public int WaitAnimationHash;

        public int EnterStateMachineHash;
        
        public int ExitStateMachineHash;

        public float ExcuteAnimationTime;
    }
}