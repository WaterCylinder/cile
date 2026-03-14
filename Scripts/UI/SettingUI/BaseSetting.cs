using Godot;
using System;

public partial class BaseSetting : Control
{
	[Export]public ScrollBar masterVolumeBar;
	[Export]public ScrollBar musicVolumeBar;
	[Export]public ScrollBar soundVolumeBar;

	[Export]public TextureButton resetButton;

	public override void _Ready()
	{
		AudioManager.Instance.UpdateVolume();
		masterVolumeBar.Value = AudioManager.Instance.masterVolume;
		musicVolumeBar.Value = AudioManager.Instance.musicVolume;
		soundVolumeBar.Value = AudioManager.Instance.soundVolume;
		resetButton.Pressed += () =>
		{
			GD.Print("重置音量");
			AudioManager.Instance.UpdateVolume(1,0.5f,0.5f);
			masterVolumeBar.Value = AudioManager.Instance.masterVolume;
			musicVolumeBar.Value = AudioManager.Instance.musicVolume;
			soundVolumeBar.Value = AudioManager.Instance.soundVolume;
		};
	}

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
		if(@event is InputEventMouseButton)
        {
            if(!@event.IsPressed())
            {
                AudioManager.Instance.UpdateVolume(
					(float)masterVolumeBar.Value, 
					(float)musicVolumeBar.Value, 
					(float)soundVolumeBar.Value
				);
            }
        }
    }

}
