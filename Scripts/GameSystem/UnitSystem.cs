using Godot;
using Godot.Collections;
using System;

/// <summary>
/// 单位管理系统，用于游戏局内单位的行为
/// </summary>
public partial class UnitSystem
{
	PackedScene packedScene;

	public Array<Unit> unitList = new Array<Unit>();

	/// <summary>
	/// 当前玩家
	/// </summary>
	private Player PlayerNow => GameManager.Instance.game.roundCricle.turnLogic.turnPlayer;
	public void Init()
	{
		GD.Print("单位管理系统初始化");
		packedScene = AssetManager.GetDefaultObject("unit");
	}

	/// <summary>
	/// 放置单位到指定区域
	/// </summary>
	public void PutUnit(UnitData unitData, Terrain terrain)
	{	
		if (terrain.unit != null)
		{
			GD.Print($"区域{terrain.Name}已经有单位了");
			return;
		}
		Unit unit = packedScene.Instantiate<Unit>();
		unit.data = unitData;
		unit.Init();
		GameManager.Instance.game.unitNode.AddChild(unit);
		unitList.Add(unit);
		unit.MoveTo(terrain);
	}

	public void SelectReadyTerrain(Terrain terrain, bool isBig = true)
	{
		//选择准备区域
		UnitData data = isBig ? PlayerNow.unitBig : PlayerNow.unitSmall;
		PutUnit(data, terrain);
	}
}
