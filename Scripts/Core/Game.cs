using Godot;
using System;

public partial class Game : Node
{
	[Export] public Control UI;
    [Export] public Node2D scene;
    [ExportCategory("固定节点")]
    [Export] public Control PUI;
    [ExportCategory("A状态机")]
    [Export] public int A;
    [Export] public int ATemp;
    [Export] public int defaultStep = 100;
    [ExportCategory("实例")]
    [Export] public PackedScene innerMenuPackedScene;
    [Export] public PackedScene defaultMapPackedScene;
    [ExportCategory("可变节点{运行时加载}")]
    [Export] public Control innerMenu;
    [Export] public Node2D mapNode;
    public Map Map => mapNode as Map;

    // 回合循环实例
    public RoundCricle roundCricle;

    public override void _Ready()
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
        PUI.Visible = visable;
    }

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
    }
    
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
                        OpenMenu();
                    }
                    else
                    {
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
                //初始化回合循环系统
                roundCricle = new RoundCricle();
                roundCricle.Start();
                //加载地图
                ChangeMap();
                Next();
                break;
            case 100: //地图生成
                //初始化地图
                Map.Generate();
                Map.Init();
                Next();
                break;
            case 200: //等待
                break;
            default:
                break;
        }
    }

}
