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

4. 卡牌系统
	* 卡牌效果触发框架
		- 条件 Condition
			条件包含一个行为，执行条件时执行该行为并返回行为结果作为条件结果，条件结果为布尔变量
		- 行为 Behavior
			行为包含行为名称和参数字典，根据行为名称反射执行相应方法。
		- 效果 Effect
			效果包含条件和行为，执行效果时首先执行条件，如果条件结果为真则执行行为。
	
		卡牌数据里定义卡牌效果
	* 卡牌管理系统
