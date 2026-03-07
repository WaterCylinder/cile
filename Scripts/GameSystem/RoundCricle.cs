using Godot;
using System;
using Godot.Collections;
using Utils.Coroutine;

public class RoundCricle
{
	public Coroutine coroutine = null;

	/// <summary>
    /// 当前轮
    /// </summary>
	public int round;

    /// <summary>
    /// 当前回合数
    /// </summary>
    public int turn;

	/// <summary>
    /// 是否正在运行回合循环
    /// </summary>
	public bool isRunning;

    [Export] private Game game;
    private Array<Player> players;

    private Turn turnLogic = new Turn();

    /// <summary>
    /// 切换玩家
    /// </summary>
    /// <param name="index"></param>
    public void ChangePlayer(int index)
    {
        turnLogic.turnPlayer = players[index];
        GD.Print($"切换当前玩家为{turnLogic.turnPlayer.user.name} {turnLogic.turnPlayer.user.id}");
    }

    /// <summary>
    /// 切换到下一个玩家
    /// </summary>
    public void NextPlayer()
    {
        ChangePlayer((turn - 1) % players.Count);
    }

	/// <summary>
    /// 开始回合循环
    /// </summary>
	public void Start()
    {	
        
		game = GameManager.Instance.game;
		GD.Print($"开始回合循环，当前A值{game.A}");
        coroutine = CoroutineManager.Instance.StartCoroutine(Callable.From(Process), 0.5);
		round = 0;
        turn = 0;
		isRunning = true;

        players = game.players;

        turnLogic.roundCricle = this;

        NextTurn();
    }

    /// <summary>
    /// 下一回合
    /// </summary>
    public void NextTurn()
    {   
        turn += 1;
        if(turn % players.Count == 1)
        {
            NextRound();
        }
        GD.Print($"开始第{turn}回合");
        turnLogic.EnterTurn();
        NextPlayer();
    }

    /// <summary>
    /// 下一轮
    /// </summary>
    public void NextRound()
    {
        round += 1;
        GD.Print($"开始第{round}轮");
    }

    /// <summary>
    /// 结束回合循环
    /// </summary>
	public void End()
    {
		game = GameManager.Instance.game;
		GD.Print($"结束回合循环，当前A值{game.A}");
		
		if(coroutine != null)coroutine.Stop();
		else GD.PushWarning("回合循环未开始");
        isRunning = false;
    }

    // 回合循环方法
	private void Process()
    {
		
    }




}
