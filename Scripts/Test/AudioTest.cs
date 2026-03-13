using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public partial class AudioTest : Node
{
	[Export] public Button button;
	[Export] public AudioStreamPlayer audioPlayer1 = null;
	[Export] public AudioStreamPlayer audioPlayer2 = null;
	public bool trans = false;
	public Dictionary<AudioStreamPlayer, float> targetVolume = new();
	public string state = "smooth";
	public override void _Ready()
	{
		targetVolume = new()
		{
			{audioPlayer1, 0.1f},
			{audioPlayer2, 1}
		};
		button.Pressed += () =>
		{
			if (!trans)
			{
				if(state == "smooth")
				{
					state = "fast";
					targetVolume = new()
					{
						{audioPlayer1, 1},
						{audioPlayer2, 0.1f}
					};
				}
				else
				{
					state = "smooth";
					targetVolume = new()
					{
						{audioPlayer1, 0.1f},
						{audioPlayer2, 1}
					};
				}
				trans = true;
			}
		};
	}
	public override void _Process(double delta)
	{
		if(trans)
		{
			float v1 = audioPlayer1.VolumeLinear;
			float v2 = audioPlayer2.VolumeLinear;
			v1 = (float)Mathf.Lerp(v1, targetVolume[audioPlayer1], delta * 2);
			v2 = (float)Mathf.Lerp(v2, targetVolume[audioPlayer2], delta * 2);
			if(Mathf.Abs(v1 - targetVolume[audioPlayer1]) < 0.01f && Mathf.Abs(v2 - targetVolume[audioPlayer2]) < 0.01f)
			{
				trans = false;
				v1 = targetVolume[audioPlayer1];
				v2 = targetVolume[audioPlayer2];
			}
			audioPlayer1.VolumeLinear = v1;
			audioPlayer2.VolumeLinear = v2;
		}
	}
}
