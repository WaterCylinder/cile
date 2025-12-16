using Godot;
using Godot.Collections;
/// <summary>
/// 或条件集
/// </summary>
[GlobalClass]
public partial class OrCondition : Condition
{
	[Export]public Array<Condition> conditionArray = new();

	public override bool Run()
	{	
		result = false;
		foreach(Condition c in conditionArray)
		{
			if (c.Run())
			{
				result = true;
				return result;
			}
		}
		return result;
	}

}