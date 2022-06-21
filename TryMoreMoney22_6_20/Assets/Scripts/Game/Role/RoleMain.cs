using Game.Modules.Components;
using Game.Modules.Components.AttackComponent;
using Game.StateMachineBehaviour;
using Unity.Entities;
using UnityEngine;

namespace Game.Role
{
    public class RoleMain
    {
        private Transform _roleContainer;
        
        public GameObjectEntity RoleGameObjectEntity { get; set; }

        public Transform CameraAim;

        public RoleMain(GameWorld gameWorld)
        {
            _roleContainer = GameObject.Find("SceneObj/RoleContainer").transform;
            RoleGameObjectEntity =
                gameWorld.SpawnByGameObject<GameObjectEntity>
                    (ResourceAdapter.GetInstance().LoadPrefabAsset<GameObject>("ABResources/Role/MainPlayer"));
            RoleGameObjectEntity.transform.SetParent(_roleContainer);
            CameraAim = RoleGameObjectEntity.transform.Find("CameraAim").transform;

            InitRole(gameWorld , 5);
        }

        private void InitRole(GameWorld gameWorld , int moveSpeed)
        {
            //添加用户命令
            gameWorld.GetEntityManager().AddComponent(RoleGameObjectEntity.Entity, ComponentType.ReadWrite<UseCommond>());
            //添加用户移动数据
            gameWorld.GetEntityManager().AddComponentData(RoleGameObjectEntity.Entity, new PlayerMovementData());
            gameWorld.GetEntityManager().AddComponentData(RoleGameObjectEntity.Entity, new MoveSpeed(moveSpeed));
           
            gameWorld.GetEntityManager().AddComponentData(RoleGameObjectEntity.Entity, 
                new LookComponent(RoleGameObjectEntity.Entity));
            gameWorld.GetEntityManager().AddComponentData(RoleGameObjectEntity.Entity, new LocomotionState(
                LocomotionState.ELocomptionState.Idle,LocomotionState.ELocomptionState.Idle));

            //添加攻击组件
            gameWorld.GetEntityManager().AddComponentData(RoleGameObjectEntity.Entity, new AttackComponent());
            gameWorld.GetEntityManager().AddComponentData(RoleGameObjectEntity.Entity, new AttackSMBLintener());
            
            var smm = RoleGameObjectEntity.GetComponent<StateMachineMono>();
            if (smm)
            {
                smm.RegisterEntiy(RoleGameObjectEntity.Entity,gameWorld.GetEntityManager());
            }

        }

    }
}