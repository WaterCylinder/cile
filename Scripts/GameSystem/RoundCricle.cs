using Godot;
using System;
using Utils.Coroutine;

public class RoundCricle
{
	public Coroutine coroutine;
	public Game game;

	/// <summary>
    /// 当前回合数
    /// </summary>
	public int round;

	public void Start()
    {	
		game = GameManager.Instance.game;
		GD.Print($"开始回合循环，当前A值{game.A}");
        coroutine = CoroutineManager.Instance.StartCoroutine(Callable.From(Process), 0.5);

		round = 0;
    }
	public void Process()
    {
        
    }


}
