using Godot;
using System;

public partial class TextureButtonColorChangeByState : TextureButton
{
	[Export] public Control modulate;
	/// <summary>
    /// 自动搜索的Label对象
    /// </summary>
	[Export] public Label label;
	/// <summary>
    /// 当前颜色
    /// </summary>
	[Export] public Color color;
	/// <summary>
    /// 默认颜色
    /// </summary>
	[Export] public Color normalColor = Colors.White;
	/// <summary>
    /// 鼠标进入颜色
    /// </summary>
	[Export] public Color enterColor = Colors.LightPink;
	/// <summary>
    /// 鼠标按下颜色
    /// </summary>
	[Export] public Color pressedColor = Colors.Gray;
	public Label InnerLabel
    {
        get
        {
			if (label == null)
			{
				label = GetChild<Label>(0);
			}
            return label;
        }
    }

	public override void _Ready()
    {	
		if(modulate == null)
        {
            modulate = this;
        }
		this.MouseEntered += () =>
		{
			color = enterColor;
			modulate.SelfModulate = color;
		};
		this.MouseExited += () =>
		{
			color = normalColor;
			modulate.SelfModulate = color;
		};
		this.Pressed += () =>
		{
			color = pressedColor;
			modulate.SelfModulate = color;
		};
    }
}
