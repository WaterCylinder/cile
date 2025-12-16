using Godot;
using System;

public partial class CardManager : Node
{
	private static CardManager instance;
	public static CardManager Instance
    {
        get
        {
            if(instance == null)
            {
                try
				{
					GD.Print("Core：正在初始化卡牌管理器");
					instance = new CardManager();
					instance.Name = "CardManager";
					instance.ProcessMode = ProcessModeEnum.Always;
					GameManager.Instance.AddChild(instance);
				}
				catch(Exception e)
				{
					GD.PrintErr($"Core：初始化卡牌管理器失败！{e}");
				}
            }
			return instance;
        }
    }

	public void Init()
    {
        
    }

}
