using System.Linq;
using System.Reflection;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Behavior : Resource
{
    public static System.Collections.Generic.Dictionary<string, MethodInfo> MethodList = new();
    [Export]public string behaviorName = "";
    [Export]public Dictionary<string, Variant> args = new();
    public virtual T Run<T>()
    {   
        MethodInfo method;
        if (MethodList.ContainsKey(behaviorName))
        {
            method = MethodList[behaviorName];
        }
        else
        {
            method = GetType().GetMethod(behaviorName);
        }
        if (method != null)
        {
            try
            {
                return (T)method.Invoke(this, null);
            }
            catch (System.Exception e)
            {
                GD.PrintErr($"{this.GetType().Name}@{method.Name}执行异常{e.InnerException}");
                return default;
            }
            
        }
        GD.PrintErr($"{this.GetType().Name} 未找到行为方法：" + behaviorName);
        return default;
    }
    public void Run()
    {
        Run<object>();
    }

    #region 全局行为方法
    public string DebugLog()
    {   
        string info = args["info"].AsString();
        GD.Print(info);
        return info;
    }
    #endregion
}