using Godot;
using System;

[GlobalClass]
public partial class Effect : Resource
{
    [Export]public string effectName;
    [Export]public Condition condition;
    [Export]public Behavior behavior;
    public void Run()
    {
        if (condition.Run())
        {
            behavior.Run();
        }
    }
}
