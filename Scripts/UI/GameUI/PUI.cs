using Godot;
using System;

public partial class PUI : Control
{
	[Export]public SelectedUI selectedUI;

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
				if (!selectedUI.Visible)
				{
					selectedUI.Visible = true;
				}
			};
			GameManager.Instance.game.OnTerrainSelectedCancle += () =>
			{
				if (selectedUI.Visible)
				{
					selectedUI.Visible = false;
				}
			};
			GameManager.Instance.game.mainCamera.OnScreenStartMove += () =>
			{
				if (selectedUI.Visible)
				{
					//selectedUI.Visible = false;
				}
			};
		}
		if (selectedUI.Visible)
		{
			Vector2 pos = GameManager.Instance.game.selectedTerrains[0].GlobalPosition;
			pos += new Vector2(GameManager.Instance.game.selectedTerrains[0].width/2, -GameManager.Instance.game.selectedTerrains[0].height/2);
			pos = GameManager.Instance.game.mainCamera.ToScreenPosition(pos);
			selectedUI.GlobalPosition = pos;
		}
    }


}
