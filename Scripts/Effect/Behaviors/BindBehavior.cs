using System.Collections.Generic;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class BindBehavior : Behavior
{
    [Export]public Array<Behavior> behaviorArray;
    public override T Run<T>()
    {
        behaviorName = "RunList";
        return base.Run<T>();
    }
    public List<object> RunList()
    {
        List<object> result = new(); 
        foreach (Behavior behavior in behaviorArray)
        {
            result.Add(behavior.Run<object>());
        }
        return result;
    }
}