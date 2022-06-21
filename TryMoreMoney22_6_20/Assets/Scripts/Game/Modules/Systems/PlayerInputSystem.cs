using System.Collections;
using System.Collections.Generic;
using Game.Action;
using Game.Modules.Components.AttackComponent;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class PlayerInputSystem : ComponentSystem
{
    public PlayerInputSystem()
    {
        
    }

    //符合实体得组
    private EntityQuery _group;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        _group = GetEntityQuery(typeof(UseCommond));
    }

    protected override void OnUpdate()
    {
        var commondArray =  _group.ToComponentDataArray<UseCommond>(Allocator.TempJob);
        var entityArray = _group.ToEntityArray(Allocator.TempJob);
        for (int i = 0; i < commondArray.Length; i++)
        {
            var c = commondArray[i];
            SampleAttackNormalInput(ref c, entityArray[i]);
            SampleMovementInput(ref c,entityArray[i]);
        }
    
        commondArray.Dispose();
        entityArray.Dispose();
    }

    private void SampleMovementInput(ref UseCommond commond,Entity entity)
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            commond.HorizontalAxis = Input.GetAxis("Horizontal");
            commond.VerticalAxis = Input.GetAxis("Vertical");
        }

        var isKeyDownDirection = Input.GetKeyDown(KeyCode.W) |
                                 Input.GetKeyDown(KeyCode.A) |
                                 Input.GetKeyDown(KeyCode.S) |
                                 Input.GetKeyDown(KeyCode.D);
        var forward = Vector3.zero;
        if (isKeyDownDirection)
        {
            //forward = SceneMgr.Instance.GetMainPlayerCamera().PlayerThirdCamera.transform.forward;
            forward = new Vector3(forward.x, 0, forward.z);
        }
        ActionMgr.Instance.ExecuteAction(EntityManager,entity
            ,new UseCommondAction()
            {
                DoHorticalAxis = commond.HorizontalAxis,
                DoVerticalAxis = commond.VerticalAxis,
                CameraDirection = forward
            });
        
    }


    private void SampleAttackNormalInput(ref UseCommond commond,Entity entity)
    {
        commond.AttackNormaling = Input.GetKey(KeyCode.I);
        var attackComponent = EntityManager.GetComponentData<AttackComponent>(entity);
        attackComponent.ExcuteAttackType = commond.AttackNormaling ? (int) ERoleAttackType.AttackNormal : 0;
        EntityManager.SetComponentData(entity,attackComponent);
    }
    
}
