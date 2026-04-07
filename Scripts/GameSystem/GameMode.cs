using Godot;
using System;
using System.Reflection;

public class GameMode
{
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

    public static GameMode CreatModeObject(string modeName, Game game)
    {
        GameMode gameMode = null;
        switch (modeName)
        {
            case "Demo":
                gameMode = new GameModeDemo();
                break;
            default:
                GD.Print("{modeName}未设置, 请在GameMode子类脚本里添加映射, 默认返回Demo模式对象");
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

    public virtual void Init(){}
    public virtual void Start(){}
    public virtual void Update(float delta){}
    public virtual void End(){}
    public virtual void TurnStart(Turn turn){}
    public virtual void TurnEnd(Turn turn){}
    public virtual void RoundStart(){}
    public virtual void RoundEnd(){}
}

