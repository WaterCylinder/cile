using Godot;
using System;

public partial class ShowingCard : Control
{
	[Export]public TextureRect sprite;
	[Export]public string cardName = "";
	[Export]public string cardInfo = "";
	[Export]public string state = "ready";

	[Export]public bool latest = false;
	
	float timer = 0;

	public CardDisplay display;

	void SetSize(float size)
	{
		Scale = new(size, size);
	}

	public void Start(Card card)
	{
		state = "show";
		SetSize(0);
		timer = 0;
		if(card != null)
		{
			sprite.Texture = card.sprite;
			cardName = card.cardName;
			cardInfo = card.cardInfo;
		}
	}

    public override void _Process(double delta)
    {
		float timeDelta = (float)delta;
        if(state == "show")
		{
			float size = Scale.X;
			size += timeDelta * display.showSpeed;
			if(size >= display.maxSize)
			{
				size = 1;
				state = "wait";
				timer = 0;
			}
			SetSize(size);
		}
		else if(state == "wait")
		{
			timer += timeDelta;
			if(timer >= display.waitTime)
			{
				state = "back";
			}
		}
		else if(state == "back")
		{
			float size = Scale.X;
			size -= timeDelta * display.showSpeed;
			if(size <= 0)
			{
				size = 0;
				state = "ready";
				if (latest)
				{
					display.OnShowEnd();
				}
				QueueFree();
			}
			SetSize(size);
		}
    }

}
