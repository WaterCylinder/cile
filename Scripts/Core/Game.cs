using Godot;
using Godot.Collections;
using System;

public partial class Game : Node
{
	[Export] public Control UI;
    [Export] public Node2D scene;
    [Export] public string gameMode;
    [Export] public string seed;
    [ExportCategory("固定节点")]
    [Export] public PUI pui;
    [Export] public Control ActiveUI;
    [Export] public CardSelector cardSelector;
    [Export] public Node playerNode;
    [Export] public InGameMusic inGameMusic;
    [ExportCategory("A状态机")]
    [Export] public int A;
    [Export] public int ATemp;
    [Export] public int defaultStep = 100;
    [ExportCategory("实例")]
    [Export] public PackedScene innerMenuPackedScene;
    [Export] public PackedScene defaultMapPackedScene;
    [ExportCategory("场景实体")]
    [Export] public CameraController mainCamera;
    [Export] public Node2D unitNode;
    [Export] public Array<Terrain> selectedTerrains = new Array<Terrain>();
    [ExportCategory("可变节点{运行时加载}")]
    [Export] public Control innerMenu;
    [Export] public Node2D mapNode;
    public Array<Player> players = new Array<Player>();
    public GameInfo gameInfo = null;
    
    //索引
    public Map Map => mapNode as Map;
    public Player CurrentPlayer => roundCricle.turnLogic.turnPlayer;

    // 局内系统
    public RoundCricle roundCricle = new RoundCricle();
    public UnitSystem unitSystem = new UnitSystem();
    public RandomSystem randomSystem = null;
    public GameMode gameModeSystem;

    # region 信号
    /// <summary>
    /// 信号：选择地形
    /// </summary>
    [Signal]public delegate void OnTerrainSelectedEventHandler(Terrain terrain);
    /// <summary>
    /// 信号：取消选择地形
    /// </summary>
    [Signal]public delegate void OnTerrainSelectedCancleEventHandler();
    /// <summary>
    /// 信号：执行地形事件
    /// </summary>
    [Signal]public delegate void OnTerrainEffectEventHandler();
    /// <summary>
	/// 信号: 卡牌完成选择
	/// </summary>
	[Signal] public delegate void CardSelectedFinishSelectEventHandler(Array<CardData> result);

    # endregion

    // 是否可操作(对当前回合可操作性的判断，控制所有的回合操作)
    public bool CanOpera
    {
        get
        {
            try
            {
                return roundCricle.turnLogic.canOpera && roundCricle.turnLogic.isYouCanOpera;
            }
            catch(Exception e)
            {
                GD.Print(e.Message);
                return false;
            }
        }
    }

    public override void _EnterTree()
    {
        GameManager.Instance.game = this;   
    }

    # region A状态机
    public void Next(int step  = 0)
    {
        if(step == 0)
            step = defaultStep;
        A += step - A % step;
    }

    public void Goto(int target)
    {
        A = target;
    }

    public void GotoTemp(int target = -1)
    {
        if(target < 0)
        {
            A = ATemp;
        }
        else
        {
            ATemp = A;
            A = target;
        }
    }

    # endregion

	# region 初始化

    //加载对局信息
    public void LoadGameInfo()
    {   
        //初始化随机数系统
        randomSystem = new RandomSystem(seed != "" ? seed : new Random((int)DateTime.Now.Ticks).Next());
        //初始化游戏模式系统
        gameModeSystem = GameMode.CreatModeObject(gameMode, this);
        GD.Print("加载对局信息");
        if(gameInfo == null)
        {
            gameInfo = GameManager.Instance.gameInfoDefault;
        }
        gameModeSystem.OnLoadGameInfo(gameInfo);
    }
    
    //加载玩家信息
    public void LoadPlayer()
    {
        GD.Print("加载玩家信息");
        foreach(User user in gameInfo.users)
        {
            GD.Print($"加载用户{user.name},{user.id}");
            Player player = new Player();
            player.user = user;
            player.Init();
            player.Name = user.name + user.id;
            playerNode.AddChild(player);
            players.Add(player);
        }
    }

    //加载卡牌数据
    public void LoadCardData()
    {
        GD.Print("加载卡牌数据");
    }

    //游戏正式开始初始化，包括先后手敲定等
    public void GameInit()
    {
        GD.Print("回合前初始化");
        unitSystem.Init();
    }

    # endregion

    # region 功能方法

    public void OpenMenu()
    {
        innerMenu = innerMenuPackedScene.Instantiate<Control>();
		UI.AddChild(innerMenu);
    }

	public void CloseMenu()
    {
        innerMenu.QueueFree();
		innerMenu = null;
    }

    public void ChangePUIVisable(bool visable)
    {
        pui.Visible = visable;
    }

    /// <summary>
    /// 更换地图（更换地图对象，需要执行地图对象的加载方法来生成地图）
    /// </summary>
    /// <param name="packedScene">地图对象</param>
    public void ChangeMap(PackedScene packedScene = null)
    {
        if(packedScene == null)
        {
            packedScene = defaultMapPackedScene;
        }
        GD.Print($"更换地图{packedScene.ResourceName}");
        if(mapNode != null)
        {
            mapNode.QueueFree();
            mapNode = null;
        }
        mapNode = packedScene.Instantiate<Node2D>();
        mapNode.Name = "Map";
        mapNode.Position = Vector2.Zero;
        scene.AddChild(mapNode);
        scene.MoveChild(mapNode, 0);
    }

    public void OpenCardSelector()
    {
        cardSelector.Visible = true;
        cardSelector.Open();
    }

    # endregion

	public override void _Input(InputEvent @event)
    {
		if(@event is InputEventKey inputEventKey)
        {
			if(inputEventKey.Keycode == Key.Escape)
            {
                if (!inputEventKey.Pressed)
                {
                    if(innerMenu == null)
                    {
                        GD.Print("打开inner菜单");
                        OpenMenu();
                    }
                    else
                    {
                        GD.Print("关闭inner菜单");
                        CloseMenu();
                    }
                }
            }
		}
    }

    public override void _Process(double delta)
    {
        switch (A)
        {
            case 0: //A状态机初始化
                //加载地图
                ChangeMap();
                Next();
                break;
            case 100: //地图生成
                //初始化地图
                Map.Generate();
                Map.ReadyTerrainSet();
                Map.Init();
                Next();
                break;
            case 200: //加载
                //加载对局信息
                LoadGameInfo();
                //加载玩家信息
                LoadPlayer();
                //加载卡牌数据
                LoadCardData();
                //游戏初始化
                GameInit();
                Next();
                break;
            case 300:
                //开始动画
                Next();
                break;
            case 400:
                //开始回合循环
                roundCricle.Start();
                //从温和战斗音乐开始
                GameManager.Instance.game.inGameMusic.LoadMusic("fight");
                Next();
                break;
            case 500:
                //回合循环等待

            default:
                break;
        }
    }
}
