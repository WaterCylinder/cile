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

	#region 选择模式

	#endregion

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
		Player player = GameManager.Instance.game.CurrentPlayer;
		unit.player = player;
	}
	
	/// <summary>
	/// 获取以某个单位为中心符合范围列表的地形列表
	/// </summary>
	/// <param name="unit"></param>
	/// <param name="range"></param>
	/// <returns></returns>
	public Array<Terrain> GetUnitRangeTerrains(Unit unit, Array<Vector2> range, Func<Terrain, bool> ignoreCondition = null)
	{
		Terrain terrain = unit.terrain;
		if(terrain == null)
		{
			GD.Print($"单位{unit}没有区域");
			return null;
		}
		Array<Terrain> terrains = new Array<Terrain>();
		foreach(Vector2 rangePos in range)
		{
			Vector2 pos = terrain.MapPos + rangePos;
			if(!terrain.map.isInMap(pos))
				//超出地图区域
				continue;
			Terrain targetTerrain = terrain.map.GetTerrain(pos);
			if(ignoreCondition != null && ignoreCondition.Invoke(targetTerrain))
			{
				continue;
			}
			if(targetTerrain.unit != null)
				//有单位的区块不能移动
				continue;
			
			terrains.Add(targetTerrain);
		}
		return terrains;
	}

	public bool TerrainRangeIgnoreConditionDefault(Terrain terrain)
	{
		if(terrain.CheckTag(TerrainTag.Blocked))
			return true;
		if(terrain.CheckTag(TerrainTag.Ready))
			return true;
		return false;
	}
	/// <summary>
	/// 获取单位移动范围内的所有地形对象
	/// </summary>
	/// <param name="unit"></param>
	/// <returns></returns>
	public Array<Terrain> GetUnitMoveRangeTerrains(Unit unit)
	{
		Array<Vector2> range = unit.moveRange;
		return GetUnitRangeTerrains(unit, range, t =>
		{
			bool result = TerrainRangeIgnoreConditionDefault(t);
			if(t.unit != null)
				return true;
			return result;
		});
	}
	/// <summary>
	/// 获取单位攻击范围内的所有地形对象
	/// </summary>
	/// <param name="unit"></param>
	/// <returns></returns>
	public Array<Terrain> GetUnitBattleRangeTerrains(Unit unit)
	{
		Array<Vector2> range = unit.battleRange;
		if(range == null)
		{
			range = unit.moveRange;
		}
		return GetUnitRangeTerrains(unit, range, TerrainRangeIgnoreConditionDefault);
	}

	public void SelectReadyTerrain(Terrain terrain, bool isBig = true)
	{
		//选择准备区域
		UnitData data = isBig ? PlayerNow.unitBig : PlayerNow.unitSmall;
		PutUnit(data, terrain);
	}

	/// <summary>
	/// 进入单位的地形选择模式
	/// </summary>
	/// <param name="unit"></param>
	public void StartTerrainSelectMode(Action<Terrain> callback)
	{
		Game game = GameManager.Instance.game;
		Action<Terrain> handler = null;
		handler = (t) =>
		{
			callback?.Invoke(t);
			game.OnTerrainSelected -= handler.Invoke;
		};
		game.OnTerrainSelected += handler.Invoke;
	}

}
