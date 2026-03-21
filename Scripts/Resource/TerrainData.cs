using Godot;
using Godot.Collections;
using System;

public enum TerrainType
{
    None,
}
[GlobalClass]
public partial class TerrainData : Resource
{
	[ExportCategory("地形属性")]
	[Export]public string terrainName = "地形";
	[Export]public TerrainType type;
	[Export]public int sourceLevel;
	[Export]public int sourceLimit;
	[Export]public Texture2D texture;
	[Export]public Array<Effect> effects;
}
