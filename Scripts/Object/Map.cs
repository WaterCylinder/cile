using Godot;
using System;

public partial class Map : Node2D
{
	private static Map instance;
	public static Map Instance => instance;

    [ExportCategory("地图属性")]
    [Export] public int mapWidth;
    [Export] public int mapHeight;

	private Terrain[,] _map;
    public Terrain[,] map
    {
        get
        {
            if(_map == null)
            {
                _map = new Terrain[mapHeight, mapWidth];
            }
            return _map;
        }
    }
    public override void _Ready()
    {
		instance = this;
    }

    // 生成地图框架
    public void Generate()
    {   
        GD.Print("地图框架生成");
        PackedScene packedScene = AssetManager.GetDefaultObject("terrain");
        for(int i = 0; i < mapHeight; i++)
        {
            for(int j = 0; j < mapWidth; j++)
            {
                Terrain t = packedScene.Instantiate<Terrain>();
                t.mapPosH = i;
                t.mapPosW = j;
                t.Position = new Vector2(j * t.width, -i * t.height);
                map[i,j] = t;
                AddChild(t);
            }
        }
    }

    // 初始化地图区块
    public void Init()
    {
        
        GD.Print("地图区块初始化");
        for(int i = 0; i < mapHeight; i++)
        {
            for(int j = 0; j < mapWidth; j++)
            {
                Terrain t = map[i,j];
                GD.Print($"初始化地图区块: {t}");
                t.Init();
                map[i,j] = t;
            }
        }
    }

}
