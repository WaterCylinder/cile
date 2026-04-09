using System.Linq;
using Godot;
using Godot.Collections;

public partial class Map : Node2D
{
	private static Map instance;
	public static Map Instance => instance;

    [ExportCategory("地图属性")]
    [Export] public int mapWidth;
    [Export] public int mapHeight;

    //特殊地形索引
    /// <summary>
    /// 准备地形
    /// </summary>
    public Dictionary<Vector2, Array<Terrain>> readyTerrains;
    public Array<Terrain> innerTerrains;

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
        //左下角为0，0
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
                t.Name = $"Terrain_{i}_{j}";
                AddChild(t);
            }
        }
        GD.Print("重置地图位置");
        Position = new Vector2((mapWidth - 1) * -0.5f * map[0,0].width, (mapHeight - 1) * 0.5f * map[0,0].height);
    }

    /// <summary>
    /// 设置准备区域相关特殊区域
    /// </summary>
    public void ReadyTerrainSet()
    {   
        GD.Print("特殊地图区块设置");
        // 设置屏蔽区域(四个角)
        TerrainData terrain_data_blocked = AssetManager.Instance.GetData("Terrains.terrain_data_blocked") as TerrainData;
        map[0,0].ChangeData(terrain_data_blocked);
        map[0,mapWidth-1].ChangeData(terrain_data_blocked);
        map[mapHeight-1,0].ChangeData(terrain_data_blocked);
        map[mapHeight-1,mapWidth-1].ChangeData(terrain_data_blocked);
        // 设置准备区域(四个边)
        readyTerrains = new Dictionary<Vector2, Array<Terrain>>()
        {
            {Vector2.Up, new Array<Terrain>()},
            {Vector2.Down, new Array<Terrain>()},
            {Vector2.Left, new Array<Terrain>()},
            {Vector2.Right, new Array<Terrain>()}
        };
        TerrainData terrain_data_ready = AssetManager.Instance.GetData("Terrains.terrain_data_ready") as TerrainData;
        for (int i = 1; i< mapHeight-1; i++)
        {
            map[i, 0].ChangeData(terrain_data_ready);
            readyTerrains[Vector2.Left].Add(map[i, 0]);
            map[i, mapWidth-1].ChangeData(terrain_data_ready);
            readyTerrains[Vector2.Right].Add(map[i, mapWidth-1]);
        }
        for (int j = 1; j< mapWidth-1; j++)
        {
            map[0, j].ChangeData(terrain_data_ready);
            readyTerrains[Vector2.Down].Add(map[0, j]);
            map[mapHeight-1, j].ChangeData(terrain_data_ready);
            readyTerrains[Vector2.Up].Add(map[mapHeight-1, j]);
        }
    }

    public void SetInnerTerrain()
    {
        innerTerrains = new Array<Terrain>();
        for(int i = 1; i < mapHeight-1; i++)
        {
            for(int j = 1; j < mapWidth-1; j++)
            {
                innerTerrains.Add(map[i,j]);
            }
        }
    }

    /// <summary>
    /// 初始化所有区域的地形
    /// </summary>
    public void Init()
    {
        GD.Print("地图区块初始化");
        for(int i = 0; i < mapHeight; i++)
        {
            for(int j = 0; j < mapWidth; j++)
            {
                Terrain t = map[i,j];
                // GD.Print($"初始化地图区块: {t}");
                t.Init();
                map[i,j] = t;
                if(i < mapHeight-1)t.neighbors.Add(map[i+1,j]); //上
                if(i > 0)t.neighbors.Add(map[i-1,j]);           //下
                if(j > 0)t.neighbors.Add(map[i,j-1]);           //左
                if(j < mapWidth-1)t.neighbors.Add(map[i,j+1]);  //右
            }
        }
        SetInnerTerrain();
        GD.Print($"初始化了{mapHeight * mapWidth}个区块");
    }

}
