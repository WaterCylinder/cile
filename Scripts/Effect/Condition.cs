using Godot;
using Godot.Collections;

[GlobalClass]
public abstract partial class Condition : Resource
{
	[Export]public string info;
	[Export]public bool result;
	public virtual bool Run()
	{
		return false;
	}
}



