using Godot;
using Godot.Collections;

public enum DeckTag
{
	/// <summary>
	/// 资源卡组
	/// </summary>
	Resource,
	/// <summary>
	/// 地形卡组
	/// </summary>
	Terrain
}

[GlobalClass]
public partial class DeckData : Resource
{
	[ExportCategory("卡组属性")]
	[Export] public string deckName = "";
	[Export] public string deckInfo = "";
	[Export] public Array<DeckTag> tags = new();
	[Export] public Dictionary<string, uint> cards = new();
}
