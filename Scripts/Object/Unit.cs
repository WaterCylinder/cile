using System;
using Godot;
using Godot.Collections;

/*
玩家可操控单位
*/

public partial class Unit : Node2D
{
	[Export] public AnimatedSprite2D animator;
	[Export] public UnitData data;
	[ExportCategory("所属玩家")]
	[Export] public Player player;
	[ExportCategory("所在地形")]
	[Export] public Terrain terrain;
	[ExportCategory("自身属性")]
	[Export] public string unitName;
	[Export] public float maxHealth;
	[Export] public float health;
	[Export] public Array<Effect> effects;
	[Export] public Array<bool> effectEnable;
	[Export] public Array<UnitBattleBehavior> battleBehaviors;
	[Export] public Array<Vector2> moveRange;
	[Export] public Array<Vector2> battleRange;
	[Export] public int movePoint; //移动点数
	[Export] public float scale = 1;

	[Signal]public delegate void OnEffectInvokeEventHandler(Unit unit, Effect effect);
	/// <summary>
	/// 效果无效状态检查结束前执行的方法，用于在效果无效状态检查前执行一些操作以修改检查结果
	/// </summary>
	public BoolEventSheet BeforeEffctEnableCheck = new();
	

	public override void _Ready()
	{
		Game game = GameManager.Instance.game;
		game.roundCricle.OnTurnStart += () =>
		{
			// 单位在回合开始的通用初始化
			movePoint = 1;
		};
	}
	
	public void Init()
	{
		if(data == null)
		{
			GD.PushWarning("单位数据为空");
			data = AssetManager.GetDefaultData("unit") as UnitData;
		}
		//根据单位数据初始化单位对象
		if(data.frames != null)
		{
			animator.SpriteFrames = data.frames;
		}
		else
		{
			animator.SpriteFrames = AssetManager.GetDefaultData("unitAnimation") as SpriteFrames;
		}
		
		maxHealth = data.maxHealth;
		health = data.maxHealth;

		AnimationPlay("idle");
		unitName = data.unitName;
		effects = data.effects.Duplicate(false);
		effectEnable = new();
		for(int i = 0; i <= effects.Count - 1; i++)
		{
			effectEnable.Add(true);
		}
		battleBehaviors = data.behaviors.Duplicate(false);
		moveRange = data.moveRange.Duplicate(false);
		movePoint = 1; //初始化移动点数
		battleRange = data.battleRange.Duplicate(false);
		SetScale();
	}

	/// <summary>
	/// 移动到指定地形
	/// </summary>
	/// <param name="terrain"></param>
	public void MoveTo(Terrain target, bool userPoint = true)
	{
		if(target.unit != null)
		{
			GD.PushWarning($"目标地形{target.Name}已有单位");
			return;
		}
		if(terrain != null)
			terrain.unit = null;
		terrain = target;
		terrain.unit = this;
		GD.Print($"移动实体{Name}到{terrain.Name}");
		//移动逻辑，只简单做一个移动位置
		ResetPosition();
		//单位进入地形信号发出
		Game game = GameManager.Instance.game;
		game.EmitSignal(nameof(game.OnUnitEnterTerrain), this, target);
		if(userPoint)
			movePoint -= 1;
	}

	/// <summary>
	/// 重置位置
	/// </summary>
	public void ResetPosition()
	{
		Position = terrain.GlobalPosition;
	}

	/// <summary>
	/// 与另一单位展开战斗
	/// </summary>
	/// <param name="other"></param>
	public void Battle(Unit other)
	{
		GD.Print($"单位{unitName} 与 单位{other.unitName} 展开战斗");
		GameManager.Instance.game.battleSystem.Start(this, other);
	}

	# region 动画与特效

	public void AnimationPlay(string name = null)
	{
		if(name == null)
		{
			animator.Play();
		}
		if (! animator.SpriteFrames.HasAnimation(name))
		{
			name = "default";
		}
		GD.Print($"{unitName} 播放动画 {name}");
		animator.Play(name);
	}

	public void AnimationPause()
	{
		animator.Pause();
	}

	public void SetScale(float scale = -1)
	{
		if(scale > 0)
			this.scale = scale;
		animator.Scale = new Vector2(this.scale, this.scale);
	}

	# endregion

	# region 效果相关方法
	public void EffectInit(Effect effect)
	{
		if(effect == null)
			return;
		if(effect.behavior is not UnitBehavior)
			return;
		UnitBehavior behavior = effect.behavior as UnitBehavior;
		if(behavior.unit == this)
			return;
		behavior.unit = this;
		//为条件里可能存在的单位行为设置单位对象
		Condition condition = effect.condition as SingalCondition;
		condition.OnBehaviorInit = (b) =>{
			if(b is UnitBehavior)
			{
				(b as UnitBehavior).unit = this;
			}
		};
	}
	public void EffectInvoke(int index, Action onInvoke = null)
	{
		if(effects == null)
			return;
		GD.Print($"调用效果ID：{index}， {effects}");
		Effect effect = effects[index];
		if(effect != null)
		{
			EffectInit(effect);
			effect.Run();
			//本体的事件信号
			EmitSignal(nameof(OnEffectInvoke), this, effect);
			//全局单位事件信号
			try{ GameManager.Instance.game.EmitSignal(nameof(GameManager.Instance.game.OnUnitEffect), this, effect); } catch {}
			onInvoke?.Invoke();
		}
	}
	public Effect GetEffect(int index)
	{
		if(effects == null)
		{
			return null;
		}
		if(index >= effects.Count || index < 0)
        {
			return null;
        }
		return effects[index];
	}
	/// <summary>
	/// 根据效果的行为名称获取效果，可以输入部分字符串，自带包含检测
	/// </summary>
	/// <param name="behaviornName"></param>
	/// <returns></returns>
	public int GetEffectIndex(string behaviornName)
	{
		if(effects == null)
		{
			return -1;
		}
		for(int i = 0; i <= effects.Count - 1; i++)
		{
			Effect e = effects[i];
			if(e.behavior.behaviorName.Contains(behaviornName))
			{
				return i;
			}
		}
		return -1;
	}
	/// <summary>
	/// 检查某个效果是否在单位效果列表里
	/// </summary>
	/// <param name="effect"></param>
	/// <returns></returns>
	public bool checkEffect(Effect effect)
	{
		return effects.Contains(effect);
	}
	/// <summary>
	/// 获取单位效果是否启用
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public bool GetEffectIsEnable(int index)
	{	
		if(effects == null)
		{
			return false;
		}
		if(index >= effects.Count || index < 0)
        {
			return false;
        }
		bool result = effectEnable[index];
		//执行操作改变检查结果
		if(BeforeEffctEnableCheck != null)
		{
			result = BeforeEffctEnableCheck.Invoke(result);
		}
		return result;
	}

	/// <summary>
	/// 设置单位效果启用
	/// </summary>
	/// <param name="index"></param>
	/// <param name="isEnable"></param>
	public void SetEffectEnable(int index, bool isEnable = true)
	{
		if(effects == null)
		{
			return;
		}
		if(index >= effects.Count || index < 0)
        {
			return;
        }
		effectEnable[index] = isEnable;
	}

	/// <summary>
	/// 给单位添加效果
	/// </summary>
	/// <param name="effect"></param>
	public void AddEffect(Effect effect)
	{
		if(effects == null)
        {
			effects = new Array<Effect>();
        }
		effects.Add(effect);
		effectEnable.Add(false);
	}
	/// <summary>
	/// 移除指定索引的效果
	/// </summary>
	/// <param name="index"></param>
	public void RemoveEffect(int index)
	{
		if(effects == null)
		{
			return;
		}
		if(index >= effects.Count || index < 0)
        {
			return;
        }
		effects.RemoveAt(index);
	}
	/// <summary>
	/// 移除指定效果
	/// </summary>
	/// <param name="effect"></param>
	public void RemoveEffect(Effect effect)
    {
		if(effects == null)
        {
			return;
        }
		effects.Remove(effect);
    }

	# endregion
	
}
