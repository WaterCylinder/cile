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

    public void PutTestUnit()
    {
        UnitData data = AssetManager.GetDefaultData("unit") as UnitData;
        GameManager.Instance.game.unitSystem.PutUnit(data, terrain);
    }

    public void SelectReadyBig()
    {
        GD.Print("准备地形选择大单位");
        GameManager.Instance.game.unitSystem.SelectReadyTerrain(terrain, true);
    }
    public void SelectReadySmall()
    {
        GD.Print("准备地形选择小单位");
        GameManager.Instance.game.unitSystem.SelectReadyTerrain(terrain, false);
    }

    #endregion

    # region 地形判断
    public bool AlreadyHasUnit()
    {
        return terrain.unit != null;
    }
    # endregion
}
