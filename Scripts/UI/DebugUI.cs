using Godot;
using System;

public partial class DebugUI : Node
{
	[Export] public Button nextRoundButton;

	public override void _Ready()
	{
		nextRoundButton.Pressed += () =>
        {
            GameManager.Instance.game.roundCricle.NextTurn();
        };
	}
}
