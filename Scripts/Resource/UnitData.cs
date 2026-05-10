using Godot;
using Godot.Collections;
using System;

public enum UnitTag
{
    Normal,
}

/// <summary>
/// 单位数据
/// </summary>
[GlobalClass]
public partial class UnitData : Resource
{
    [Export]public Array<UnitTag> tags = new Array<UnitTag>();
    [Export]public string unitName = "单位";
    [Export]public SpriteFrames frames;
    [Export]public float maxHealth;
    [Export]public Array<Vector2> moveRange = new(); 
    [Export]public Array<Vector2> battleRange = new();
    [Export]public Array<Effect> effects = new();
    [Export]public Array<UnitBattleBehavior> behaviors = new();
    
}
