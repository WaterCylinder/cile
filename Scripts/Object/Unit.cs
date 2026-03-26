using Godot;
using System;

/*
玩家可操控单位
*/

public partial class Unit : Node2D
{
	[Export] public Sprite2D sprite;
	[Export] public UnitData data;
	[ExportCategory("所在地形")]
	[Export] public Terrain terrain;
	
	public void Init()
	{
		if(data == null)
		{
			GD.PushWarning("单位数据为空");
			data = AssetManager.GetDefaultData("unit") as UnitData;
		}
		//根据单位数据初始化单位对象
		
	}

	/// <summary>
	/// 移动到指定地形
	/// </summary>
	/// <param name="terrain"></param>
	public void MoveTo(Terrain target)
	{
		if(target.unit != null)
		{
			GD.PushWarning($"目标地形{target.Name}已有单位");
			return;
		}
		if(terrain != null)
			terrain.unit = null;
		terrain = target;
		terrain.unit = this;
		//移动逻辑，只简单做一个移动位置
		ResetPosition();
	}

	/// <summary>
	/// 重置位置
	/// </summary>
	public void ResetPosition()
	{
		Position = terrain.GlobalPosition;
	}
}
