
using System;
using System.Collections.Generic;
using System.Xml.XPath;


public class EventSheet<T> where T : MulticastDelegate 
{
    protected Dictionary<string, T> _events;

    public EventSheet(){
        _events = new();
    }

    public void Add(T func, string name = null)
    {
        if(name == null)
        {
            name = Guid.NewGuid().ToString();
        }
        _events.Add(name, func);
    }

    public List<string> GetEventIDs()
    {
        return [.. _events.Keys];
    }

    public T GetEvent(string id)
    {
        if (!_events.ContainsKey(id))
        {
            return null;
        }
        return _events[id];
    }

    //运算符重载
    public static dynamic operator + (EventSheet<T> sheet, T func)
    {
        sheet.Add(func);
        return sheet;
    }
}

public class BoolEventSheet : EventSheet<Func<bool, bool>>
{
    public bool Invoke(bool value)
    {
        bool result = value;
        foreach (Func<bool, bool> func in _events.Values)
        {
            result = func(result);
        }
        return result;
    }
}