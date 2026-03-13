using Godot;
using System;

public partial class DebugUI : Node
{
	[Export] public Button nextRoundButton;
	[Export] public RichTextLabel turnText;

	public override void _Ready()
	{
		nextRoundButton.Pressed += () =>
        {
            GameManager.Instance.game.roundCricle.NextTurn();
        };
	}

    public override void _Process(double delta)
    {
        base._Process(delta);
		RoundCricle rc = GameManager.Instance.game.roundCricle;
        // 修改Debug文本
        // 回合信息
        try
        {
			Turn tl = rc.turnLogic;
            turnText.Text = 
				$"第{rc.round}轮\n" +
				$"第{rc.turn}回合\n" +
				$"当前玩家name: {tl.turnPlayer.user.name}\n" +
				$"当前玩家id: {tl.turnPlayer.user.id}\n" +
				$"- 玩家状态: {tl.turnPlayer.state}\n" +
				$"- 玩家行动点: {tl.turnPlayer.activatePoint}\n" +
				$"- 玩家资源点: {tl.turnPlayer.resourcesPoint}\n" +
				$"- 玩家单位死亡数: {tl.turnPlayer.death}\n" +
				$"- 玩家手牌: {tl.turnPlayer.handCards}\n" +
				$"canOpera: {tl.canOpera}\n" +
				$"isYouCanOpera: {tl.isYouCanOpera}\n";
        }
		catch
		{
			turnText.Text = "错误";
		}
			
    }

}
