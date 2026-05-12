using Godot;

public partial class SmallText : Node2D
{
	[Export]public bool startOnLoad = false;
	[Export]public Node2D labelNode;
	[Export]public Label label;
	[Export]public string state;
	[Export]public float maxScale;
	[Export]public float speed = 3;
	[Export]public float minY = -10;
	[Export]public float maxWaitTime = 1;

    public override void _Ready()
    {
		if (startOnLoad)
		{
			Start();
		}
    }

    public void Start()
    {
        state = "show";
    }

	public void SetScale(float scale)
	{
		labelNode.Scale = new Vector2(scale, scale);
	}

	public void SetY(float newY)
	{
		labelNode.SetPosition(new Vector2(labelNode.Position.X, newY));
	}

	float timer = 0;

    public override void _Process(double delta)
    {
		float deltatime = (float)delta;
		switch (state)
		{
			case "show":
				float posY = labelNode.Position.Y;
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
					state = "small";
				}
			break;
			case "small":
				float scale = label.Scale.X;
				scale -= deltatime * 3;
				if(scale <= 0)
				{
					scale = 0;
					state = "end";
					QueueFree();
				}
				label.Scale = new Vector2(scale, scale);
			break;
		}
    }

	public void SetText(string text)
	{
		label.Text = text;
	}


}
