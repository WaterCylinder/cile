using Godot;
using System;

/// <summary>
/// 单位战斗行为，在战斗系统中调用，方法名后缀：FuncRollback表示为方法Func的回滚方法。
/// </summary>
[GlobalClass]
public partial class UnitBattleBehavior : UnitBehavior
{
    [Export]public string displayName = "战斗";
    public Unit other;

    BattleSystem battleSystem => GameManager.Instance.game.battleSystem;

    public void ExitBattleSystem()
    {
        battleSystem.Exit();
    }

    public void BattleLog()
    {
        GD.Print($"{unit.unitName} 正在与 {other.unitName} 展开战斗");
    }

    # region "单位战斗行为"

    public void Fight()
    {
        //TODO,这里只实现一下显示小文本的功能
        float demage = (int)Arg("demage");
        other.Hurt(unit, demage);
    }

    # endregion
}
