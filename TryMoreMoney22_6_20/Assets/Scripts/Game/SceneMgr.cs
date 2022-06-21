using System;
using System.Collections;
using System.Collections.Generic;
using Game.Camera;
using Game.Role;
using Unity.Entities;
using UnityEngine;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr Instance;

    //private GameObjectEntity _roleEntity;

    //private GameCamera _playerCamera;
    private void Awake()
    {
        Instance = this;
    }

    public void Init(GameWorld gameWorld)
    {
        GearEntity role = new GearEntity(gameWorld);
        //_roleEntity = role.RoleGameObjectEntity;
        //_playerCamera = new GameCamera(role.CameraAim);
    }

    // public Entity GetMainRole()
    // {
    //     return _roleEntity.Entity;
    // }
    //
    // public GameCamera GetMainPlayerCamera()
    // {
    //     return _playerCamera;
    // }
}
