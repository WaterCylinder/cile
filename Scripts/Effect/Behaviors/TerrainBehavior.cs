using Godot;
using System;

/// <summary>
/// 地形行为
/// </summary>
[GlobalClass]
public partial class TerrainBehavior : Behavior
{
    public Terrain terrain;
    
    # region 地形行为

    public void TerrainLog()
    {
        GD.Print(terrain.Name);
    }

    #endregion
}
