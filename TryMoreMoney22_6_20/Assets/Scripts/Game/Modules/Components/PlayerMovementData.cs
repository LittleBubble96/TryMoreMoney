
using System;
using Game.Action;
using Unity.Entities;
using UnityEngine;

public struct PlayerMovementData : IComponentData
{
    public float HorticalAxis;

    public float VerticalAxis;

    public Vector3 CameraDirection;
}

public struct UseCommondAction : IAction
{
    public EActionName ActionName => EActionName.UseCommondAction;
    public DateTime ActionStartTime { get; set; }
        
    public  float DoHorticalAxis;

    public  float DoVerticalAxis;

    public  Vector3 CameraDirection;

    
    public void Execute(EntityManager entityManager, Entity entity)
    {
        var moveComponent = entityManager.GetComponentData<PlayerMovementData>(entity);
        moveComponent.VerticalAxis = DoVerticalAxis;
        moveComponent.HorticalAxis = DoHorticalAxis;
        if(CameraDirection != default)
            moveComponent.CameraDirection = CameraDirection;
        entityManager.SetComponentData(entity , moveComponent);
    }
}