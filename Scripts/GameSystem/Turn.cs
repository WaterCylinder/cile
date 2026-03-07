/*
    回合逻辑
*/

using Godot;
using System;

public partial class Turn
{
    public TurnPanel turnPanel;
    public RoundCricle roundCricle;

    private int turn
    {
        get
        {
            return roundCricle.turn;
        }
    }

    /// <summary>
    /// 当前轮玩家
    /// </summary>
    public Player turnPlayer;

    /// <summary>
    /// 是否可操作
    /// </summary>
    public bool canOpera;

    /// <summary>
    /// 你能操作吗？（当前客户端是否为当前轮玩家）
    /// </summary>
    public bool isYouCanOpera;

    //进入回合的方法
    public void EnterTurn()
    {   
        turnPanel = GameManager.Instance.game.ActiveUI.GetNode<TurnPanel>("TurnPanel");
        turnPanel.SetTurnCount(turn);
        turnPanel.PlayAnimation();
    }

}