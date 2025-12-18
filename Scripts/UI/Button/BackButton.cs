using Godot;
using System;

public partial class BackButton : Button
{
	[Export] public Button button;

	public override void _Ready()
	{
		button.Pressed += Back;
	}

	public void Back()
    {
        GameManager.SM.CheckBack();
    }
}
