using Game.Action;
using Game.Modules.Components.AttackComponent;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class GearRotateSystem : ComponentSystem
{
    public GearRotateSystem()
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
        var driveArray = _group.ToComponentDataArray<GearDrive>(Allocator.TempJob);
        var driveRadiusArray = _group.ToComponentDataArray<GearDriveCompoent>(Allocator.TempJob);
        var transformArray = _group.ToComponentArray<Transform>();
        for (int i = 0; i < driveArray.Length; i++)
        {
            transformArray[i].localEulerAngles +=
                new Vector3(0, 0, UnityEngine.Time.deltaTime * (driveArray[i].marginalLinearVelocity / driveRadiusArray[i].gearRadius));
        }

        driveArray.Dispose();
        driveRadiusArray.Dispose();
    }
}