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
	public string objectResourcePath = Global.ResourcePath.PathJoin("Objects");
	public string spriteResourcePath = Global.ResourcePath.PathJoin("Sprites");
	public string dataResourcePath = Global.ResourcePath.PathJoin("Datas");
	
	[Export] private Dictionary<string, PackedScene> _objectDict = new();
	[Export] public Dictionary<string, Texture2D> _spriteDict = new();
	[Export] public Dictionary<string, Resource> _dataDict = new();

	/// <summary>
    /// 缓存并获取物体资源（tscn）
    /// </summary>
    /// <param name="path">资源地址，使用Folder1.Folder2.resname的格式</param>
    /// <returns></returns>
	public PackedScene GetObject(string path)
    {
        if (_objectDict.ContainsKey(path))
        {
            return _objectDict[path];
        }
		path = Tools.Path(path,objectResourcePath);
		path += ".tscn";
        try
        {
            PackedScene obj = ResourceLoader.Load<PackedScene>(path);
			_objectDict.Add(path, obj);
			return obj;
        }
        catch(Exception e)
        {
            GD.PrintErr($"AssetManager加载物体资源出错：{e}");
			return null;
        }
    }
	/// <summary>
    /// 缓存并获取精灵资源
    /// </summary>
    /// <param name="path">资源地址，使用Folder1.Folder2.resname的格式</param>
    /// <returns></returns>
	public Texture2D GetSprite(string path)
    {
        if (_objectDict.ContainsKey(path))
        {
            return _spriteDict[path];
        }
		path = Tools.Path(path,spriteResourcePath);
        try
        {
            Texture2D sprite = ResourceLoader.Load<Texture2D>(path);
			_spriteDict.Add(path, sprite);
			return sprite;
        }
        catch(Exception e)
        {
            GD.PrintErr($"AssetManager加载精灵资源出错：{e}");
			return null;
        }
    }
	/// <summary>
    /// 缓存并获取数据资源（tres）
    /// </summary>
    /// <param name="path">资源地址，使用Folder1.Folder2.resname的格式</param>
    /// <returns></returns>
	public Resource GetData(string path)
    {	
        if (_objectDict.ContainsKey(path))
        {
            return _dataDict[path];
        }
		path = Tools.Path(path,dataResourcePath);
		path += ".tres";
        try
        {
            Resource data = ResourceLoader.Load<Resource>(path);
			_dataDict.Add(path, data);
			return data;
        }
        catch(Exception e)
        {
            GD.PrintErr($"AssetManager加载DATA资源出错：{e}");
			return null;
        }
    }
}
