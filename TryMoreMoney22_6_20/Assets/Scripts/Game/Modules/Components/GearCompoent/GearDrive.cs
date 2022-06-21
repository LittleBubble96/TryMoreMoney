using Unity.Entities;

public struct GearDrive : IComponentData
{
    //边缘线速度
    public float marginalLinearVelocity;

    public GearDrive(float v)
    {
        marginalLinearVelocity = v;
    }
}