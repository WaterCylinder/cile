using Godot;
using System;

public partial class AudioManager : Node
{
	private static AudioManager instance;
	public static AudioManager Instance
    {
        get
        {
            if(instance == null)
            {
                try
				{
					GD.Print("Core：正在初始化媒体管理器");
					instance = new AudioManager();
					instance.Name = "AudioManager";
					instance.ProcessMode = ProcessModeEnum.Always;
					GameManager.Instance.AddChild(instance);
				}
				catch(Exception e)
				{
					GD.PrintErr($"Core：初始化媒体管理器失败！{e}");
				}
            }
			return instance;
        }
    }

	public void Init()
    {
        
    }
}
