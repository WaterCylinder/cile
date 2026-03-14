using Godot;
using Godot.Collections;
using System;

public enum MusicState
{
	Smooth,
	Fast,
}

/// <summary>
/// 游戏内音乐
/// </summary>
public partial class InGameMusic : Node
{
	[Export]public Array<AudioStreamPlayer> audioPlayerList = new Array<AudioStreamPlayer>();
	[Export]public MusicState musicState = MusicState.Smooth;

	public bool trans = false;
	public Dictionary<int, float> playerVolume = new();

    public override void _Ready()
    {
        AudioManager.Instance.UpdateVolume();
    }

	public void LoadMusic(string musicName)
	{
		GD.Print($"正在加载音乐{musicName}");
		AudioStreamPlayer playerSmooth = audioPlayerList[0];
		playerSmooth.Stream = AudioManager.Instance.GetInGameMusic($"{musicName}_smooth");
		playerSmooth.Play();
		AudioStreamPlayer playerFast = audioPlayerList[1];
		playerFast.Stream = AudioManager.Instance.GetInGameMusic($"{musicName}_fast");
		playerFast.Play();
		playerFast.VolumeLinear = 0.16f;
	}

	/// <summary>
	/// 切换状态
	/// </summary>
	/// <param name="state"></param>
	public void ChangeState(MusicState state)
	{
		if(musicState == state)return;
		if(trans) return;
		GD.Print($"切换音乐状态:{state}");
		if(musicState == MusicState.Smooth && state == MusicState.Fast)
		{
			//温和战斗切换为激烈战斗
			playerVolume = new()
			{
				{ 0, 0.16f },
				{ 1, 1f },
			};
		}
		if(musicState == MusicState.Fast && state == MusicState.Smooth)
		{
			//激烈战斗切换为温和战斗
			playerVolume = new()
			{
				{ 0, 1f },
				{ 1, 0.16f },
			};
		}
		foreach(AudioStreamPlayer player in audioPlayerList)
		{
			if (player.Stream == null) continue;
			if (!player.Playing) player.Play();
		}
		trans = true;
		musicState = state;
	}

    public override void _Process(double delta)
    {
        base._Process(delta);
		//渐变音量切换
		if(trans)
		{
			float transDiff = 999f;
            foreach (var _ in playerVolume)
			{
				int index = _.Key;
				float target = playerVolume[index];
				audioPlayerList[index].VolumeLinear = 
					Mathf.Lerp(audioPlayerList[index].VolumeLinear, target, (float)delta * 3);
				transDiff = Mathf.Min(transDiff, Mathf.Abs(audioPlayerList[index].VolumeLinear - target));
			}
			if(transDiff < 0.01f)
			{
				foreach (var _ in playerVolume)
				{
					int index = _.Key;
					audioPlayerList[index].VolumeLinear = playerVolume[index];
				}
				trans = false;
			}
		}
    }


}
