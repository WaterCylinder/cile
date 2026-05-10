using Godot;
using Godot.Collections;

public partial class BattleUI : Control
{
	Game game => GameManager.Instance.game;
	BattleSystem system => game.battleSystem;
	
	[Export] public Control buttonNode;
	[Export] public PackedScene buttonPrefab;
	[Export] public float interval = 200;
	[Export] public Array<UnitBattleBehavior> behaviors;
	[Export] public UnitBattleBehavior exitBehavior;

	float timer = 0;

	public void Start()
	{
		GD.Print("战斗UI初始化");
		Position = game.canvas.center;
		GenerateButton();
	}

	public void ClearButton()
	{
		//清空所有按钮
		foreach (Node child in buttonNode.GetChildren())
		{
			child.QueueFree();
		}
	}

	public void CreatButton(Vector2 pos, UnitBattleBehavior behavior)
	{
		TextureButton button = buttonPrefab.Instantiate<TextureButton>();
		buttonNode.AddChild(button);
		button.Position = pos;
		button.Pressed += () =>
		{
			system.AddOperation(
				action: (u, o) =>
				{
					behavior.unit = u;
					behavior.other = o;
					behavior.Run();
				},
				rollback: (u, o) =>
				{
					string rollbackName = behavior.behaviorName + "Rollback";
					behavior.unit = u;
					behavior.other = o;
					behavior.Run(rollbackName);
				}
			);
		};
		button.GetChild<Label>(0).Text = behavior.displayName;
	}

	public void GenerateButton()
	{
		ClearButton();
		//暂时做成在屏幕右边生成一排按钮的样式
		float posX = -400;
		int count = behaviors.Count + 1;
		float posY = -(float)count / 2 * interval;
		foreach(UnitBattleBehavior behavior in behaviors)
		{
			CreatButton(new Vector2(posX, posY), behavior);
			posY += interval;
		}
		CreatButton(new Vector2(posX, posY), exitBehavior);
	}

    public override void _Process(double delta)
    {
		//在UI这里0.5s执行一次战斗系统的指令模拟战斗动画
		timer += (float)delta;
		if(timer >= 0.5)
		{
			system?.RunNext();
			timer = 0;
		}
    }


}
