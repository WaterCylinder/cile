using Godot;
using System;
using System.Diagnostics;

public partial class MainMenu : Control
{
	[Export]public TextureButton StartGameButton;
	[Export]public TextureButton SettingButton;
	[Export]public TextureButton HandBookButton;

	public override void _Ready()
	{
		StartGameButton.Pressed += OnStartGameButtonPressed;
		SettingButton.Pressed += OnSettingButtonPressed;
		HandBookButton.Pressed += OnHandBookButtonPressed;
	}
	public void OnStartGameButtonPressed()
    {
		GD.Print("开始游戏");
		GameManager.SM.CheckForward("GameMenu");
	}

	public void OnSettingButtonPressed()
    {
		GD.Print("进入设置");
		GameManager.SM.CheckForward("Setting");
	}
	public void OnHandBookButtonPressed()
    {
		GD.Print("退出游戏");
		GameManager.SM.CheckForward("HandBookMenu");
	}
}
