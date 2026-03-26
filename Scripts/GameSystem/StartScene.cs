using Godot;
using System;
using System.Diagnostics;

public partial class StartScene : Node
{
	[Export]public VideoStreamPlayer videoPlayer;
	[Export]public ColorRect frontBlack;

	private bool isVideoFinished = false;

	public override void _Ready()
	{	
		isVideoFinished = false;
		frontBlack.Color = new Color(0, 0, 0, 0);

		VideoStreamTheora video = AudioManager.Instance.GetVideo("cile_logo");
		if(video != null)
		{
			videoPlayer.Stream = video;
			videoPlayer.Play();
		}

		videoPlayer.Finished += () =>
		{
			isVideoFinished = true;
		};
	}

	float alpha = 0;

	public override void _Process(double delta)
	{
		if (isVideoFinished)
		{
			alpha += (float)delta * 2f;
			if (alpha >= 1)
			{
				GD.Print("LOGO展示结束，跳转到主界面");
				GameManager.SM.CheckTo("MainMenu");
			}
			frontBlack.Color = new Color(0, 0, 0, alpha);
		}
	}
}
