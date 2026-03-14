using Godot;
using Godot.Collections;
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

	public float musicVolume = 0.5f;
	public float soundVolume = 0.5f;

	public void Init()
    {
		UpdateVolume();
    }

	/// <summary>
	/// 从配置里加载音量配置
	/// </summary>
	public void UpdateVolume()
	{
		try
		{
			Dictionary volumeConfig = Global.Config.AsGodotDictionary()["volume"].AsGodotDictionary();
			musicVolume = (float)volumeConfig["music_volume"].AsDouble();
			soundVolume = (float)volumeConfig["sound_volume"].AsDouble();
		}
		catch(Exception e)
		{
			GD.PrintErr($"Core：获取媒体管理器音量配置失败！{e}");
			musicVolume = 0.5f;
			soundVolume = 0.5f;
		}
		
	}

	/// <summary>
	/// 更新音量
	/// </summary>
	/// <param name="musicVolume"></param>
	/// <param name="soundVolume"></param>
	public void UpdateVolume(float musicVolume, float soundVolume)
	{
		Global.Config.AsGodotDictionary()["volume"].AsGodotDictionary()["music_volume"] = musicVolume;
		Global.Config.AsGodotDictionary()["volume"].AsGodotDictionary()["sound_volume"] = soundVolume;
		Global.SaveConfig();
		this.musicVolume = musicVolume;
		this.soundVolume = soundVolume;
	}

	public AudioStream GetInGameMusic(string musicName)
	{
		AudioStream audioStream = AssetManager.Instance.GetAudio($"InGame.{musicName}",".mp3");
		GD.Print($"Core：获取音乐{musicName}:{audioStream}");
		return audioStream;
	}
}
