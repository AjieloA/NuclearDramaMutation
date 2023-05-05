using System.Collections.Generic;
using UnityEngine.Events;

public interface IEventInfo
{ }

//无泛型参数
public class EventInfo : IEventInfo
{
    public UnityAction eventAction;
}

//有泛型参数_一个参数
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> eventAction;
}

//有泛型参数_两个参数
public class EventInfo<T, S> : IEventInfo
{
    public UnityAction<T, S> eventAction;
}

public class EventCore
{
    private static EventCore instance;

    public static EventCore Instance
    {
        get
        {
            if (instance == null) instance = new EventCore();
            return instance;
        }
    }

    public Dictionary<string, IEventInfo> eventActionDic = new Dictionary<string, IEventInfo>();

    //添加事件
    public void AddEventListener(string name, UnityAction action,params object[] objects)
    {
        if (eventActionDic.ContainsKey(name))
            (eventActionDic[name] as EventInfo).eventAction += action;
        else
            eventActionDic.Add(name, new EventInfo() { eventAction = action });
    }

    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventActionDic.ContainsKey(name))
            (eventActionDic[name] as EventInfo<T>).eventAction += action;
        else
            eventActionDic.Add(name, new EventInfo<T>() { eventAction = action });
    }

    public void AddEventListener<T, S>(string name, UnityAction<T, S> action)
    {
        if (eventActionDic.ContainsKey(name))
            (eventActionDic[name] as EventInfo<T, S>).eventAction += action;
        else
            eventActionDic.Add(name, new EventInfo<T, S>() { eventAction = action });
    }

    //移除事件
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventActionDic.ContainsKey(name))
            (eventActionDic[name] as EventInfo).eventAction -= action;
    }

    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventActionDic.ContainsKey(name))
            (eventActionDic[name] as EventInfo<T>).eventAction -= action;
    }

    public void RemoveEventListener<T, S>(string name, UnityAction<T, S> action)
    {
        if (eventActionDic.ContainsKey(name))
            (eventActionDic[name] as EventInfo<T, S>).eventAction -= action;
    }

    //触发事件
    public void TiggerEventListener(string name,params object[] objects)
    {
        if (eventActionDic.ContainsKey(name))
            (eventActionDic[name] as EventInfo).eventAction?.Invoke();
    }

    public void TiggerEventListener<T>(string name, T t)
    {
        if (eventActionDic.ContainsKey(name))
            (eventActionDic[name] as EventInfo<T>).eventAction?.Invoke(t);
    }

    public void TiggerEventListener<T, S>(string name, T t, S s)
    {
        if (eventActionDic.ContainsKey(name))
            (eventActionDic[name] as EventInfo<T, S>).eventAction?.Invoke(t, s);
    }

    //清空事件
    public void ClearEvents()
    {
        eventActionDic.Clear();
    }
}