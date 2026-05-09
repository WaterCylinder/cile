using Godot;
using System;

[GlobalClass]
public partial class UnitBehavior : Behavior
{
	public Unit unit;

	# region 单位事件

	public void UnitLog()
	{
		GD.Print(unit.Name);
	}

	public void UnitMove()
	{
		Game game = GameManager.Instance.game;
		game.marksUI.ShowUnitMoveRange(unit);
		game.unitSystem.StartTerrainSelectMode(
			callback: t =>
			{
				game.marksUI.ShowUnitMoveRange(null);
				GD.Print("单位移动区块选择：" + t.Name);
				if (game.unitSystem.GetUnitMoveRangeTerrains(unit).Contains(t))
				{
					unit.MoveTo(t);
					GD.Print("单位移动区块选择， 移动到：" + t.Name);
					game.pui.selectedUI.GenerateButtons();
				}
			}
		);
		game.pui.selectedUI.Cancel();
	}

	public void UnitBattle()
	{
		Game game = GameManager.Instance.game;
		game.marksUI.ShowUnitBattleRange(unit);
		game.unitSystem.StartTerrainSelectMode(
			callback: t =>
			{
				game.marksUI.ShowUnitBattleRange(null);
				GD.Print("单位战斗区块选择：" + t.Name);
				if (game.unitSystem.GetUnitBattleRangeTerrains(unit).Contains(t))
				{
					GD.Print("单位战斗区块选择：" + t.Name);
					//TODO
				}
			}
		);
		game.pui.selectedUI.Cancel();
	}

	# endregion

	# region 单位条件

	public bool UnitMovePointCheck()
	{
		return unit.movePoint > 0;
	}

	# endregion
}
