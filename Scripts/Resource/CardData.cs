using Godot;
using System;

/// <summary>
/// 卡片类
/// </summary>
public enum CardType
{	
	/// <summary>
    /// 基础卡
    /// </summary>
    Base,
	/// <summary>
    /// 事件卡
    /// </summary>
	Event,
	/// <summary>
    /// 事件卡（可丢弃）
    /// </summary>
	EventDroppable,
	/// <summary>
    /// 强化卡
    /// </summary>
	Intensify
}
/// <summary>
/// 卡牌类
/// </summary>
[GlobalClass]
public partial class CardData : Resource
{	
	[ExportCategory("卡牌属性")]
	[Export]public CardType Type{get;set;}
	[Export]public Callable OnUse{get;set;}
	[Export]public Texture2D sprite{get;set;}

	[Signal]
	public delegate void CardUseEventHandler(CardData card);
}
