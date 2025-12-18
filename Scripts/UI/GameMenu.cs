using Godot;
using System;

public partial class GameMenu : Control
{
	[Export] public Button StartTestGame;

	public override void _Ready()
	{
		StartTestGame.Pressed += OnStartTestGamePressed;
	}

	public void OnStartTestGamePressed()
	{	
		GD.Print("开始测试游戏");
		GameManager.SM.CheckForward("DemoGame");
	}
}
