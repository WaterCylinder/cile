using Godot;
using Godot.Collections;
using System;

/*
SpriteSheet精灵组动画
*/

public partial class SpriteSheetAnimation : AnimatedSprite2D
{
	[Export] public Texture2D spriteSheet;
	[Export] public int frameHight;
	[Export] public int frameWidth;
	[Export] public Array<Texture2D> sprites = new Array<Texture2D>();

	[Export] public int frameCount;
	
	public override void _Ready()
    {
        Cut();
		SetAnimatedSprite();
		Play("default");
    }

	public void Cut()
    {
        Vector2 sheetSize =  spriteSheet.GetSize();
		int frameYCount = (int)(sheetSize.Y / frameHight);
		int frameXCount = (int)(sheetSize.X / frameWidth);
		Image image = spriteSheet.GetImage();
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

	public void SetAnimatedSprite()
    {
        if(sprites.Count <= 0)
        {
            return;
        }
		SpriteFrames frames = SpriteFrames;
		foreach (var item in sprites)
		{
			frames.AddFrame("default",item);
		}
		GD.Print($"添加动画");
		
    }
}
