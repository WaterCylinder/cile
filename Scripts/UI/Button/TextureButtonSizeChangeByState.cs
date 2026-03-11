using Godot;
using System;

public partial class TextureButtonSizeChangeByState : TextureButton
{
	public float baseScale;

	public float targetScale;

	public override void _Ready()
    {	
		baseScale = Scale.X;
		targetScale = baseScale;
		MouseEntered += () =>
		{
			targetScale = baseScale * 1.08f;
		};
		MouseExited += () =>
		{
			targetScale = baseScale;
		};
		Toggled += (bool toggled) =>
		{
			targetScale = baseScale * 0.92f;
		};
		
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
		float scale = Scale.X + (targetScale - Scale.X) * (float)delta * 23;
		if (Mathf.Abs(targetScale - Scale.X) < 0.01f)
        {
            scale = targetScale;
        }
		Scale = new Vector2(scale, scale);
    }

}
