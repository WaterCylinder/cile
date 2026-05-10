using Godot;
using System;

[GlobalClass]
public partial class AnimationAction : Resource
{
	[Export] public string actionName;
	[Export] public int startIndex;
	[Export] public int endIndex;
	[Export] public float speed = 1;
}