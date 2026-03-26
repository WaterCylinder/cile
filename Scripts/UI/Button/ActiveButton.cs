using Godot;
using Godot.Collections;
using System;
using System.ComponentModel;

public partial class ActiveButton : TextureButton
{
	[Export]public Label label;
	[Export]public Color colorNormal;
	[Export]public Color textColor;
	[Export]public Color colorDisable;
	[Export]public Color textColorDark;

    public override void _Ready()
    {
        base._Ready();
		Dictionary colors = Global.config.As<Dictionary>()["menu_color"].As<Dictionary>();
		colorNormal = colors["color1"].As<Color>();
		textColor = colors["color4"].As<Color>();
		colorDisable = colorNormal.Darkened(0.5f);
		textColorDark = textColor.Darkened(0.5f);
	}

	public void SetText(string text)
	{
		label.Text = text;
	}

	public void Switch(bool able = false)
	{
		Disabled = !able;
		if (Disabled)
		{
			SelfModulate = colorDisable;
			label.SelfModulate = textColorDark;
		}
		else
		{
			SelfModulate = colorNormal;
			label.SelfModulate = textColor;
		}
	}
}
