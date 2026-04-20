using Godot;
using System;

[GlobalClass]
public partial class EventBehavior : Behavior
{
	public Action action;
	public void Execute()
	{
		action?.Invoke();
	}
}
