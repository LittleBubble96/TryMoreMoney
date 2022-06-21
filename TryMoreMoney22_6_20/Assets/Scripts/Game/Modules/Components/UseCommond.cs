

using Unity.Entities;
//用户指令
public struct UseCommond : IComponentData
{
    //-1 left    0 idle     1 right 
    public float HorizontalAxis;

    //-1 Forward    0 idle     1 Down 
    public float VerticalAxis;

    public bool AttackNormaling;

    public override string ToString()
    {
        return $"Horizontal's value is {HorizontalAxis} , Vertical's value is {VerticalAxis}";
    }
}
