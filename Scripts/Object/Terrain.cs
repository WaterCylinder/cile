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
	[Export]public Array<Effect> effects = new();
	[Export]public Array<bool> effectDisable = new();
	[ExportCategory("组件")]
	[Export]public Sprite2D sprite;
	[Export]public Area2D area;
	[Export]public CollisionShape2D collision;
	[ExportCategory("相邻区块（上下左右）")]
	[Export]public Array<Terrain> neighbors = new();
	[ExportCategory("挂载实体")]
	[Export]public Unit unit;

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

		//设置初始效果组(浅克隆)
		if(data.effects != null)
		{
			effects = data.effects.Duplicate(false);
			//设置禁用状态(默认全部启用)
			effectDisable = new Array<bool>();
			effectDisable.Resize(effects.Count);
		}
			
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
	# region 地形效果相关方法
	/// <summary>
	/// 获取地形效果是否启用
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
		return !effectDisable[index];
	}
	/// <summary>
	/// 设置地形效果启用
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
		effectDisable[index] = !isEnable;
	}

	/// <summary>
	/// 给地形添加效果
	/// </summary>
	/// <param name="effect"></param>
	public void AddEffect(Effect effect)
	{
		if(effects == null)
        {
			effects = new Array<Effect>();
        }
		effects.Add(effect);
		effectDisable.Add(false);
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
