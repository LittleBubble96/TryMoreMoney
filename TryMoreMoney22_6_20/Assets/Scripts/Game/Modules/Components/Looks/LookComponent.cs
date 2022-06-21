using Unity.Entities;
using UnityEngine;

namespace Game.Modules.Components
{
    public struct LookComponent : IComponentData
    {
        public Entity LookAnimator;

        public LookComponent(Entity lookAnimator)
        {
            LookAnimator = lookAnimator;
        }
    }
}