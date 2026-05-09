using Godot;
using Godot.Collections;

/// <summary>
/// UI标记，用于标记地区，标记位置等
/// </summary>
public partial class UIMarks : Node
{
	[Export]public Control terrainSelector;
	[Export]public Control unitSelector;
	[Export]public PackedScene unirSelectorMoveRangeBox;

	public Terrain lastTerrain = null;
	public Unit lastUnit = null;

	private bool isEventSet = false;

	private void ClearUnitSelector()
	{
		for (int i = unitSelector.GetChildCount() - 1; i >= 0; i--)
		{
			Node child = unitSelector.GetChild(i);
			child.QueueFree();
		}
	}

	public void ShowRange(Unit unit, Array<Terrain> range, Color color)
	{
		foreach (Terrain tr in range)
		{
			if (tr == null) continue;
			TextureRect box = unirSelectorMoveRangeBox.Instantiate() as TextureRect;
			unitSelector.AddChild(box);
			box.SelfModulate = color;
			box.SetSize(new Vector2(lastTerrain.width, lastTerrain.height));
			box.Position = tr.GlobalPosition;
			box.Position -= terrainSelector.PivotOffset;
		}
	}

	public void ShowUnitMoveRange(Unit unit)
	{
		ClearUnitSelector();
		if (unit == null)
		{
			//单位为空不处理
			return;
		}
		Array<Terrain> moveRange = GameManager.Instance.game.unitSystem.GetUnitMoveRangeTerrains(unit);
		ShowRange(unit, moveRange, new Color(0.5f, 1, 1, 0.5f));
	}

	public void ShowUnitBattleRange(Unit unit)
	{
		ClearUnitSelector();
		if (unit == null)
		{
			//单位为空不处理
			return;
		}
		Array<Terrain> battleRange = GameManager.Instance.game.unitSystem.GetUnitBattleRangeTerrains(unit);
		ShowRange(unit, battleRange, new Color(1, 0.5f, 0.4f, 0.5f));
	}

	/// <summary>
	/// 显示单位的可移动范围
	/// </summary>
	/// <param name="unit"></param>
	/*public void ShowUnitMoveRange(Unit unit)
	{

		if (GameManager.Instance.game.pui.selectedUI.Visible)
		{
			if (!GameManager.Instance.game.pui.selectedUI.isUnitSelected)
			{
				unitSelector.Visible = false;
			}
			else
			{
				unitSelector.Visible = true;
			}
		}
		else
		{
			unitSelector.Visible = false;
		}
		
		if(unit == lastUnit)
		{
			//相同单位不处理
			return;
		}

		if(unit == null)
		{
			//如果没有unit则移除所有的移动范围显示
			ClearUnitSelector();
		}
		else
		{
			Array<Terrain> moveRange = GameManager.Instance.game.unitSystem.GetUnitMoveRangeTerrains(unit);
			foreach (Terrain tr in moveRange)
			{
				if (tr == null) continue;
				TextureRect box = unirSelectorMoveRangeBox.Instantiate() as TextureRect;
				unitSelector.AddChild(box);
				box.SelfModulate = new Color(0.5f, 1, 1, 0.5f);
				box.SetSize(new Vector2(lastTerrain.width, lastTerrain.height));
				box.Position = tr.GlobalPosition;
				box.Position -= terrainSelector.PivotOffset;
			}
		}
		lastUnit = unit;
	}*/

	public void Init()
	{
		isEventSet = true;
		GameManager.Instance.game.OnTerrainSelected += (tr) =>
		{
			lastTerrain = tr;
		};
	}

	public override void _Process(double delta)
	{
		if(GameManager.Instance.game != null && !isEventSet)
		{
			Init();
		}
	}

}
