using System.Collections.Generic;
using Unity.Entities;

namespace Game
{
    public class SystemCollection
    {
        private readonly List<ComponentSystem> _systems;

        public SystemCollection()
        {
            _systems = new List<ComponentSystem>();
        }

        public void AddSystem(ComponentSystem system)
        {
            _systems.Add(system);
        }

        public void Update()
        {
            foreach (var system in _systems)
            {
                system.Update();
            }
        }

        public void ShutDown(World world)
        {
            foreach (var system in _systems)
            {
                world.DestroySystem(system);
            }
        }
    }
}