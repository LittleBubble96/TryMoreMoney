using System;
using Cinemachine;
using Unity.Entities;
using UnityEngine;

namespace Game.Camera
{
    public class GameCamera : IDisposable
    {
        private CinemachineFreeLook _freeLook;
        
        public GameObjectEntity CameraGameObjectEntity { get; set; }

        public UnityEngine.Camera PlayerThirdCamera { get; set; }

        public GameCamera(Transform aim)
        {
            var freeLookObj =  GameObject.Find("Camera/PlayerFreeLook");
            PlayerThirdCamera =  GameObject.Find("Camera/PlayerCamera").GetComponent<UnityEngine.Camera>();
            var cameraEntity =  GameMain.Instance.Get_MyWorld().InternalEntity(freeLookObj);
            _freeLook = freeLookObj.GetComponent<CinemachineFreeLook>();
            CameraGameObjectEntity = freeLookObj.GetComponent<GameObjectEntity>();
            InitParams(aim);
            
        }


        private void InitParams(Transform aim)
        {
            _freeLook.Follow = aim;
            _freeLook.LookAt = aim;
        }

        public void Dispose()
        {
            
        }
    }
}