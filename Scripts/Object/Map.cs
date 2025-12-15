using Godot;
using System;

public partial class Map : Node
{
	private static Map instance;
	public static Map Instance => instance;

	
	public Terrain[,] map;
    public override void _Ready()
    {
		instance = this;
        map = new Terrain[7,8];
    }

}
