using Godot;
using Godot.Collections;

public partial class Terrain : Node2D
{
	[ExportCategory("地块属性")]
	[Export]public float height = 32;
	[Export]public float width = 32; 
	[Export]public Map map;
	[Export]public int mapPosH;
	[Export]public int mapPosW;
	[Export]public TerrainData data;
	[ExportCategory("组件")]
	[Export]public Sprite2D sprite;
	[Export]public Area2D area;
	[Export]public CollisionShape2D collision;
	[ExportCategory("相邻区块（上下左右）")]
	[Export]public Array<Terrain> neighbors = new();

	public int Y {get{return mapPosH;}}
	public int X {get{return mapPosW;}}

	public Terrain Up => neighbors[0];
	public Terrain Down => neighbors[1];
	public Terrain Left => neighbors[2];
	public Terrain Right => neighbors[3];

	public TerrainType Type => data.type;

	public bool isMouseEnter = false;
	public bool isPressed = false;

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
		area.MouseEntered += () =>
        {
			isMouseEnter = true;
        };
		area.MouseExited += () =>
        {
			isMouseEnter = false;
			isPressed = false;
        };
    }

	// 根据地块数据初始化地块信息
	public void Init()
    {	
		// GD.Print($"{X}-{Y}地形初始化");
		if(data == null)
        {
            GD.Print($"{mapPosH}行 - {mapPosW}列 地形数据为空");
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
		// GD.Print($"{X}-{Y}贴图缩放:{sprite.Scale}");

		//设置碰撞箱大小
		collision.Shape = new RectangleShape2D()
        {
            Size = new Vector2(width, height)
        };
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
		if(@event is InputEventMouseButton mouseEvent) 
        {
            if(isMouseEnter && mouseEvent.ButtonIndex == MouseButton.Left)
            {
                if(mouseEvent.Pressed)
                {
					isPressed = true;
                }
				if(!mouseEvent.Pressed)
                {
					if(isPressed)
                    {
						isPressed = false;
						if(!GameManager.Instance.game.mainCamera.isDrug)
                        {
							Selected();
                        }
                    }
                }
            }
        }
    }


	/// <summary>
    /// 更改地形的数据，如果需要则重新初始化
    /// </summary>
    /// <param name="newData"></param>
    /// <param name="isReload"></param>
	public void ChangeData(TerrainData newData, bool isReload = false)
    {
        data = newData;
		if (isReload)
        {
			Init();
        }
    }

	/// <summary>
    /// 被选择
    /// </summary>
	public void Selected()
    {
		GD.Print($"{X}-{Y}被选择");
        GameManager.Instance.game.mainCamera.SetTargetPosition(area.GlobalPosition);
		GameManager.Instance.game.mainCamera.SetTargetZoom(2);
		//区块选择，暂时只支持单选，所以设置前清除当前选中集合
		GameManager.Instance.game.selectedTerrains.Clear();
		GameManager.Instance.game.selectedTerrains.Add(this);
		GameManager.Instance.game.EmitSignal("OnTerrainSelected", this);
    }

}
