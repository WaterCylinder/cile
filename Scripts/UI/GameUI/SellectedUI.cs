using Godot;
using Godot.Collections;
using System;

public partial class SellectedUI : Control
{
	[Export]public PackedScene buttonPrefab;
	[Export]public Terrain selectedTerrain;
	[Export]public Label terrainNameLabel;
	[Export]public Array<Button> terrainButtons; 
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
		if(selectedTerrain.data.effects != null)
		for(int i = 0; i<=selectedTerrain.data.effects.Count-1; i++)
		{
			Effect effect = selectedTerrain.data.effects[i];
			if(effect != null)
			{
				TerrainBehavior behavior = effect.behavior as TerrainBehavior;
				behavior.terrain = selectedTerrain;
				float posY = terrainNameLabel.Size.Y + buttonHeight * i;
				Vector2 pos = new Vector2(0, posY);
				ActiveButton button = buttonPrefab.Instantiate() as ActiveButton;
				button.SetText(effect.effectName);
				button.Pressed += effect.Run;
				AddChild(button);
			}
		}
	}
}
