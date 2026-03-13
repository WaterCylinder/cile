using Godot;
using DataFormat;
using System;
using Godot.Collections;

public static class Global
{
	public static string welcome = "HelloWorld";
	public static GameGlobalState gameState = GameGlobalState.InMainMenu;

	public static string AssetPath = "res://";
	public static string ResourcePath = "res://Resources";
	public static string ScenePath = "res://Scenes";
	public static string UserPath = "user://";

	public static string DefaultConfigPath = "res://default_config.json";
	public static string ConfigPath = "user://config.json";

	public static Variant config = new();
	public static Variant Config
	{
		get
		{
			if (config.VariantType == Variant.Type.Nil)
			{
				LoadConfig();
			}
			return config;
		}
	}

	/// <summary>
    /// 加载配置
    /// </summary>
	public static void LoadConfig()
    {	
		//创建用户目录
		if(!DirAccess.DirExistsAbsolute(UserPath))
		{
			GD.Print($"用户目录不存在，正在创建...{ProjectSettings.GlobalizePath(UserPath)}");
			DirAccess.MakeDirAbsolute(UserPath);
		}

        try
        {
            if (!FileAccess.FileExists(ConfigPath))
            {
                using FileAccess sourceFile = FileAccess.Open(DefaultConfigPath, FileAccess.ModeFlags.Read);
				using FileAccess targetFile = FileAccess.Open(ConfigPath, FileAccess.ModeFlags.Write);
				string text = sourceFile.GetAsText();
				targetFile.StoreString(text);
				sourceFile.Close();
				targetFile.Close();
				GD.Print($"配置文件创建成功: {ProjectSettings.GlobalizePath(ConfigPath)}");
				config = Json.ParseString(text);
            }
            else
            {
                using FileAccess file = FileAccess.Open(ConfigPath, FileAccess.ModeFlags.Read);
				config = Json.ParseString(file.GetAsText());
				GD.Print($"配置文件加载成功: {ProjectSettings.GlobalizePath(ConfigPath)}");
            }
        }
		catch (Exception e)
		{
			GD.PrintErr(e);
			var error = DirAccess.CopyAbsolute(DefaultConfigPath, ConfigPath);
			using FileAccess file = FileAccess.Open(DefaultConfigPath, FileAccess.ModeFlags.Read);
			config = Json.ParseString(file.GetAsText());
			GD.Print($"默认配置文件加载成功: {ProjectSettings.GlobalizePath(DefaultConfigPath)}");
			file.Close();
		}
    }

	/// <summary>
    /// 保存配置
    /// </summary>
	public static void SaveConfig()
    {
		using FileAccess file = FileAccess.Open(ConfigPath, FileAccess.ModeFlags.Write);
		file.StoreString(Json.Stringify(config));
		GD.Print($"配置文件保存成功: {ConfigPath}");
    }
	
}
