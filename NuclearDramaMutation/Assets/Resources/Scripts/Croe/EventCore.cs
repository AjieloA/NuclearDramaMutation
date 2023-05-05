using System.Collections.Generic;
using UnityEngine.Events;

public interface IEventInfo
{ }

//�޷��Ͳ���
public class EventInfo : IEventInfo
{
    public UnityAction eventAction;
}

//�з��Ͳ���_һ������
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> eventAction;
}

//�з��Ͳ���_��������
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

    //����¼�
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

    //�Ƴ��¼�
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

    //�����¼�
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

    //����¼�
    public void ClearEvents()
    {
        eventActionDic.Clear();
    }
}