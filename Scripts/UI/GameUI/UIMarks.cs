using Godot;
using System;

/// <summary>
/// UI标记，用于标记地区，标记位置等
/// </summary>
public partial class UIMarks : Node
{
	[Export]public Control selector;

	public Terrain lastTerrain = null;

	private bool isEventSet = false;

	public override void _Process(double delta)
	{
		if(GameManager.Instance.game != null && !isEventSet)
		{
			isEventSet = true;
			GameManager.Instance.game.OnTerrainSelected += (tr) =>
			{
				selector.Visible = true;
				lastTerrain = tr;
				selector.SetSize(new Vector2(lastTerrain.width, lastTerrain.height));
				selector.Position = lastTerrain.GlobalPosition;
				selector.Position -= selector.PivotOffset;
			};
			GameManager.Instance.game.OnTerrainSelectedCancle += () =>
			{
				selector.Visible = false;
			};
		}
	}

}
