using Godot;
using Godot.Collections;

public partial class Terrain : Node2D
{
	[ExportCategory("地块属性")]
	[Export]public float height = 20;
	[Export]public float width = 20; 
	[Export]public Map map;
	[Export]public int mapPosH;
	[Export]public int mapPosW;
	[Export]public TerrainData data;
	[ExportCategory("组件")]
	[Export]public Sprite2D sprite;
	[ExportCategory("相邻区块（上下左右）")]
	[Export]public Array<Terrain> neighbors = new();

	public int Y {get{return mapPosH;}}
	public int X {get{return mapPosW;}}

	public Terrain Up => neighbors[0];
	public Terrain Down => neighbors[1];
	public Terrain Left => neighbors[2];
	public Terrain Right => neighbors[3];

	public TerrainType Type => data.type;

    public override void _Ready()
    {
		for(Node prt = GetParent(); prt != null; prt = prt.GetParent())
        {
            if(prt is Map)
            {
                map = prt as Map;
				break;
            }
        }
        map.map[Y,X] = this;
    }

	// 根据地块数据初始化地块信息
	public void Init()
    {	
		GD.Print($"{X}-{Y}地形初始化");
		if(data == null)
        {
            GD.Print("地形数据为空");
			data = AssetManager.GetDefaultData("terrain") as TerrainData;
        }
		//设置贴图
		if(data.texture != null)
        	sprite.Texture = data.texture;
		else
		{
			sprite.Texture = AssetManager.GetDefaultSprite("terrain");
		}
		Vector2 textureSize = sprite.Texture.GetSize();
		sprite.Scale = new Vector2(width / textureSize.X, height / textureSize.Y);
		GD.Print($"{X}-{Y}贴图缩放:{sprite.Scale}");
    }

}
