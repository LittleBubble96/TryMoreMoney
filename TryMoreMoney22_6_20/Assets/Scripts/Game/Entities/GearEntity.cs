using Unity.Entities;
using UnityEngine;

public class GearEntity
{
    private Transform _gearContainer;

    public GameObjectEntity gearEntity;
    
    public GearEntity(GameWorld gameWorld)
    {
        _gearContainer = GameObject.Find("SceneObj/GearContainer").transform;
        gearEntity = gameWorld.SpawnByGameObject<GameObjectEntity>
                (ResourceAdapter.GetInstance().LoadPrefabAsset<GameObject>("ABResources/Role/chilun"));
        gearEntity.transform.SetParent(_gearContainer);

        InitGear(gameWorld);
    }

    private void InitGear(GameWorld gameWorld)
    {
        //添加用户命令
        gameWorld.GetEntityManager().AddComponentData(gearEntity.Entity, new GearDrive(50));
        gameWorld.GetEntityManager().AddComponentData(gearEntity.Entity, new GearDriveCompoent(5));

    }
}