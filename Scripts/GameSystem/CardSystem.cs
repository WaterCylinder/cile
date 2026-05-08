using Godot;
using System;
using System.Collections.Generic;

public class CardSystem
{

	public Dictionary<string, Deck> decks = new();

	/// <summary>
	/// 资源卡组
	/// </summary>
	public Deck resourceDeck;
	/// <summary>
	/// 地形卡组
	/// </summary>
	public Deck terrainDeck;

	public Deck GetDeck(string deckName)
	{
		Deck deck = CardManager.Instance.LoadDeck(deckName);
		if (! decks.ContainsKey(deck.deckName))
		{
			decks.Add(deck.deckName, deck);
		}
		return decks[deck.deckName];
	}

	/// <summary>
	/// 加载资源卡组
	/// </summary>
	/// <param name="deckName"></param>
	public void LoadResourceDeck(string deckName)
	{
		resourceDeck = GetDeck(deckName);
	}

	/// <summary>
	/// 加载地形卡组
	/// </summary>
	/// <param name="deckName"></param>
	public void LoadTerrainDeck(string deckName)
	{
		terrainDeck = GetDeck(deckName);
	}

	/// <summary>
	/// 抽卡
	/// </summary>
	/// <param name="deck"></param>
	/// <returns></returns>
	public Card PickCard(Deck deck, Player player = null)
	{
		if(deck == null)
		{
			GD.Print("卡组为null");
			return null;
		}
		CardData data = deck.RandomPick(GameManager.Instance.game.randomSystem.GetDefaultGenerator());
		Card card = CardManager.Instance.CreateCard(data);
		//Card抽取事件
		if(player != null)
		{
			card.owner = player;
		}
		card.OnPick();
		return card;
	}

	public Card PickResourceCard(Player player = null)
	{
		return PickCard(resourceDeck, player);
	}

	public Card PickTerrainCard(Player player = null)
	{
		return PickCard(terrainDeck, player);
	}
}
