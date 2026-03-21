using System.Linq;
using System.Reflection;
using Godot;
using Godot.Collections;

[GlobalClass] // 标记为全局类，可在整个项目中访问
public partial class Behavior : Resource // Behavior类继承自Resource，是资源类的一部分
{
    // 使用静态字典存储方法信息，键为方法名，值为方法信息
    public static System.Collections.Generic.Dictionary<string, MethodInfo> MethodList = new();
    // 行为名称
    [Export]public string behaviorName = "";
    // 参数
    [Export]public Dictionary<string, Variant> args = new();
    
    /// <summary>
    /// 执行指定名称的行为方法，并返回结果
    /// </summary>
    /// <typeparam name="T">返回值的类型</typeparam>
    /// <returns>方法执行后的结果，如果执行失败则返回默认值</returns>
    public virtual T Run<T>()
    {   
        // 用于存储要执行的方法信息
        MethodInfo method;
        // 首先检查方法是否已缓存
        if (MethodList.ContainsKey(behaviorName))
        {
            // 从缓存中获取方法
            method = MethodList[behaviorName];
        }
        else
        {
            // 如果未缓存，则从当前类型中获取方法并添加到缓存
            method = GetType().GetMethod(behaviorName);
            MethodList.Add(behaviorName, method);
        }
        // 如果找到方法
        if (method != null)
        {
            try
            {
                // 执行方法并返回结果
                return (T)method.Invoke(this, null);
            }
            catch (System.Exception e)
            {
                // 捕获并打印执行异常信息
                GD.PrintErr($"{this.GetType().Name}@{method.Name}执行异常{e.InnerException}");
                return default;
            }
            
        }
        // 如果未找到方法，打印错误信息并返回默认值
        GD.PrintErr($"{this.GetType().Name} 未找到行为方法：" + behaviorName);
        return default;
    }
    /// <summary>
    /// 执行行为逻辑
    /// </summary>
    public void Run()
    {
        Run<object>();
    }

    #region 全局行为方法
    public string DebugLog()
    {   
        if (!args.ContainsKey("info"))
        {
            GD.PrintErr("DebugLog行为缺少参数info");
            return "";
        }
        string info = args["info"].AsString();
        GD.Print(info);
        return info;
    }
    #endregion
}