using Godot;
using System;
using System.Diagnostics;

public partial class MainMenu : Control
{
	[Export]public Button StartGameButton;
	[Export]public Button SettingButton;
	[Export]public Button ExitButton;

	public override void _Ready()
	{
		StartGameButton.Pressed += OnStartGameButtonPressed;
		SettingButton.Pressed += OnSettingButtonPressed;
		ExitButton.Pressed += OnExitButtonPressed;
	}
	public void OnStartGameButtonPressed()
    {
		GD.Print("开始游戏");
	}

	public void OnSettingButtonPressed()
    {
		GD.Print("进入设置");
	}
	public void OnExitButtonPressed()
    {
		GD.Print("退出游戏");
	}
}
