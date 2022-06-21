using Game.Action;
using Game.Modules.Components.AttackComponent;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class GearDriveSystem : ComponentSystem
{
    public GearDriveSystem()
    {
    }

    //符合实体得组
    private EntityQuery _group;

    protected override void OnCreate()
    {
        base.OnCreate();
        _group = GetEntityQuery(typeof(GearDrive),typeof(GearDriveCompoent),typeof(Transform));
    }

    protected override void OnUpdate()
    {
        var commondArray = _group.ToComponentDataArray<UseCommond>(Allocator.TempJob);
        var entityArray = _group.ToEntityArray(Allocator.TempJob);
        for (int i = 0; i < commondArray.Length; i++)
        {
            //var c = commondArray[i];
        }

        commondArray.Dispose();
        entityArray.Dispose();
    }
    
}