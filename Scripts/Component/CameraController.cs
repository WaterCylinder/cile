using Godot;
using System;

public partial class CameraController : Camera2D
{
	[Export]public Vector2 targetPos = Vector2.Zero;
	[Export]public float moveSpeed = 15;
	[Export]public Vector2 posLimitX = new Vector2(-300, 300);
	[Export]public Vector2 posLimitY = new Vector2(-300, 300);
	[Export]public float targetZoom = 1;
	[Export]public float zoomSpeed = 5;
	[Export]public Vector2 zoomLimit = new Vector2(0.5f, 8);

	public Vector2 mousePos;
	public bool isMousePressed = false;
    public bool isDrug = false;
	public Vector2 mousePressedPos;

    /// <summary>
    /// 信号：屏幕开始移动
    /// </summary>
    [Signal]public delegate void OnScreenStartMoveEventHandler();

    public override void _Process(double delta)
    {	
        base._Process(delta);
		Vector2 toward = targetPos - this.Position;
		if (toward.Length() > 0.1)
        {
            Vector2 pos = this.Position + toward * (float)delta * moveSpeed;
			this.SetPosition(pos);
        }
		float zoom = this.Zoom.X + (targetZoom - this.Zoom.X) * (float)delta * zoomSpeed;
		this.Zoom = new Vector2(zoom, zoom);
		Move();
    }

    /// <summary>
    /// 获取世界坐标到屏幕左边的转换
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public Vector2 ToScreenPosition(Vector2 worldPos)
    {
        // 2. 获取视口在屏幕上的实际位置和大小
        Rect2 viewportRect = GetViewport().GetVisibleRect();  // 视口在屏幕上的矩形
        
        // 3. 计算视口中心在屏幕上的位置
        Vector2 viewportCenter = new Vector2(
            viewportRect.Position.X + viewportRect.Size.X * 0.5f,
            viewportRect.Position.Y + viewportRect.Size.Y * 0.5f
        );
        
        // 4. 计算相对于相机的偏移
        Vector2 relativePos = worldPos - GlobalPosition;
        
        // 5. 应用缩放
        relativePos *= Zoom.X;
        
        // 6. 转换到屏幕坐标
        Vector2 screenPos = viewportCenter + relativePos;

        GD.Print($"中心位置{viewportCenter}，相对位置{relativePos}，最终位置{screenPos}");
        
        return screenPos;
    }

	/// <summary>
    /// 设置相机目标位置
    /// </summary>
    /// <param name="pos"></param>
	public void SetTargetPosition(Vector2 pos)
    {
        targetPos = pos;
		targetPos = new Vector2(
			Mathf.Clamp(targetPos.X, posLimitX.X, posLimitX.Y),
			Mathf.Clamp(targetPos.Y, posLimitY.X, posLimitY.Y)
		);
    }

    /// <summary>
    /// 设置相机目标缩放
    /// </summary>
    /// <param name="zoom"></param>
    public void SetTargetZoom(float zoom)
    {
        targetZoom = zoom;
		targetZoom = Mathf.Clamp(targetZoom, zoomLimit.X, zoomLimit.Y);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
		if(@event is InputEventMouseButton mouseEvent)
        {
            if(mouseEvent.ButtonIndex == MouseButton.WheelUp)
            {	
				if(targetZoom < zoomLimit.Y)
                {
                    targetZoom *= 1.1f;
                }
            }
			if(mouseEvent.ButtonIndex == MouseButton.WheelDown)
            {
				if(targetZoom > zoomLimit.X)
                {
                    targetZoom /= 1.1f;
                }
            }
			if (mouseEvent.Pressed)
			{
                mousePos = GetGlobalMousePosition();
				if(mouseEvent.ButtonIndex == MouseButton.Left)
                {
					mousePressedPos = mousePos;
                    isMousePressed = true;
                }
			}
			else
			{
				if(mouseEvent.ButtonIndex == MouseButton.Left)
                {
                    isMousePressed = false;
                }
			}
        }
    }

    //相机移动方法
	private void Move()
    {
        if(isMousePressed)
        {   
            mousePos = GetGlobalMousePosition();
            if (!isDrug)
            {
                Vector2 toward = mousePos - mousePressedPos;
                if (toward.Length() > 5)
                {
                    EmitSignal("OnScreenStartMove");
                    isDrug = true;
                }
            }
            else
            {
                Vector2 toward = mousePos - mousePressedPos;
                SetTargetPosition(targetPos - toward * 4);
                mousePressedPos = mousePos;
            }
        }
        else
        {
            isDrug = false;
        }
    }


}
