using Godot;
using System;
using System.Collections.Generic;


/// <summary>
/// 卡组类
/// </summary>
public class Deck
{
	public Dictionary<CardData, uint> cards = new();
	/// <summary>
	/// 卡组卡牌数量
	/// </summary>
	public int count
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
	/// 随机抽取卡牌，可选是否移除
	/// </summary>
	/// <param name="remove"></param>
	/// <returns></returns>
	public CardData RandomPick(bool remove = false)
	{
		uint totalWeight = 0;
		foreach(var item in cards)
		{
			totalWeight += item.Value;
		}
		uint randomWeight = (uint)GD.RandRange(0, totalWeight);
		CardData result = null;
		foreach(var item in cards)
		{
			result = item.Key;
			randomWeight -= item.Value;
			if(randomWeight <= 0)
			{
				break;
			}
		}
		if(remove && result != null)
		{
			Pick(result);
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
					instance = new CardManager();
					instance.Name = "CardManager";
					instance.ProcessMode = ProcessModeEnum.Always;
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
