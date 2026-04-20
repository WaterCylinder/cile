using System;
using Godot;
using Godot.Collections;

public enum CardSelectorMode
{
	ThreeChoiceOne,
}

public partial class CardSelector : Control
{
	[Export] public Array<Card> cards = new Array<Card>();
	[Export] public PackedScene cardPrefab;
	[Export] public Control selector;
	[Export] public Control cardNode;
	[Export] public TextureButton back;
	[Export] public Button confrim;

	private float backButtonTargetRotation = 0;

	public override void _Ready()
	{
		back.Pressed += () =>
		{
			if (selector.Visible)
			{
				selector.Visible = false;
				backButtonTargetRotation = 180;
			}
			else
			{
				selector.Visible = true;
				backButtonTargetRotation = 0;
			}
		};
	}
	
	public void Open()
	{
		GD.Print("打开卡牌选择界面");
	}

	public void StartSelect()
	{
		GD.Print("开始选择卡牌");
		Vector2 cardsCenter = cardNode.Position + cardNode.Size / 2;
		
	}

    public override void _Process(double delta)
	{
		float rotate = back.RotationDegrees;
		if (rotate != backButtonTargetRotation)
		{
			rotate += (backButtonTargetRotation - rotate) * (float)delta * 16;
			if (Math.Abs(rotate - backButtonTargetRotation) < 0.1f)
			{
				rotate = backButtonTargetRotation;
			}
			back.RotationDegrees = rotate;
		}
	}
}
