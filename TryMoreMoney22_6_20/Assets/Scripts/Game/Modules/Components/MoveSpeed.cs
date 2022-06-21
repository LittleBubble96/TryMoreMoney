using System;
using System.Collections;
using System.Collections.Generic;
using Game.Action;
using Unity.Entities;
using UnityEngine;

public struct MoveSpeed : IComponentData
{
    public float moveSpeed;

    public MoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
}

public struct SpeedAction : IAction
{
    private float _speed;
    public EActionName ActionName => EActionName.ChangeSpeedAction;
    public DateTime ActionStartTime { get; set; }
    

    public void Execute(EntityManager entityManager , Entity entity)
    {
        var speedComponent = entityManager.GetComponentData<MoveSpeed>(entity);
        speedComponent.moveSpeed = _speed;
        entityManager.SetComponentData(entity , speedComponent);
    }
}
