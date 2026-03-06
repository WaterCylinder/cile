using Godot;
using System;
using Godot.Collections;
using System.IO;

/*
* 场景管理器
*/
public partial class SceneManager : Node
{
	private static SceneManager instance;
	public static SceneManager Instance
    {
        get
        {
            if(instance == null)
            {
                try
				{
					GD.Print("Core：正在初始化场景管理器");
					instance = new SceneManager();
					instance.Name = "SceneManager";
					instance.ProcessMode = ProcessModeEnum.Always;
					GameManager.Instance.AddChild(instance);
				}
				catch(Exception e)
				{
					GD.PrintErr($"Core：初始化场景管理器失败！{e}");
				}
            }
			return instance;
        }
    }

	[Export] public Dictionary<string, PackedScene> sceneDict = new();

	[Export] public Array<string> sceneArray;
	[Export] public string rootScene;
	[Export] public int sceneIndexNow = 0;

	public void Init()
    {
        sceneArray = new();
		rootScene = "MainMenu";
    }
	/// <summary>
    /// 切换场景
    /// </summary>
    /// <param name="scene"></param>
	public void ChangeScene(PackedScene scene)
    {
		GD.Print($"Core：切换到场景:{scene.ResourcePath}");
		GetTree().ChangeSceneToPacked(scene);
    }
	/// <summary>
    /// 切换场景
    /// </summary>
    /// <param name="sceneName"></param>
	public void ChangeScene(string sceneName)
    {	
		if(sceneDict.ContainsKey(sceneName))
        {
            ChangeScene(sceneDict[sceneName]);
			return;
        }
		string fileName = sceneName;
		if(!sceneName.EndsWith(".tscn"))
        	fileName += ".tscn";
		string path = Global.ScenePath.PathJoin(fileName);
		PackedScene scene = ResourceLoader.Load<PackedScene>(path);
		sceneDict.Add(sceneName, scene);
		ChangeScene(scene);
    }
	
	/// <summary>
    /// 前往下一个场景，没有下一个场景则重新加载当前场景
    /// </summary>
    /// <param name="sceneName"></param>
	public void CheckForward(string sceneName = "")
    {
        if(sceneName == "")
        {
			sceneIndexNow++;
			if(sceneIndexNow >= sceneArray.Count)
            {
				sceneIndexNow--;
            }
			ChangeScene(sceneArray[sceneIndexNow - 1]);
        }
        else
        {
            ChangeScene(sceneName);
        }
    }
	/// <summary>
    /// 返回上一个场景
    /// </summary>
	public void CheckBack()
    {
        sceneIndexNow--;
		if(sceneIndexNow < 0)
        {
			sceneIndexNow = 0;
			ChangeScene(rootScene);
        }
        else
        {
            ChangeScene(sceneArray[sceneIndexNow - 1]);
        }

    }
	/// <summary>
    /// 切换到独立场景，清空场景列表
    /// </summary>
    /// <param name="sceneName"></param>
	public void CheckTo(string sceneName = "")
    {   
        if(sceneName == "")
        {
            sceneName = rootScene;
        }
        sceneArray = new();
		ChangeScene(sceneName);
    }
}
