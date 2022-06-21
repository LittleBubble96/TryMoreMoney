using System;
using System.Collections.Generic;
using Game.Action;
using Game.Demo_Face;
using Game.Modules.Systems;
using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class GameMain : MonoBehaviour
    {
        public static GameMain Instance;

        private SystemCollection _systems;

        private GameWorld _myWorld;
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Initialize();
        }

        public GameWorld Get_MyWorld()
        {
            return _myWorld;
        }

        void Initialize()
        {
            _myWorld = new GameWorld("ClientWorld");
            SceneMgr.Instance.Init(_myWorld);
            ActionMgr.CreateActionMgr();
            InitSystems();
        }

        void InitSystems()
        {
            _systems = new SystemCollection();
            _systems.AddSystem(_myWorld.GetECSWorld().CreateSystem<GearRotateSystem>());
            // _systems.AddSystem(_myWorld.GetECSWorld().CreateSystem<AttackUpdateSystem>());
            // _systems.AddSystem(_myWorld.GetECSWorld().CreateSystem<PlayerMovementSystem>());
            // _systems.AddSystem(_myWorld.GetECSWorld().CreateSystem<AnimatorUpdateSystem>());
        }

        private void Update()
        {
            _systems.Update();
        }

        private void OnDestroy()
        {
            ActionMgr.Clear();
        }
    }
}