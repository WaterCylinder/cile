using Godot;
using System;

[GlobalClass]
public partial class UnitBehavior : Behavior
{
	public Unit unit;

	# region 单位事件

	public void UnitLog()
	{
		GD.Print(unit.Name);
	}

	# endregion

	# region 单位条件

	# endregion
}
