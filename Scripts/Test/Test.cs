using Godot;
using Godot.Collections;
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
        Global.LoadConfig();
        Variant colorSheet = Global.config.As<Dictionary>();
        GD.Print(colorSheet);
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
