using Godot;
using System;

namespace DataFormat
{
    public enum GameGlobalState
    {
		/// <summary>
        /// 游戏状态-主菜单
        /// </summary>
        InMainMenu,
		/// <summary>
        /// 游戏状态-游戏进行中
        /// </summary>
		InGame,
		/// <summary>
        /// 游戏状态-游戏暂停
        /// </summary>
		Pause,
    }
}