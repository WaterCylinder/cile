using Godot;
using Godot.Collections;

/// <summary>
/// 条件类
/// </summary>
[GlobalClass]
public partial class SingalCondition : Condition
{
	[Export]public Behavior behavior;
	
	public override bool Run()
	{
		result = behavior.Run<bool>();
		return result;
	}
}

