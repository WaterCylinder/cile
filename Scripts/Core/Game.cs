using Godot;
using System;

public partial class Game : Node
{
	[Export] public Control UI;
	[Export] public Node2D scene;
	[Export] public PackedScene innerMenuPackedScene;
	[Export] public Control innerMenu;

	public override void _Ready()
    {
		// 设置Game对象
        GameManager.Instance.game = this;
		// 监听按键按下
		GameManager.IM.KeyUp += (keyCode) =>
        {
            if(keyCode == OS.GetKeycodeString(Key.Escape))
            {
                if(innerMenu == null)
                {
                    OpenMenu();
                }
                else
                {
                    CloseMenu();
                }
            }
        };
    }

	public void OpenMenu()
    {
        innerMenu = innerMenuPackedScene.Instantiate<Control>();
		UI.AddChild(innerMenu);
    }

	public void CloseMenu()
    {
        innerMenu.QueueFree();
		innerMenu = null;
    }

	public override void _Input(InputEvent @event)
    {
		if(@event is InputEventKey)
        {
			
		}
    }
}
