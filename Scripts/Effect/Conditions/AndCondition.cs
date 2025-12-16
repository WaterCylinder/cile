using Godot;
using Godot.Collections;

/// <summary>
/// 与条件集
/// </summary>
[GlobalClass]
public partial class AndCondition : Condition
{
	[Export]public Array<Condition> conditionArray = new();

	public override bool Run()
	{	
		result = true;
		foreach(Condition c in conditionArray)
		{
			if (!c.Run())
			{
				result = false;
				return result;
			}
		}
		return result;
	}

}