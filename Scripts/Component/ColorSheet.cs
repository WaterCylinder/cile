using Godot;
using Godot.Collections;
using System;

public partial class ColorSheet : Node
{
	/// <summary>
    /// 一般为背景色
    /// </summary>
	[Export] public Array<Control> colorSheet1 = new Array<Control>();
	/// <summary>
    /// 一般为边框色
    /// </summary>
	[Export] public Array<Control> colorSheet2 = new Array<Control>();
	/// <summary>
    /// 一般为内容色
    /// </summary>
	[Export] public Array<Control> colorSheet3 = new Array<Control>();
	/// <summary>
    /// 一般为文字色1
    /// </summary>
	[Export] public Array<Control> colorSheet4 = new Array<Control>();
	/// <summary>
    /// 一般为文字色2
    /// </summary>
	[Export] public Array<Control> colorSheet5 = new Array<Control>();
	/// <summary>
    /// 一般为特殊色
    /// </summary>
	[Export] public Array<Control> colorSheet6 = new Array<Control>();

	private bool isUpdate = false;

	public void UpdateColor()
    {	
        Dictionary colors = Global.config.As<Dictionary>()["menu_color"].As<Dictionary>();
		//GD.Print(colors);	
		foreach(Control ui in colorSheet1)
        {
            ui.SelfModulate = colors["color1"].As<Color>();
        }
		foreach(Control ui in colorSheet2)
        {
            ui.SelfModulate = colors["color2"].As<Color>();
        }
		foreach(Control ui in colorSheet3)
        {
            ui.SelfModulate = colors["color3"].As<Color>();
        }
		foreach(Control ui in colorSheet4)
        {
            ui.SelfModulate = colors["color4"].As<Color>();
        }
		foreach(Control ui in colorSheet5)
        {
            ui.SelfModulate = colors["color5"].As<Color>();
        }
		foreach(Control ui in colorSheet6)
        {
            ui.SelfModulate = colors["color6"].As<Color>();
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
		//单次加载颜色
		if (!isUpdate)
        {
            UpdateColor();
            isUpdate = true;
        }
    }

}
