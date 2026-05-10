using Godot;
using Godot.Collections;
using System;

/*
SpriteSheet精灵组动画(已弃用)
*/

public partial class SpriteSheetAnimation : AnimatedSprite2D
{
	[Export] public SpriteSheetAnimationData spriteSheet;

	public bool initDown = false;

	public void Init()
	{
		spriteSheet.Init();
		foreach(Texture2D sprite in spriteSheet.frames)
		{
			SpriteFrames.AddFrame("default", sprite);
		}
		foreach(AnimationAction action in spriteSheet.actionList)
		{
			string name = action.actionName;
			GD.Print($"添加动画{name}");
			Array<Texture2D> sprites = spriteSheet.GetActionSprites(action);
			foreach(Texture2D sprite in sprites)
			{
				SpriteFrames.AddFrame(name, sprite);
			}
		}
		Play("default");
	}

    public override void _Process(double delta)
    {
        if(initDown == false && spriteSheet != null)
		{
			Init();
			initDown = true;
		}
    }

	/*
	[Export] public SpriteSheetAnimationData spriteSheet;
	[Export] public Dictionary<string,SpriteSheetAnimationAction>
	
	public override void _Ready()
    {
        Cut();
		SetDefaultAnimatedSprite();
		Play("default");
    }

	/// <summary>
	/// 切割spritesheet
	/// </summary>
	public void Cut()
    {
        Vector2 sheetSize =  spriteSheet.GetSize();
		int frameYCount = (int)(sheetSize.Y / frameHight);
		int frameXCount = (int)(sheetSize.X / frameWidth);
		Image image = spriteSheet.GetImage();
		sprites.Clear();
		for (int i = 0; i < frameYCount; i++)
		{
			for (int j = 0; j < frameXCount; j++)
			{
				Rect2I ract = new Rect2I(j * frameWidth, i * frameHight, frameWidth, frameHight);
				Image subImage = image.GetRegion(ract);
				sprites.Add(ImageTexture.CreateFromImage(subImage));
			}
		}
		frameCount = frameXCount * frameYCount;
		GD.Print($"{spriteSheet.ResourceName}分割了{frameCount}帧");
    }

	public void SetDefaultAnimatedSprite()
    {
        if(sprites.Count <= 0)
        {
            return;
        }
		foreach (var item in sprites)
		{
			SpriteFrames.AddFrame("default",item);
		}
		GD.Print($"添加动画");
    }*/
}
