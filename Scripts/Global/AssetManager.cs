using Godot;
using System;
using Godot.Collections;
using Utils;

public partial class AssetManager : Node
{	
	private static AssetManager instance;
	public static AssetManager Instance
    {
        get
        {
            if(instance == null)
            {
                try
				{
					GD.Print("正在初始化资源管理器");
					SceneTree sceneTree = Engine.GetMainLoop() as SceneTree;
					instance = new AssetManager();
					instance.Name = "AssetManager";
					instance.ProcessMode = ProcessModeEnum.Always;
					sceneTree.Root.AddChild(instance);
				}
				catch(Exception e)
				{
					GD.PrintErr($"初始化资源管理器失败！{e}");
				}
            }
			return instance;
        }
    }
	/// <summary>
    /// 物体（tsen）资源地址
    /// </summary>
    public string objectResourcePath = Global.ResourcePath.PathJoin("Objects");
    /// <summary>
    /// 精灵资源地址
    /// </summary>
	public string spriteResourcePath = Global.ResourcePath.PathJoin("Sprites");
    /// <summary>
    /// 数据资源地址
    /// </summary>
	public string dataResourcePath = Global.ResourcePath.PathJoin("Datas");
	
	[Export] private Dictionary<string, PackedScene> _objectDict = new();
	[Export] public Dictionary<string, Texture2D> _spriteDict = new();
	[Export] public Dictionary<string, Resource> _dataDict = new();

	/// <summary>
    /// 缓存并获取物体资源（tscn）,从Resources/Objects目录下加载
    /// </summary>
    /// <param name="path">资源地址，使用Folder1.Folder2.resname的格式</param>
    /// <returns></returns>
	public PackedScene GetObject(string path)
    {
		path = Tools.Path(path,objectResourcePath);
		path += ".tscn";
        if (_objectDict.ContainsKey(path))
        {
            return _objectDict[path];
        }
        try
        {
            PackedScene obj = ResourceLoader.Load<PackedScene>(path);
			_objectDict.Add(path, obj);
            GD.Print($"加载物体成功：{path}");
			return obj;
        }
        catch(Exception e)
        {
            GD.PrintErr($"AssetManager加载物体资源出错：{e}");
			return null;
        }
    }

    /// <summary>
    /// 默认使用的物体资源
    /// </summary>
    public static PackedScene GetDefaultObject(string name)
    {
        switch (name)
        {
            case "terrain":
                return Instance.GetObject("terrain");
            default:
                return null;
        }
    }

	/// <summary>
    /// 缓存并获取精灵资源,从Resources/Sprites目录下加载
    /// </summary>
    /// <param name="path">资源地址，使用Folder1.Folder2.resname的格式(图片路径需要自己添加后缀名)</param>
    /// <returns></returns>
	public Texture2D GetSprite(string path, string exten = ".png")
    {
		path = Tools.Path(path,spriteResourcePath);
        path += exten;
        if (_spriteDict.ContainsKey(path))
        {
            return _spriteDict[path];
        }
        try
        {
            Texture2D sprite = ResourceLoader.Load<Texture2D>(path);
			_spriteDict.Add(path, sprite);
            GD.Print($"加载图片成功：{path}");
			return sprite;
        }
        catch(Exception e)
        {
            GD.PrintErr($"AssetManager加载精灵资源出错：{e}");
			return null;
        }
    }

    /// <summary>
    /// 默认使用的精灵图片
    /// </summary>
    public static Texture2D GetDefaultSprite(string name)
    {
        switch (name)
        {
            case "terrain":
                return Instance.GetSprite("Default.block_default");
            default:
                return null;
        }
    }

	/// <summary>
    /// 缓存并获取数据资源（tres），从Resources/Datas目录下加载
    /// </summary>
    /// <param name="path">资源地址，使用Folder1.Folder2.resname的格式</param>
    /// <returns></returns>
	public Resource GetData(string path)
    {	
		path = Tools.Path(path,dataResourcePath);
		path += ".tres";
        if (_dataDict.ContainsKey(path))
        {
            return _dataDict[path];
        }
        try
        {
            Resource data = ResourceLoader.Load<Resource>(path);
			_dataDict.Add(path, data);
            GD.Print($"加载数据成功：{path}");
			return data;
        }
        catch(Exception e)
        {
            GD.PrintErr($"AssetManager加载DATA资源出错：{e}");
			return null;
        }
    }

    /// <summary>
    /// 默认使用的数据
    /// </summary>
    public static Resource GetDefaultData(string name)
    {
        switch (name)
        {
            case "terrain":
                return Instance.GetData("Default.terrain_data_default");
            default:
                return null;
        }
    }
}
