using Godot;
using System;

public partial class GameManager : Node
{	
	private static GameManager instance;
	public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GD.PrintErr("GlobalManager 尚未初始化！请确保在项目设置中启用了AutoLoad");
            }
            return instance;
        }
    }

    public static CardManager CM => CardManager.Instance;
    public static SceneManager SM => SceneManager.Instance;
    public static InputManager IM => InputManager.Instance;

    public Game game;

	public override void _Ready()
    {	
		GD.Print("初始化GameManager");
        ProcessMode = ProcessModeEnum.Always;
		GameManager.instance = this;

        InitCoreManager();
    }

    private void InitCoreManager() //初始化核心管理器
    {
        CardManager.Instance.Init();
        SceneManager.Instance.Init();
        InputManager.Instance.Init();
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
        GetTree().Quit();
    }

}
