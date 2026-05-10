using Godot;
using System;
using System.Reflection;

public class GameMode
{
    public static GameMode CreatModeObject(string modeName, Game game)
    {
        GameMode gameMode = null;
        switch (modeName)
        {
            case "Demo":
                gameMode = new GameModeDemo();
                break;
            default:
                GD.Print("{modeName}未设置, 请在GameMode脚本里添加映射, 默认返回Demo模式对象");
                gameMode = new GameModeDemo();
                break;
        }
        if(gameMode != null)
        {
            gameMode.game = game;
            gameMode.roundCricle = game.roundCricle;
            gameMode.turnLogic = game.roundCricle.turnLogic;
            gameMode.Init();
        }
        gameMode.A = 0;
        return gameMode;
    }

    //初始化时设置的变量
    public Game game;
    public RoundCricle roundCricle;
    public Turn turnLogic;
    
    //需要时设置的变量
    public SelectedUI selectedUI;

    public int A;
    public int temp;

    public void Next()
    {
        A += 100;
        A -= A % 100;
    }

    public void Next(int num)
    {
        A += num;
    }

    public void CheckTemp(int temp)
    {
        if(this.temp > 0)
        {
            A = this.temp;
            this.temp = -1;
        }
        else
        {
            this.temp = A;
            A = temp;
        }
    }

    
    /// <summary>
    /// 游戏玩法初始化
    /// </summary>
    public virtual void Init(){}
    /// <summary>
    /// 加载游戏信息时
    /// </summary>
    /// <param name="gameInfo"></param>
    public virtual void OnLoadGameInfo(GameInfo gameInfo){} //加载游戏信息时调用
    /// <summary>
    /// 开始游戏时
    /// </summary>
    public virtual void Start(){}
    /// <summary>
    /// 游戏进程
    /// </summary>
    /// <param name="delta"></param>
    public virtual void Update(float delta){}
    /// <summary>
    /// 游戏结束
    /// </summary>
    public virtual void End(){}
    /// <summary>
    /// 第一回合开始时
    /// </summary>
    public virtual void FirstTurnStart(){}
    /// <summary>
    /// 第二轮开始时，默认第一轮是准备阶段
    /// </summary>
    public virtual void SecondRoundStart(){}
    /// <summary>
    /// 回合开始时
    /// </summary>
    /// <param name="turn"></param>
    public virtual void TurnStart(Turn turn){}
    /// <summary>
    /// 回合结束时
    /// </summary>
    /// <param name="turn"></param>
    public virtual void TurnEnd(Turn turn){}
    /// <summary>
    /// 轮次开始时
    /// </summary>
    public virtual void RoundStart(){}
    /// <summary>
    /// 轮次结束时
    /// </summary>
    public virtual void RoundEnd(){}
}

