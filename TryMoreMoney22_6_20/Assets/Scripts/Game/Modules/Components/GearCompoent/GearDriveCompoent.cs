using Unity.Entities;

public struct GearDriveCompoent: IComponentData
{
    public float gearRadius;

    public GearDriveCompoent(float r)
    {
        gearRadius = r;
    }
}