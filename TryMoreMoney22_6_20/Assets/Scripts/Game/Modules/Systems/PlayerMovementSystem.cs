using Game;
using Game.Action;
using Game.Config;
using Game.Modules.Components;
using Game.Modules.Components.AttackComponent;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class PlayerMovementSystem : Unity.Entities.ComponentSystem
{
    public PlayerMovementSystem()
    {
        
    }

    private EntityQuery _entityQuery;

    protected override void OnCreate()
    {
        base.OnCreate();
        _entityQuery = GetEntityQuery(
            typeof(PlayerMovementData),
            typeof(CharacterController),
            typeof(MoveSpeed),
            typeof(LocomotionState));
    }

    protected override void OnUpdate()
    {
        var movements = _entityQuery.ToComponentDataArray<PlayerMovementData>(Allocator.TempJob);
        var speeds = _entityQuery.ToComponentDataArray<MoveSpeed>(Allocator.TempJob);
        var transforms = _entityQuery.ToComponentArray<CharacterController>();
        var locomotions = _entityQuery.ToComponentDataArray<LocomotionState>(Allocator.TempJob);
        var entityArray = _entityQuery.ToEntityArray(Allocator.TempJob);
        for (int i = 0; i < movements.Length; i++)
        {
            var hasAc = EntityManager.HasComponent<AttackComponent>(entityArray[i]);
            bool isAttacking = hasAc && EntityManager.GetComponentData<AttackComponent>(entityArray[i]).Attacking;
           
            if (!isAttacking)
            {
                var forward = PlayerRotate(transforms[i].transform, movements[i]);
                PlayerMove(transforms[i],entityArray[i], forward, speeds[i],locomotions[i]);
            }
          
        }

        movements.Dispose();
        speeds.Dispose();
        locomotions.Dispose();
        entityArray.Dispose();
    }


    private Vector3 PlayerRotate(Transform playerBody , PlayerMovementData moveData)
    {
        var targetDirection = new Vector3(moveData.HorticalAxis, 0, moveData.VerticalAxis).normalized;
        if(targetDirection == Vector3.zero) return default;
        Vector3 forward = moveData.CameraDirection;
        Quaternion offsetRot= Quaternion.FromToRotation(Vector3.forward, forward);
        targetDirection = offsetRot * targetDirection; 

        var nowDirection = playerBody.forward;
        nowDirection = new Vector3(nowDirection.x, 0, nowDirection.z);
        var angleTime = GameConst.PlayerMoveRotateSpeed * UnityEngine.Time.deltaTime;
        var needRotateAngle = getAngleV2(nowDirection, targetDirection);

        if (Mathf.Abs(needRotateAngle) > angleTime)
        {
            // Vector3 lastDirection = nowDirection;
            nowDirection = Quaternion.AngleAxis(angleTime , 
                Vector3.up * (needRotateAngle / Mathf.Abs(needRotateAngle))) * nowDirection;
            // DDebug.Log($"目标方向为:{targetDirection},相机朝向为：{forward}" +
            //            $",当前方向为：{nowDirection} , 上一帧方向为:{lastDirection}，需要旋转的角度{needRotateAngle}");

        }
        else
        {
            nowDirection = targetDirection;
        }

        playerBody.forward = nowDirection;
        return nowDirection;
    }


    private void PlayerMove(CharacterController characterController,Entity entity,Vector3 forward,MoveSpeed moveSpeed,LocomotionState locomotion)
    {
        if (forward == default)
        {
            HelperAction.JudgeStateExcuteAction(EntityManager, entity, locomotion,
                LocomotionState.ELocomptionState.Idle);
            return;
        }

        HelperAction.JudgeStateExcuteAction(EntityManager, entity, locomotion,
            LocomotionState.ELocomptionState.Run);
        characterController.Move(forward.normalized * moveSpeed.moveSpeed * UnityEngine.Time.deltaTime);
    }


    private float getAngleV2(Vector3 v1, Vector3 v2)
    {
        Vector3 cross = Vector3.Cross(v1, v2);
        float angle = Vector3.Angle(v1, v2);
        angle = cross.y > 0 ? angle : -angle;
        return angle;
    }
}
