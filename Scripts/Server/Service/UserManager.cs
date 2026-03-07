using System;

public class UserManager
{
    private static UserManager instance = null;
    public static UserManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UserManager();
            }
            return instance;
        }
    }

    /// <summary>
    /// 创建机器人账户
    /// </summary>
    /// <returns></returns>
    public User CreateBotUser()
    {
        User user = new User("BOT", "1111");
        return user;
    }

}