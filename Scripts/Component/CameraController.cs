using Godot;
using System;

public partial class CameraController : Camera2D
{
	[Export]public Vector2 targetPos = Vector2.Zero;
	[Export]public float moveSpeed = 8;
	[Export]public Vector2 posLimitX = new Vector2(-300, 300);
	[Export]public Vector2 posLimitY = new Vector2(-300, 300);
	[Export]public float targetZoom = 1;
	[Export]public float zoomSpeed = 5;
	[Export]public Vector2 zoomLimit = new Vector2(0.5f, 8);

	public Vector2 mousePos;
	public bool isMousePressed = false;
	public Vector2 mousePressedPos;

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

	private void SetTargetPosition(Vector2 pos)
    {
        targetPos = pos;
		targetPos = new Vector2(
			Mathf.Clamp(targetPos.X, posLimitX.X, posLimitX.Y),
			Mathf.Clamp(targetPos.Y, posLimitY.X, posLimitY.Y)
		);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
		if(@event is InputEventMouseButton mouseEvent)
        {
			mousePos = GetGlobalMousePosition();
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

	public void Move()
    {
        if(isMousePressed)
        {
			mousePos = GetGlobalMousePosition();
            Vector2 toward = mousePressedPos - mousePos;
            SetTargetPosition(targetPos + toward);
            mousePressedPos = mousePos;
        }
    }


}
