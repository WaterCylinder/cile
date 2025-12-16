## 实体类
1. 玩家 Player
	* 属性：
		- id PlayerID
		- 玩家状态 PlayerState
		- 资源点 SourcePoint
		- 血量 Hp
		- 死亡数 Death
		- 手卡 HandCards
2. 卡牌
	* 属性：
		- 卡牌类型 CardType
		- 卡牌使用时事件 OnUse
	* 信号
		- 卡牌使用 CardUse
3. 地形
	* 属性
		- 地形类型 TerrainType

## 系统
1. 流程控制系统
	* GameManger和Global
	* A变量系统
2. 检定系统
	* 骰子系统
	* 卡牌管理系统
3. 资源管理系统 AssetManager

	用于资源的加载和缓存，可加载的资源有物体资源（tscn）、精灵图像资源（png）、数据资源（tres）。
