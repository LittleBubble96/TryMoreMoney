using System;
using System.Collections.Generic;
using System.Reflection;
using System.Timers;
using Bubble.Tools;
using Unity.Entities;
using UnityEngine;

namespace Game.Action
{
   public class ActionMgr : IDisposable
   {
      public ActionMgr()
      {
         
      }
   

      //注意 这是实例对象 并不是单例
      public static ActionMgr Instance;
   
      public static void CreateActionMgr()
      {
         Instance = new ActionMgr();
      }

      //清楚对象
      public static void Clear()
      {
         Instance.Dispose();
         Instance = null;
      }

    

      public void Dispose()
      {
         
      }

      //适合有参构造函数得结构体
      public void ExecuteAction<T>(EntityManager entityManager,Entity entity, T action) where  T: IAction
      {
         action.ActionStartTime = DateTime.Now;
         action.Execute(entityManager , entity);
      }

      //适合无参构造函数得结构体
      public void ExecuteAction<T>(EntityManager entityManager,Entity entity) where  T : struct,IAction
      {
         T action = new T();
         ExecuteAction(entityManager,entity, action);
      }

      public void ExecuteAction(EActionName actionName)
      {
         // var types = Assembly.GetAssembly(typeof(IAction)).GetTypes();
         //
         //
         // foreach (var type in types)
         // {
         //    FieldInfo fieldInfo = type.GetField("ActionName");
         //    if (fieldInfo.GetValue())
         //    {
         //       
         //    }
         // }
      }
   }
}
