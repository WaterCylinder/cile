using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

/*
玩家实体类，意思是游戏内的玩家数据，游戏外的玩家数据使用User类
*/

public enum PlayerState
{
    None,
}
public partial class Player : Node
{	
	[Export]public string id;
	/// <summary>
    /// 角色数据
    /// </summary>
	[Export]public CharacterData characterData;
	/// <summary>
    /// 行动点数
    /// </summary>
	[Export]public int activatePoint;
	/// <summary>
    /// 玩家状态
    /// </summary>
	[Export]public PlayerState state;
	/// <summary>
    /// 资源点
    /// </summary>
	[Export]public int resourcesPoint;
	/// <summary>
    /// 生命值
    /// </summary>
	[Export]public int healthPoint;
	/// <summary>
    /// 死亡次数
    /// </summary>
	[Export]public int death;
	/// <summary>
    /// 手卡
    /// </summary>
	[Export]public Array<CardData> handCards;

    [Export]public UnitData unitBig;
    [Export]public UnitData unitSmall;

	public User user;

	public void Init()
    {	
		//从角色数据里获取初始行动点数等数据
		if (characterData == null)
        {
            characterData = AssetManager.GetDefaultData("character") as CharacterData;
        }
		activatePoint = characterData.activatePoint;
        if(characterData.unitBigName != "")
        {
            GD.Print(characterData.unitBigName);
            unitBig = AssetManager.Instance.GetData("Units." + characterData.unitBigName) as UnitData;
        }
        if(unitBig == null)
        {
            unitBig = AssetManager.GetDefaultData("unit") as UnitData;
        }
        if(characterData.unitSmallName != "")
        {
            unitSmall = AssetManager.Instance.GetData("Units." + characterData.unitSmallName) as UnitData;
        }
        if(unitSmall == null)
        {
            unitSmall = AssetManager.GetDefaultData("unit") as UnitData;
        }
    }
}
