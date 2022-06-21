using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput
{
    private static GameInput _instance;

    public static GameInput GetInstance => _instance ?? (_instance = new GameInput());

    //is pause
    public bool IsBlock { get; set; }

    private readonly Dictionary<KeyCode, bool> _keycodeUpDic = new Dictionary<KeyCode, bool>();

    public bool GetKeyUp(KeyCode keyCode)
    {
        if (IsBlock)
        {
            return false;
        }
        _keycodeUpDic.TryGetValue(keyCode, out bool up);
        return up || Input.GetKeyUp(keyCode);
    }

    public void SetKeyUp(KeyCode keyCode,bool up)
    {
        if (_keycodeUpDic.TryGetValue(keyCode, out _))
        {
            _keycodeUpDic[keyCode] = up;
        }
        else
        {
            _keycodeUpDic.Add(keyCode,up);
        }
    }

}
