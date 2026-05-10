using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class BattleOperation
{
	public string name;
	public string info;
	public Unit unit;
	public Unit other;
	public Action<Unit, Unit> action;
	public Action<Unit, Unit> rollbackAction;
	public void Run()
	{
		if(unit == null || other == null)
		{
			GD.PushWarning("战斗操作：战斗单位缺失");
		}
		action?.Invoke(unit, other);
	}

	public void Rollback()
	{
		if(unit == null || other == null)
		{
			GD.PushWarning("战斗回滚：战斗单位缺失");
		}
		rollbackAction?.Invoke(unit, other);
	}
}

public class BattleSystem
{
	public bool isBattle = false;
	public Unit unit;
	public Unit other;
	/// <summary>
	/// 战斗系统操作栈，用于快速操作，回退。
	/// </summary>
	private Stack<BattleOperation> operationStack = new();
	/// <summary>
	/// 操作历史栈。
	/// </summary>
	private Stack<BattleOperation> operationHistory = new();
	BattleUI battleUI;
	Game game => GameManager.Instance.game;
	public void Init()
	{
		battleUI = game.pui.battleUI;
		battleUI.Visible = false;
	}

	public void Start(Unit unit, Unit other)
	{
		GD.Print("启用战斗系统");
		//设置当前战斗单位
		this.unit = unit;
		this.other = other;
		//清空操作栈
		operationStack.Clear();
		operationHistory.Clear();
		//开始显示UI并重置UI位置
		battleUI.Visible = true;

		//设置单位战斗事件
		battleUI.behaviors = unit.battleBehaviors.Duplicate(false);
		battleUI.Start();

		isBattle = true;
	}

	/// <summary>
	/// 添加操作
	/// </summary>
	/// <param name="unit">战斗发起单位</param>
	/// <param name="other">其他单位</param>
	/// <param name="action">战斗行为</param>
	/// <param name="rollback">战斗回滚</param>
	/// <returns></returns>
	public bool AddOperation(Unit unit, Unit other, Action<Unit, Unit> action, Action<Unit, Unit> rollback)
	{
        BattleOperation operation = new BattleOperation
        {
            unit = unit,
            other = other,
			action = action,
			rollbackAction = rollback
        };
		if(operationStack.Count > 64)
		{
			GD.PushWarning("当前战斗系统有太多操作未处理，添加操作失败！");
			return false;
		}
        operationStack.Push(operation);
		return true;
	}

	public bool AddOperation(Action<Unit, Unit> action, Action<Unit, Unit> rollback)
	{
		return AddOperation(unit, other, action, rollback);
	}

	/// <summary>
	/// 执行下一个行为
	/// </summary>
	public void RunNext()
	{
		if(operationStack.Count == 0)
		{
			return;
		}
		BattleOperation operation = operationStack.Pop();
		operationHistory.Push(operation);
		if(operationHistory.Count > 128)
		{
			//操作历史超过128条时切割掉最早的64条
			var temp = operationHistory.ToArray().Take(64).ToArray();
			operationHistory.Clear();
			for(int i = temp.Length - 1; i >= 0; i--)
			{
				operationHistory.Push(temp[i]);
			}
		}
		operation.Run();
	}

	/// <summary>
	/// 回滚上一个执行的行为
	/// </summary>
	public void Rollback()
	{
		if(operationHistory.Count == 0)
		{
			return;
		}
		BattleOperation operation = operationHistory.Pop();
		operation.Rollback();
		operationStack.Push(operation);
	}

	public void Exit()
	{
		GD.Print("退出战斗系统");
		battleUI.Visible = false;
	}
}
