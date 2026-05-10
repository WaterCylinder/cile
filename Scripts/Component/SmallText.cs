using Godot;
using System;

public partial class SmallText : Control
{
	[Export]public Label label;
	[Export]public string state;
	[Export]public float maxScale;
	[Export]public float speed = 3;
	[Export]public float minY = -10;
	[Export]public float maxWaitTime = 1;

    public void Start()
    {
        state = "show";
    }

	public void SetScale(float scale)
	{
		label.Scale = new Vector2(scale, scale);
	}

	public void SetY(float newY)
	{
		label.SetPosition(new Vector2(label.Position.X, newY));
	}

	float timer = 0;

    public override void _Process(double delta)
    {
		float deltatime = (float)delta;
		switch (state)
		{
			case "show":
				float posY = label.Position.Y;
				posY -= deltatime * speed;
				if(posY < minY)
				{
					state = "wait";
					timer = 0;
				}
				SetY(posY);
			break;
			case "wait":
				timer += deltatime;
				if(timer > maxWaitTime)
				{
					state = "end";
					QueueFree();
				}
			break;
		}
    }

	public void SetText(string text)
	{
		label.Text = text;
	}


}
