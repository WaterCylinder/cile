using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

/*
TODO:
2. 做全局随机数生成器，种子以来并将其改到deck的抽取逻辑里
1. 实现卡牌选择器的卡牌生成，步骤是先从deck里抽取cardCount张卡牌，那之后生成卡牌对象，将卡牌对象分配位置

卡牌选择系统暂时不做，地形卡牌直接在地形卡组里抽（此脚本暂时废弃）
*/

public partial class CardSelector : Control
{
	[Export] public PackedScene cardPrefab;
	[Export] public Control selector;
	[Export] public Control cardNode;
	[Export] public TextureButton back;
	[Export] public Button confrim;
	[ExportCategory("卡牌选择器数据")]
	[Export] public int maxSelect = 1;
	[Export] public int cardCount = 3;
	[Export] string randomGenerator;
	[Export] Array<CardData> cardTemp = new();
	[Export] Array<CardData> cardSelected = new();

	public Deck deck;

	private float backButtonTargetRotation = 0;

	public override void _Ready()
	{
		back.Pressed += () =>
		{
			if (selector.Visible)
			{
				//设置卡牌选择器为不可见
				selector.Visible = false;
				backButtonTargetRotation = 180;
			}
			else
			{
				//设置卡牌选择器为可见
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
		if(cardCount > deck.Count)
		{
			GD.Print($"卡牌选择功能：卡组不足【{cardCount}】张，仅有【{deck.Count}】张");
			cardCount = deck.Count;
		}
		if(maxSelect > cardCount)
		{
			GD.Print($"卡牌选择功能：最大选择数量【{maxSelect}】大于卡组数量【{cardCount}】，已调整为【{cardCount}】");
			maxSelect = cardCount;
		}
		GameManager.Instance.game.randomSystem.SetRandomGenerator(generatorName: randomGenerator);
		Random r = GameManager.Instance.game.randomSystem.GetGenerator(randomGenerator);
		GD.Print("开始选择卡牌");
		Vector2 cardsCenter = cardNode.Position + cardNode.Size / 2;
		//抽取N张卡牌
		Deck temp = deck.Clone();
		cardTemp = new();
		cardSelected = new();
		for (int i = 0; i < cardCount; i++)
		{
			CardData card = temp.RandomPick(r, DeckPickMode.PickAndRemove);
			cardTemp.Add(card);
		}

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
