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

	public override void _Ready()
    {	
		GD.Print("初始化GameManager");
        ProcessMode = ProcessModeEnum.Always;
		GameManager.instance = this;
    }

}
