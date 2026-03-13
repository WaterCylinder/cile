using Godot;
using Godot.Collections;
using System;

public partial class SettingMenu : Control
{
	[Export] public TextureButton backButton;
	[Export] public TextureButton frontButton;
	[Export] public TextureButton nextButton;
	[Export] public Array<TextureButton> buttonList;
	[Export] public Array<Control> planeList;

	public int settingIndexNow = 0;
	public int settingIndexBefore = -1;

	public override void _Ready()
	{
		backButton.Pressed += () =>
		{
			GameManager.SM.CheckBack();
		};
		frontButton.Pressed += () =>
		{
			settingIndexNow--;
			if (settingIndexNow < 0)
			{
				settingIndexNow = buttonList.Count - 1;
			}
			CheckButton();
		};
		nextButton.Pressed += () =>
		{
			settingIndexNow++;
			if (settingIndexNow >= buttonList.Count)
			{
				settingIndexNow = 0;
			}
			CheckButton();
		};
		for (int i = 0; i < buttonList.Count; i++)
		{
			int index = i;
			buttonList[i].Pressed += () =>
			{
				CheckButton(index);
			};
		}
		CheckButton(0);
	}

	/// <summary>
	/// 切换设置
	/// </summary>
	/// <param name="index"></param>
	public void CheckButton(int index = -1)
	{
		if(index < 0)
		{
			index = settingIndexNow;
		}
		GD.Print($"切换设置为{index}号设置，{buttonList[index]}");
		if(settingIndexBefore >= 0)
			planeList[settingIndexBefore].Visible = false;
		planeList[index].Visible = true;
		settingIndexBefore = index;
		settingIndexNow = index;
	}


}
