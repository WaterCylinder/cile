using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public enum PlayerState
{
    None,
}
public partial class Player : Node
{	
	[Export]public string Id{get;set;}
	[Export]public PlayerState State{get;set;}
	[Export]public int ResourcesPoint{get;set;}
	[Export]public int HealthPoint{get;set;}
	[Export]public int Death{get;set;}
	[Export]public Array<CardData> HandCards{get;set;}
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
