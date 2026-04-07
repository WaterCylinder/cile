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

		//延迟加载AssetManager
		CallDeferred(nameof(Init));

		videoPlayer.Finished += () =>
		{
			isVideoFinished = true;
		};
	}

	public void Init()
	{
		VideoStreamTheora video = AudioManager.Instance.GetVideo("cile_logo");
		if(video != null)
		{
			videoPlayer.Stream = video;
			videoPlayer.Play();
		}
	}

	float alpha = 0;

	public override void _Process(double delta)
	{
		if (isVideoFinished)
		{
			alpha += (float)delta * 4f;
			if (alpha >= 1)
			{
				GD.Print("LOGO展示结束，跳转到主界面");
				GameManager.SM.CheckTo("MainMenu");
			}
			frontBlack.Color = new Color(0, 0, 0, alpha);
		}
	}

    public override void _Input(InputEvent @event)
    {
		if (@event.IsPressed())
		{
			GD.Print("按键跳过LOGO");
			isVideoFinished = true;
		}
    }

}
