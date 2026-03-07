using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
/*
标准信息类
*/

/// <summary>
/// 对局信息
/// </summary>
public class GameInfo
{
    /// <summary>
    /// 对局ID
    /// </summary>
    public string id;
    /// <summary>
    /// 对局用户信息
    /// </summary>
    public List<User> users = new List<User>();

    public GameInfo()
    {
        id = Guid.NewGuid().ToString();
        users = new List<User>();
    }
}