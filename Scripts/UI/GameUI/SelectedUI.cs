using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class SelectedUI : Control
{
	[Export]public PackedScene buttonPrefab;
	[Export]public Terrain selectedTerrain;
	[Export]public Label terrainNameLabel;
	[Export]public Array<ActiveButton> buttons = [];
	[Export]public Array<Effect> effects = [];
	[Export]public int buttonHeight = 40;
	[Export]public Effect terrainSelectSwitchToTerrainEffect;
	[Export]public Effect terrainSelectSwitchToUnitEffect;

	[ExportCategory("运行时变量")]
	[Export]public bool isUnitSelected = false;

	public override void _Ready()
	{	
		Game game = GameManager.Instance.game;
		game.OnTerrainSelected += OnSelected;
		game.OnTerrainSelectedCancle += Cancel;

		EventBehavior behavior = terrainSelectSwitchToTerrainEffect.behavior as EventBehavior;
		behavior.action += ChangeToTerrainSelect;
		behavior.action += GenerateButtons;

		behavior = terrainSelectSwitchToUnitEffect.behavior as EventBehavior;
		behavior.action += ChangeToUnitSelect;
		behavior.action += GenerateButtons;

		game.roundCricle.OnTurnStart += Cancel;
	}

	public void Cancel()
	{
		Visible = false;
	}

	void ChangeToTerrainSelect()
	{
		if(selectedTerrain != null)
		{
			terrainNameLabel.Text = selectedTerrain.data.terrainName;
			effects = selectedTerrain.effects.Duplicate(false);
			if(selectedTerrain.unit != null)
				effects.Add(terrainSelectSwitchToUnitEffect);
			isUnitSelected = false;
		}
	}

	void ChangeToUnitSelect()
	{
		if(selectedTerrain != null)
		{
			if(selectedTerrain.unit != null)
			{
				terrainNameLabel.Text = selectedTerrain.unit.data.unitName;
				effects = selectedTerrain.unit.effects.Duplicate(false);
				effects.Add(terrainSelectSwitchToTerrainEffect);
				isUnitSelected = true;
			}
		}
	}
	
	public void OnSelected(Terrain terrain)
	{
		if (!GameManager.Instance.game.IsPlayerNow)
		{
			//非当前玩家进行选择操作不弹出UI
			GD.Print("非当前玩家操作");
			Cancel();
			return;
		}
		isUnitSelected = false;
		selectedTerrain = terrain;
		Visible = true;
		Position = terrain.GlobalPosition - PivotOffset;
		if(terrain.unit == null)
			ChangeToTerrainSelect();
		else
			ChangeToUnitSelect();

		GenerateButtons();
	}

	public void GenerateButtons()
	{
		if (isUnitSelected)
		{
			ChangeToUnitSelect();
		}	
		else
		{
			ChangeToTerrainSelect();
		}
		buttons.Clear();
		foreach(Node button in GetChildren())
		{
			if(button is TextureButton)
			{
				button.QueueFree();
			}
		}

		for(int i = 0; i<=effects.Count-1; i++)
		{
			Effect effect = effects[i];
			if(effect != null)
			{
				selectedTerrain.EffectInit(effect);

				int index = effects.Count-1 - i;

				float posY = terrainNameLabel.Size.Y + buttonHeight * index;
				Vector2 pos = new Vector2(0, posY);
				ActiveButton button = buttonPrefab.Instantiate() as ActiveButton;

				index = i;

				button.SetText(effect.effectName);                              //设置按钮文本
				if (selectedTerrain.checkEffect(effect))
				{
					selectedTerrain.EffectInit(effect);
					button.Pressed += () =>	selectedTerrain.EffectInvoke(index);//设置按钮点击事件
				}else if (selectedTerrain.unit.checkEffect(effect))
				{
					selectedTerrain.unit.EffectInit(effect);
					button.Pressed += () =>	selectedTerrain.unit.EffectInvoke(index);//设置按钮点击事件
				}
				else
					button.Pressed += effect.Run;
				
				button.Pressed += ResetButtonState; 						//点击之后重置状态
				button.Pressed += GenerateButtons;						//点击之后重新生成按钮
				button.Position = pos;											//设置按钮位置
				
				buttons.Add(button);

				AddChild(button);
			}
		}
		
		ResetButtonState();
		Size = new Vector2(Size.X, buttonHeight * effects.Count + terrainNameLabel.Size.Y);
	}

	public void ResetButtonState()
	{
		if (isUnitSelected)
		{
			for(int i = 0; i<=effects.Count-1; i++)
			{
				if(!effects[i].condition.Run())
				{
					buttons[i].Switch(false);
				}
				else
				{
					buttons[i].Switch(true);
				}
				if(selectedTerrain.unit.checkEffect(effects[i]))
				{
					if (!selectedTerrain.unit.GetEffectIsEnable(i))
					{
						buttons[i].Switch(false);
					}
				}
			}
		}
		else
		{
			for(int i = 0; i<=effects.Count-1; i++)
			{
				if(!effects[i].condition.Run())
				{
					buttons[i].Switch(false);
				}
				else
				{
					buttons[i].Switch(true);
				}
				if(selectedTerrain.checkEffect(effects[i]))
				{
					if (!selectedTerrain.GetEffectIsEnable(i))
					{
						buttons[i].Switch(false);
					}
				}
			}
		}
		
	}
}
