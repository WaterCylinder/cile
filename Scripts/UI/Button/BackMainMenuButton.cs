using Godot;
using System;

public partial class BackMainMenuButton : Button
{
	[Export] public Button button;

	public override void _Ready()
	{
		button.Pressed += BackToMainMenu;
	}

	public void BackToMainMenu()
    {
        GameManager.SM.CheckTo();
    }
}
