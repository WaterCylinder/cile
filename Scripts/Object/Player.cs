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
	[Export]public string Id{get;set;}
	[Export]public PlayerState State{get;set;}
	/// <summary>
    /// 资源点
    /// </summary>
	[Export]public int ResourcesPoint{get;set;}
	/// <summary>
    /// 生命值
    /// </summary>
	[Export]public int HealthPoint{get;set;}
	/// <summary>
    /// 死亡次数
    /// </summary>
	[Export]public int Death{get;set;}
	/// <summary>
    /// 手卡
    /// </summary>
	[Export]public Array<CardData> HandCards{get;set;}

	public User user;

	public void Init()
    {
        
    }
}
