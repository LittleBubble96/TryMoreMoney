using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Profiling;

public class GameWorld
{
    //定义多个世界 ，每个世界是一个实体总和 相当于元宇宙 平行宇宙得概念
    public static List<World> s_Worlds = new List<World>();

    World m_ECSWorld;

    private EntityManager m_EntityManager;
    
    public GameWorld(string worldName = "World")
    {
        m_ECSWorld = new World(worldName);
        World.DefaultGameObjectInjectionWorld = m_ECSWorld;
        m_EntityManager = m_ECSWorld.EntityManager;
        s_Worlds.Add(m_ECSWorld);
    }
    
    public EntityManager GetEntityManager()    
    {
        return m_EntityManager;
    }

    public World GetECSWorld()    
    {
        return m_ECSWorld;
    }

    public T SpawnByGameObject<T>(GameObject gameObject) where  T : Component
    {
        return SpawnByGameObject<T>(gameObject, Vector3.zero, Quaternion.identity);
    }
    
    public T SpawnByGameObject<T>(GameObject gameObject , Vector3 pos , Quaternion rotation) where  T : Component
    {
        Entity entity;
        var go = RequestInternal(gameObject, pos, rotation, out entity);
        if (go == null)
        {
            return null;
        }

        var result = go.GetComponent<T>();
        if (result == null)
        {
            if (result == null)
            {
                DDebug.Log(string.Format("Spawned entity '{0}' didn't have component '{1}'", gameObject, typeof(T).FullName));
                return null;
            }
        }
        return result;
    }

    public GameObject RequestInternal(GameObject gameObject , Vector3 pos , Quaternion rotation, out  Entity entity)
    {
        Profiler.BeginSample("GameObject Spawn Internal");
        var go = Object.Instantiate(gameObject,pos,rotation);
        //注册在这个世界中
        entity = InternalEntity(go);
        Profiler.EndSample();
        return go;
    }


    public Entity InternalEntity(GameObject gameObject)
    {
        var gameObjectEntity = gameObject.GetComponent<GameObjectEntity>();
        if (gameObjectEntity == null || !m_EntityManager.Exists(gameObjectEntity.Entity))
        {
            GameObjectEntity.AddToEntityManager(m_EntityManager, gameObject);
        }
        return gameObjectEntity ? gameObjectEntity.Entity : Entity.Null;
    }


}
