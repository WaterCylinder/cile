using Godot;
using System;

public class Card
{
	public CardData data;
	public string cardName;
	public string cardInfo;
	public CardType type;
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
		onUse = data.onUse;
		sprite = data.sprite;
	}
}
