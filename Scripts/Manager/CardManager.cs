using Godot;
using System;
using System.Collections.Generic;

public enum DeckPickMode
{
	/// <summary>
	/// 仅获取pick信息
	/// </summary>
	OnlyPick,
	/// <summary>
	/// pick并且减少一次卡牌数量
	/// </summary>
	PickReduceOne,
	/// <summary>
	/// pick并且移除pick到的卡牌
	/// </summary>
	PickAndRemove,
}

/// <summary>
/// 卡组类
/// </summary>
public class Deck
{
	public Dictionary<CardData, uint> cards = new();
	/// <summary>
	/// 卡组卡牌总数量
	/// </summary>
	public int TotalCount
	{
		get
		{
			int c = 0;
			foreach(var v in cards)
			{
				c += (int)v.Value;
			}
			return c;
		}
	}
	
	/// <summary>
	/// 卡组卡牌数量（种类）
	/// </summary>
	public int Count
	{
		get
		{
			return cards.Count;
		}
	}
	/// <summary>
	/// 复制卡组，用于卡组抽牌
	/// </summary>
	/// <returns></returns>
	public Deck Clone()
	{
        Deck deck = new()
        {
            cards = new(cards)
        };
        return deck;
	}
	
	/// <summary>
	/// 向卡组里添加卡牌
	/// </summary>
	/// <param name="cardData"></param>
	/// <param name="count"></param>
	public void Add(CardData cardData, uint count = 1)
    {
		if (cards.ContainsKey(cardData))
		{
			cards[cardData] += count;
		}
		else
		{
			cards.Add(cardData, count);
		}
	}

	/// <summary>
	/// 获取并从卡组里移除卡牌
	/// </summary>
	/// <param name="cardData"></param>
	/// <param name="count"></param>
	/// <returns></returns>
	public CardData Pick(CardData cardData, uint count = 1)
	{
		if(cards.ContainsKey(cardData))
		{
			cards[cardData] -= count;
			if(cards[cardData] <= 0)
			{
				cards.Remove(cardData);
			}
			return cardData;
		}
		return null;
	}

	/// <summary>
	/// 获取并从卡组里移除所有该卡牌
	/// </summary>
	/// <param name="cardData"></param>
	/// <returns></returns>
	public (CardData, uint) PickAll(CardData cardData)
	{
		if (cards.ContainsKey(cardData))
		{
			uint count = cards[cardData];
			return (Pick(cardData, count), count);
		}
		return (null, 0);
	}

	/// <summary>
	/// 随机抽取卡牌，可选是否移除
	/// </summary>
	/// <param name="random">随机数生成器</param>
	/// <param name="remove">移除</param>
	/// <returns></returns>
	public CardData RandomPick(Random random = null, DeckPickMode mode = DeckPickMode.OnlyPick)
	{	
		if(random == null)
		{
			random = new Random((int)DateTime.Now.Ticks);
		}
		int totalWeight = 0;
		foreach(var item in cards)
		{
			totalWeight += (int)item.Value;
		}
		int randomWeight = random.Next(0, totalWeight + 1);
		CardData result = null;
		foreach(var item in cards)
		{
			result = item.Key;
			randomWeight -= (int)item.Value;
			if(randomWeight <= 0)
			{
				break;
			}
		}
		switch (mode)
		{
			case DeckPickMode.PickReduceOne:
				Pick(result);
				break;
			case DeckPickMode.PickAndRemove:
				PickAll(result);
				break;
		}
		return result;
	}
}

/*
 * 卡牌管理器
*/
public partial class CardManager : Node
{
	private static CardManager instance;
	public static CardManager Instance
    {
        get
        {
            if(instance == null)
            {
                try
				{
					GD.Print("Core：正在初始化卡牌管理器");
                    instance = new CardManager
                    {
                        Name = "CardManager",
                        ProcessMode = ProcessModeEnum.Always
                    };
                    GameManager.Instance.AddChild(instance);
				}
				catch(Exception e)
				{
					GD.PrintErr($"Core：初始化卡牌管理器失败！{e}");
				}
            }
			return instance;
        }
    }

	public void Init()
    {
        
    }

}
