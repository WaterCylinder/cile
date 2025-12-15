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
        CardData card = AssetManager.Instance.GetData("Cards.card_test") as CardData;
        GD.Print(card.type);
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
