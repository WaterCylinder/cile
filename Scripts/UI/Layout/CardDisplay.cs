using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class CardDisplay : Control
{
	[Export]public Control cardNode;
	[Export]public PackedScene showingCardPrefab;
	[Export]public Array<ShowingCard> showingCardList;
	[Export]public int maxCount = 5;
	[Export]public float interval = 220;
	[ExportCategory("属性")]
	[Export]public float showSpeed = 6;
	[Export]public float waitTime = 2;
	[Export]public float maxSize = 1;

	public Stack<List<Card>> cardStack = new();

	public void CreatShowingCard(Card card, Vector2 pos)
	{
		ShowingCard showingCard = showingCardPrefab.Instantiate<ShowingCard>();
		cardNode.AddChild(showingCard);
		showingCard.Position = pos;
		showingCard.display = this;
		showingCard.Start(card);
	}

	public void Show(List<Card> cards)
	{
		//设置到画面中心位置
		cardNode.Position = GameManager.Instance.game.canvas.center;
		cardStack.Clear();
		if(cards.Count <= maxCount)
		{
			cardStack.Push(cards);
		}
		else
		{
			List<Card> temp = new();
			foreach(Card card in cards)
			{
				temp.Add(card);
				if(temp.Count >= maxCount)
				{
					List<Card> temp2 = [.. temp];
					cardStack.Push(temp2);
					temp.Clear();
				}
			}
		}
		ShowNext();
	}

	public void Show(Card card)
	{
		Show([card]);
	}

	public void ShowNext()
	{
		List<Card> cards = cardStack.Pop();
		int count = cards.Count - 1;
		float posx = ((float)count / 2 - count) * interval;
		foreach(Card card in cards)
		{
			CreatShowingCard(card, new Vector2(posx, 0));
			posx += interval;
		}
		//TODO:根据显示的卡牌数量设置卡牌节点大小
	}

	public void OnShowEnd()
	{
		if(cardStack.Count > 0)
		{
			ShowNext();
		}
		GD.Print("所有卡牌展示完毕！");
	}

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseButton mouseEvent)
		{
			
		}
    }

}
