using Godot;
using System;

/// <summary>
/// 卡片类型
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
	[Export]public string cardName;
	[Export]public string cardInfo;
	[Export]public CardType type;
	[Export]public Effect onUse;
	[Export]public Texture2D sprite;
}