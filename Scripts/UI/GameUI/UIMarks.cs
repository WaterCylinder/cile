using Godot;
using System;

/// <summary>
/// UI标记，用于标记地区，标记位置等
/// </summary>
public partial class UIMarks : Node
{
	[Export]public Control selector;

	public Terrain lastTerrain = null;

    public override void _Process(double delta)
    {
        base._Process(delta);
		if (GameManager.Instance.game.selectedTerrains.Count <= 0)
		{
			selector.Visible = false;
			return;
		}
		if (GameManager.Instance.game.selectedTerrains[0] != lastTerrain)
		{
			selector.Visible = true;
			lastTerrain = GameManager.Instance.game.selectedTerrains[0];
			selector.SetSize(new Vector2(lastTerrain.width, lastTerrain.height));
			selector.Position = lastTerrain.GlobalPosition;
			selector.Position -= selector.PivotOffset;
		}
    }

}
