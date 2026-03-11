using Godot;
using System;

public partial class HandBookMenu : Control
{
	[Export] public ColorRect selectRateBar;
	[Export] public TextureButton backButton;
	[Export] public TextureButton enemyButton;
	[Export] public TextureButton cgButton;
	[Export] public TextureButton cardsButton;
	[Export] public TextureButton settingButton;
	[Export] public TextureButton wordsButton;
	[Export] public TextureButton achivementButton;
	[Export] public TextureButton musicButton;

	public override void _Ready()
    {
        backButton.Pressed += () =>
        {
			GameManager.SM.CheckBack();
        };
    }

}
