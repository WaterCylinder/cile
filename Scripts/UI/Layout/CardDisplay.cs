using Godot;
using System;

public partial class CardDisplay : Control
{
	[ExportCategory("组件")]
	[Export]public Control card;
	[Export]public TextureRect cardFace;
	[ExportCategory("属性")]
	[Export]public string state;

	public Card cardNow;

	float timer = 0;

    public override void _Ready()
    {
		Init();
    }

	void SetCardSize(float size)
	{
		card.Scale = new(size, size);
	}

	public void Init()
	{
		state = "ready";
		SetCardSize(0);
		card.Visible = false;
	}

	public void Show(Card card)
	{
		Init();
		cardNow = card;
		this.card.Visible = true;
		if(card.sprite != null)
		{
			cardFace.Texture = card.sprite;
		}
		else
		{
			cardFace.Texture = AssetManager.GetDefaultSprite("card");
		}
		
		state = "show";
	}

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.Pressed)
			{
				if (state == "wait")
				{
					state = "back";
				}
			}
		}
    }


    public override void _Process(double delta)
    {
        if(state == "show")
		{
			//开始展示
			float size = card.Scale.X;
			size += (float)delta * 5;
			SetCardSize(size);
			if(size > 1.28)
			{
				timer = 0;
				state = "wait";
			}
		}
		else if(state == "wait")
		{
			//等待
			timer += (float)delta;
			if(timer >= 3) // 三秒钟自动收起
			{
				state = "back";
			}
		}
		else if(state == "back")
		{
			float size = card.Scale.X;
			size -= (float)delta * 5;
			SetCardSize(size);
			if(size <= 0)
			{
				Init();
			}
		}
    }

}
