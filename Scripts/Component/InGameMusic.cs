using Godot;
using Godot.Collections;
using System;

public enum MusicState
{
	FightSmooth,
	FightFast,
}

/// <summary>
/// 游戏内音乐
/// </summary>
public partial class InGameMusic : Node
{
	[Export]public Array<AudioStreamPlayer> audioPlayerList = new Array<AudioStreamPlayer>();
	[Export]public MusicState musicState = MusicState.FightSmooth;
	public void ChangeState()
	{
		
	}

}
