
using System;
using Unity.Entities;

namespace Game.Action
{
    public interface IAction
    {
        //用于反射对应类
        EActionName ActionName { get; }

        DateTime ActionStartTime { get; set; }

        void Execute(EntityManager entityManager , Entity entity);

    }
}

