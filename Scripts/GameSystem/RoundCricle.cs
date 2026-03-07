using Godot;
using System;
using Utils.Coroutine;

public class RoundCricle
{
	public Coroutine coroutine = null;
	public Game game;

	/// <summary>
    /// 当前回合数
    /// </summary>
	public int round;

	/// <summary>
    /// 是否正在运行回合循环
    /// </summary>
	public bool isRunning;

	/// <summary>
    /// 是否等待中
    /// </summary>
	public bool waiting;

	/// <summary>
    /// 开始回合循环
    /// </summary>
	public void Start()
    {	
		game = GameManager.Instance.game;
		GD.Print($"开始回合循环，当前A值{game.A}");
        coroutine = CoroutineManager.Instance.StartCoroutine(Callable.From(Process), 0.5);
		round = 0;
		isRunning = true;
		waiting = true;
    }

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
		if (round == 1) //第一回合
        {
			
        }
        if (!waiting)
        {
            round += 1;
            GD.Print($"开始第{round}回合");
			waiting = true;
        }
    }




}
