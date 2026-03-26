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
    [Export]public Texture2D spriteSheet;
    
}
