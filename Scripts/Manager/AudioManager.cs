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

	public float masterVolume = 1f;
	public float musicVolume = 0.5f;
	public float soundVolume = 0.5f;

	int busIndexMaster = 0;
	int busIndexMusic = 1;
	int busIndexSound = 2;

	public void Init()
    {
		UpdateVolume();
		busIndexMaster = AudioServer.GetBusIndex("Master");
		busIndexMusic = AudioServer.GetBusIndex("Music");
		busIndexSound = AudioServer.GetBusIndex("Sound");
    }

	/// <summary>
	/// 设置通道音量
	/// </summary>
	public void UpdateBusVolume()
	{
		AudioServer.SetBusVolumeLinear(busIndexMaster, masterVolume);
		AudioServer.SetBusVolumeLinear(busIndexMusic, musicVolume);
		AudioServer.SetBusVolumeLinear(busIndexSound, soundVolume);
	}

	/// <summary>
	/// 从配置里加载音量配置
	/// </summary>
	public void UpdateVolume()
	{
		try
		{
			Dictionary volumeConfig = Global.Config.AsGodotDictionary()["volume"].AsGodotDictionary();
			masterVolume = (float)volumeConfig["master_volume"].AsDouble();
			musicVolume = (float)volumeConfig["music_volume"].AsDouble();
			soundVolume = (float)volumeConfig["sound_volume"].AsDouble();
		}
		catch(Exception e)
		{
			GD.PrintErr($"Core：获取媒体管理器音量配置失败！{e}");
			masterVolume = 1f;
			musicVolume = 0.5f;
			soundVolume = 0.5f;
		}
		UpdateBusVolume();
	}

	/// <summary>
	/// 更新音量
	/// </summary>
	/// <param name="musicVolume"></param>
	/// <param name="soundVolume"></param>
	public void UpdateVolume(float masterVolume, float musicVolume, float soundVolume)
	{	
		GD.Print($"Core：更新媒体管理器音量:{masterVolume} {musicVolume} {soundVolume}");
		Global.Config.AsGodotDictionary()["volume"].AsGodotDictionary()["master_volume"] = masterVolume;
		Global.Config.AsGodotDictionary()["volume"].AsGodotDictionary()["music_volume"] = musicVolume;
		Global.Config.AsGodotDictionary()["volume"].AsGodotDictionary()["sound_volume"] = soundVolume;
		Global.SaveConfig();
		this.masterVolume = masterVolume;
		this.musicVolume = musicVolume;
		this.soundVolume = soundVolume;
		UpdateBusVolume();
	}

	/// <summary>
	/// 获取游戏内音乐
	/// </summary>
	/// <param name="musicName"></param>
	/// <returns></returns>
	public AudioStream GetInGameMusic(string musicName)
	{
		AudioStream audioStream = AssetManager.Instance.GetAudio($"InGame.{musicName}",".mp3");
		GD.Print($"Core：获取音乐{musicName}:{audioStream}");
		return audioStream;
	}

	/// <summary>
	/// 获取视频资源
	/// </summary>
	/// <param name="videoName"></param>
	/// <returns></returns>
	public VideoStreamTheora GetVideo(string videoName)
	{
		VideoStreamTheora videoStream = AssetManager.Instance.GetVideo(videoName);
		GD.Print($"Core：获取视频{videoName}:{videoStream}");
		return videoStream;
	}
}
