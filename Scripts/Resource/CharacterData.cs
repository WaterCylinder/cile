using Godot;
using Godot.Collections;
using System;

/// <summary>
/// 角色数据
/// </summary>
public enum CharacterTag
{
    None,
}
public partial class CharacterData : Resource
{
    [ExportCategory("角色基础属性")]
    /// <summary>
    /// 角色标签
    /// </summary>
    [Export]public Array<CharacterTag> tags;
    /// <summary>
    /// 初始行动点数
    /// </summary>
    [Export]public int activatePoint;
    [Export]public int maxHp;
    [Export]public string unitBigName = "";
    [Export]public string unitSmallName = "";
}
