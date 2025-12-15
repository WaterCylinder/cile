using Godot;
using System;
using System.ComponentModel;

public partial class Terrain : Node
{
	[ExportCategory("地块属性")]
	[Export]public Map map;
	[Export]public Vector2 mapPos;
	[Export]public TerrainData data;
	[ExportCategory("组件")]
	[Export]public Sprite2D sprite;

	public int X{get{return (int)mapPos.X;}}
	public int Y{get{return (int)mapPos.Y;}}

	public TerrainType Type => data.type;

    public override void _Ready()
    {
        
    }

}
