using Godot;
using System;

public partial class Canvas : Node
{
	[Export]public Vector2 windowSize;
	[Export]public Vector2 center;

    public override void _EnterTree()
	{
		windowSize = GetViewport().GetVisibleRect().Size;
		center = windowSize / 2;
	}

}
