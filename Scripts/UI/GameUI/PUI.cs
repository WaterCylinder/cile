using Godot;
using System;

public partial class PUI : Control
{
	[Export]public Control terrainSelectUI;

	//设置事件
	private bool isEventSet = false;

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseButton mouseEvent)
		{
			if(mouseEvent.ButtonIndex == MouseButton.Right)
			{
				if (!mouseEvent.Pressed)
				{
					//右键清除当前选择
					GameManager.Instance.game.selectedTerrains.Clear();
					GameManager.Instance.game.EmitSignal("OnTerrainSelectedCancle");
				}
			}
		}
    }

    public override void _Process(double delta)
    {
  		if (GameManager.Instance.game != null && !isEventSet)
		{
			isEventSet = true;
			GameManager.Instance.game.OnTerrainSelected += (tr) =>
			{
				if (!terrainSelectUI.Visible)
				{
					terrainSelectUI.Visible = true;
				}
			};
			GameManager.Instance.game.OnTerrainSelectedCancle += () =>
			{
				if (terrainSelectUI.Visible)
				{
					terrainSelectUI.Visible = false;
				}
			};
			GameManager.Instance.game.mainCamera.OnScreenStartMove += () =>
			{
				if (terrainSelectUI.Visible)
				{
					//terrainSelectUI.Visible = false;
				}
			};
		}

		if (terrainSelectUI.Visible)
		{
			Vector2 pos = GameManager.Instance.game.selectedTerrains[0].GlobalPosition;
			pos += new Vector2(GameManager.Instance.game.selectedTerrains[0].width/2, -GameManager.Instance.game.selectedTerrains[0].height/2);
			pos = GameManager.Instance.game.mainCamera.ToScreenPosition(pos);
			terrainSelectUI.GlobalPosition = pos;
		}
    }


}
