using Godot;
using Godot.Collections;

public partial class GraphSetting : Control
{	
	[Export] public ColorSheet colorSheet;
	[Export] public Array<TextureButton> colorSelectors;

	public override void _Ready()
	{
		for(int i = 0; i < colorSelectors.Count; i++)
		{	
			int index = i;
			colorSelectors[i].Pressed += () =>
			{
				SetColor(index);
			};
			Color color = Global.Config.AsGodotDictionary()["color_schemes"].AsGodotArray()[i].AsGodotDictionary()["color2"].As<Color>();
			colorSelectors[i].SelfModulate = color;
		}
	}

	/// <summary>
	/// 设置颜色并更新，直接更改到配置文件
	/// </summary>
	/// <param name="index"></param>
	public void SetColor(int index)
	{
		GD.Print($"设置颜色为{index}号主题");
		Array colors = Global.Config.AsGodotDictionary()["color_schemes"].AsGodotArray();
		Dictionary color = colors[index].AsGodotDictionary();
		Global.Config.AsGodotDictionary()["menu_color"] = color;
		Global.SaveConfig();
		colorSheet.UpdateColor();
	}
}
