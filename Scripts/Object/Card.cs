using Godot;
using System;

public class Card
{
	public Player owner;

	//资源属性
	public CardData data;
	public string cardName;
	public string cardInfo;
	public CardType type;
	public Effect onPick;
	public Effect onUse;
	public Texture2D sprite;

	public void Init(CardData _data = null)
	{
		if(_data!= null)
		{
			data = _data;
		}
		cardName = data.cardName;
		cardInfo = data.cardInfo;
		type = data.type;
		onPick = data.onPick;
		onUse = data.onUse;
		sprite = data.sprite;
	}

	public void OnPick()
	{
		GD.Print($"{onPick}");
		if(onPick == null)
		{
			return;
		}
		GD.Print($"卡牌{cardName}抽取时效果触发");
		CardBehavior behavior = onPick.behavior as CardBehavior;
		CardBehavior condition = onPick.condition.behavior as CardBehavior;
		if(behavior != null)
		{
			behavior.card = this;
		}
		if(condition != null)
		{
			condition.card = this;
		}
		onPick?.Run();
	}

	public void OnUse()
	{
		if(onUse == null)
		{
			return;
		}
		GD.Print($"卡牌{cardName}使用时效果触发");
		CardBehavior behavior = onUse.behavior as CardBehavior;
		CardBehavior condition = onUse.condition.behavior as CardBehavior;
		if(behavior != null)
		{
			behavior.card = this;
		}
		if(condition != null)
		{
			condition.card = this;
		}
		onUse?.Run();
	}
}
