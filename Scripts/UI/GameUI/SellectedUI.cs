using Godot;
using Godot.Collections;
using System;

public partial class SellectedUI : Control
{
	[Export]public PackedScene buttonPrefab;
	[Export]public Terrain selectedTerrain;
	[Export]public Label terrainNameLabel;
	[Export]public Array<ActiveButton> terrainButtons = [];
	[Export]public Array<Effect> effects = [];
	[Export]public int buttonHeight = 40;

	public override void _Ready()
	{	
		Game game = GameManager.Instance.game;
		game.OnTerrainSelected += OnSelected;
	}
	
	public void OnSelected(Terrain terrain)
	{
		selectedTerrain = terrain;
		terrainNameLabel.Text = terrain.data.terrainName;
		//移除所有的按钮对象
		foreach(Node button in GetChildren())
		{
			if(button is TextureButton)
			{
				button.QueueFree();
			}
		}
		if(selectedTerrain.effects != null)
		{
			effects.Clear();
			terrainButtons.Clear();
			for(int i = 0; i<=selectedTerrain.effects.Count-1; i++)
			{
				Effect effect = selectedTerrain.effects[i];
				if(effect != null)
				{
					TerrainBehavior behavior = effect.behavior as TerrainBehavior;
					behavior.terrain = selectedTerrain;
					try
					{	//尝试为条件里可能存在的地形行为设置地形对象
						SingalCondition condition = effect.condition as SingalCondition;
						TerrainBehavior conditionBehavior = condition.behavior as TerrainBehavior;
						conditionBehavior.terrain = selectedTerrain;
					}catch { }
					float posY = terrainNameLabel.Size.Y + buttonHeight * i;
					Vector2 pos = new Vector2(0, posY);
					Size = new Vector2(Size.X, posY + buttonHeight);
					ActiveButton button = buttonPrefab.Instantiate() as ActiveButton;

					button.SetText(effect.effectName);	//设置按钮文本
					button.Pressed += effect.Run;		//设置按钮点击事件
					button.Pressed += ResetButtonState; //点击之后重置状态
					button.Position = pos;				//设置按钮位置
					
					effects.Add(effect);
					terrainButtons.Add(button);

					AddChild(button);
				}
			}
			ResetButtonState();
			Size = new Vector2(Size.X, buttonHeight * selectedTerrain.effects.Count + terrainNameLabel.Size.Y);
		}
	}

	public void ResetButtonState()
	{
		for(int i = 0; i<=effects.Count-1; i++)
		{
			if(!effects[i].condition.Run())
			{
				terrainButtons[i].Switch(false);
			}
			else
			{
				terrainButtons[i].Switch(true);
			}
		}
	}
}
