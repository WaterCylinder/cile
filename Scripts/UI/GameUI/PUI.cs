using Godot;
using System;

public partial class PUI : Control
{
	[Export]public SelectedUI selectedUI;
	[Export]public BattleUI battleUI;

	private Game game => GameManager.Instance.game;

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
					game.CancelTerrainSelect();
				}
			}
		}
    }

    public override void _Process(double delta)
    {
  		if (game != null && !isEventSet)
		{
			isEventSet = true;
		}
		if (selectedUI.Visible)
		{
			Vector2 pos = game.selectedTerrains[0].GlobalPosition;
			pos += new Vector2(game.selectedTerrains[0].width/2, -game.selectedTerrains[0].height/2);
			pos = game.mainCamera.ToScreenPosition(pos);
			selectedUI.GlobalPosition = pos;
		}
    }


}
