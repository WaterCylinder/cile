using Godot;
using Godot.Collections;
using System;

public partial class InputManager : Node
{
	private static InputManager instance;
	public static InputManager Instance
    {
        get
        {
            if(instance == null)
            {
                try
				{
					GD.Print("Core：正在初始化输入管理器");
					instance = new InputManager();
					instance.Name = "InputManager";
					instance.ProcessMode = ProcessModeEnum.Always;
					GameManager.Instance.AddChild(instance);
				}
				catch(Exception e)
				{
					GD.PrintErr($"Core：初始化输入管理器失败！{e}");
				}
            }
			return instance;
        }
    }

	[Export] public Dictionary<string, bool> inputKeyDict = new();
    [Export] public Dictionary<string, bool> inputKeyDownDict = new();

	[Signal] public delegate void KeyDownEventHandler(string Keycode);
	[Signal] public delegate void KeyUpEventHandler(string Keycode);

	public void Init()
    {
        
    }

	public bool KeyPressed(string keyCode)
    {
        if (inputKeyDict.ContainsKey(keyCode))
        {
            return inputKeyDict[keyCode];
        }
        else
        {
            return false;
        }
    }

	public bool KeyPressed(Key key)
    {
		string keyCode = OS.GetKeycodeString(key);
        return KeyPressed(keyCode);
    }

	public override void _Input(InputEvent @event)
    {
        if(@event is InputEventKey inputEventKey)
        {
            string keyCode = OS.GetKeycodeString(inputEventKey.Keycode);
            if (inputEventKey.Pressed)
            {
				EmitSignal(SignalName.KeyDown, keyCode);
                if (!inputKeyDict.ContainsKey(keyCode))
                {
                    inputKeyDict.Add(keyCode, false);
                }
				if (!inputKeyDict[keyCode])
                {
                    EmitSignal(SignalName.KeyDown, keyCode);
                }
				inputKeyDict[keyCode] = true;
            }
            else
            {
                EmitSignal(SignalName.KeyUp, keyCode);
            }
        }
    }
}
