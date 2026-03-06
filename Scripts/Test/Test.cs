using Godot;
using System;
using Utils.Coroutine;

public partial class Test : Node
{	
	private bool _init = false;
	public override void _Ready()
    {
    }

	public void _Init()
    {
        TerrainData tdata = AssetManager.Instance.GetData("Terrains.terrain_blocked") as TerrainData;
        GD.Print(tdata.texture);
    }

	public override void _Process(double delta)
    {
        if (!_init)
        {
            _init = true;
			_Init();
        }
    }
}
