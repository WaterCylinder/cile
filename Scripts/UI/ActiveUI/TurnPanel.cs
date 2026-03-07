using Godot;
using System;

public partial class TurnPanel : Control
{
	[Export] public Label label;

    [Export]private string state = "ready";

	public void SetTurnCount(int turnCount)
    {
        label.Text = $"第 {turnCount} 回合";
    }
    
    float timer = 0;
	public void PlayAnimation()
    {
        this.SetPosition(new Vector2(-320, -64));
        state = "playing";
        timer = 0;
    }

    public override void _Process(double delta)
    {
        if(state == "playing")
        {
            Vector2 newPos = this.Position;
            newPos.Y += 100 * (float)delta;
            if (newPos.Y > 0)
            {
                state = "waiting";
            }
            this.SetPosition(newPos);
        }
        else if(state == "waiting")
        {
            timer += (float)delta;
            if (timer > 0.879f)
            {
                state = "returning";
                timer = 0;
            }
        }
        else if(state == "returning")
        {
            Vector2 newPos = this.Position;
            newPos.Y -= 100 * (float)delta;
            if (newPos.Y < -64)
            {
                state = "ready";
                newPos.Y = -64;
            }
            this.SetPosition(newPos);
        }
    }

}
