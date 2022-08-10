using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLevelUpEvent
{
    public int skillCode;
    
}

public class EventBus 
{

    static Dictionary<System.Type, List<Action>> _handlerDic = new Dictionary<Type, List<Action>>();
 
    public static void Publish<T>(T ev)
    {
        var type = typeof(T);

        if (_handlerDic.ContainsKey(type) == false)
            return;

        foreach (var action in _handlerDic[type])
        {
            (action as System.Action<T>)(ev);
        }

    }

    public static void Subscribes<T>(System.Action<T> handler)
    {
        var type = typeof(T);
        if (_handlerDic.ContainsKey(type) == false)
        {
            _handlerDic.Add(type,new List<Action>());
        }
        _handlerDic[type].Add(handler as System.Action);
    }

    public static void Unsubscribes<T>(System.Action<T> handler)
    {
        var type = typeof(T);
        if (_handlerDic.ContainsKey(type) == false)
            return;
        _handlerDic[type].Remove(handler as System.Action);
    }

}
