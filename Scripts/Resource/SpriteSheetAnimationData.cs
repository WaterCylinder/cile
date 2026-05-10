using System;
using Godot;
using Godot.Collections;

/*
(已弃用， 请使用Godot的原生动画系统)
*/
[GlobalClass]
[Tool]
public partial class SpriteSheetAnimationData : Resource
{
	[Export] public Texture2D spriteSheet;
	[Export] public int frameHeight;
	[Export] public int frameWidth;

	[ExportToolButton("切分SpriteSheet")]
	public Callable CutButton => Callable.From(Cut);
	[ExportCategory("动画列表")]
	[Export] public Array<AnimationAction> actionList = new();
	[ExportCategory("只读属性")]
	[Export] public int frameCount;
	[Export] public Array<Texture2D> frames = new();
	
	Dictionary<string, AnimationAction> cache;

	public void Init()
	{
		if(frames.Count == 0)
		{
			Cut();
		}
	}

	/// <summary>
	/// 切割spritesheet
	/// </summary>
	public void Cut()
    {
        Vector2 sheetSize =  spriteSheet.GetSize();
		int frameYCount = (int)(sheetSize.Y / frameHeight);
		int frameXCount = (int)(sheetSize.X / frameWidth);
		Image image = spriteSheet.GetImage();
		frames.Clear();
		for (int i = 0; i < frameYCount; i++)
		{
			for (int j = 0; j < frameXCount; j++)
			{
				Rect2I ract = new Rect2I(j * frameWidth, i * frameHeight, frameWidth, frameHeight);
				Image subImage = image.GetRegion(ract);
				frames.Add(ImageTexture.CreateFromImage(subImage));
			}
		}
		frameCount = frameXCount * frameYCount;
		GD.Print($"{spriteSheet.ResourceName}分割了{frameCount}帧");
    }

	public AnimationAction GetAction(string actionName)
	{
		AnimationAction action = null;
		if (cache.ContainsKey(actionName))
		{
			action = cache[actionName];
		}
		else
		{
			foreach(AnimationAction a in actionList)
			{
				if(a.actionName == actionName)
					action = a;
			}
		}
		return action;
	}

	/// <summary>
	/// 获取对应动画的精灵组
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	public Array<Texture2D> GetActionSprites(AnimationAction action)
	{
		if(frames.Count == 0)
		{
			Cut();
		}
		if(action.startIndex < 0 || action.endIndex >= frames.Count)
		{
			GD.Print($"获取动画{action.actionName}的图像集失败，index超出边界");
			return null;
		}
		return frames[action.startIndex..(action.endIndex + 1)];
	}

	/// <summary>
	/// 获取对应动画名称的精灵组
	/// </summary>
	/// <param name="actionName"></param>
	/// <returns></returns>
	public Array<Texture2D> GetActionSprites(string actionName)
	{
		AnimationAction action = GetAction(actionName);
		if(action == null)
		{
			return null;
		}
		return GetActionSprites(action);
	}

	/// <summary>
	/// 设置只读属性
	/// </summary>
	/// <param name="property"></param>
	public override void _ValidateProperty(Dictionary property)
    {
        string propName = property["name"].AsString();
        if (propName == "frameCount" || propName == "frames")
        {
            property["usage"] = (int)(PropertyUsageFlags.ReadOnly | PropertyUsageFlags.Editor);
        }
    }

    public override bool _Set(StringName property, Variant value)
    {
		GD.Print("属性设置");
        if (property == "spriteSheet" || property == "frameHeight" || property == "frameWidth")
        {
			GD.Print("属性设置");
            Cut();
			return false;
        }
		return base._Set(property, value);
    }

}
