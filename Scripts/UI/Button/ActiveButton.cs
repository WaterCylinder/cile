using Godot;
using System;

public partial class ActiveButton : TextureButton
{
	[Export]public Label label;
	public void SetText(string text)
	{
		label.Text = text;
	}
}
