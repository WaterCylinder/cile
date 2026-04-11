using Godot;
using Godot.Collections;

public class GameModeDemo : GameMode
{
    /// <summary>
    /// 玩家选择的判断位置
    /// </summary>
    public Dictionary<Player, Vector2> readyPos = new();
    // 玩家选择准备地形的状态，0表示没选，1表示只选了小单位，2表示只选了大单位，3表示全选了
    public Dictionary<Player, int> selectReadyTerrainState = new(); 

    public override void Start()
    {
        GD.Print("游戏模式demo：游戏开始");
        Map map = game.Map;
        selectedUI = game.pui.selectedUI;
        //给所有的准备地形事件设置：当同位置的地形有一个单位时，禁用同位置的另一个同大小单位的地形设置。
        //设置按钮禁用检查，当前玩家已经设置了一个单位后，记录玩家设置单位的准备地形的位置，禁用该玩家在不同的位置设置单位。
        foreach(var item in map.readyTerrains)
        {
            Vector2 pos = item.Key;
            Array<Terrain> terrainList = item.Value;
            GD.Print($"游戏模式demo：{pos}准备地形事件设置");
            foreach(Terrain t in terrainList)
            {   
                int putBigIndex = t.GetEffectIndex("Big");
                int putSmallIndex = t.GetEffectIndex("Small");
                t.OnEffectInvoke += (t,e) =>
                {
                    Player player = game.CurrentPlayer;
                    if (!selectReadyTerrainState.ContainsKey(player))
                    {
                        selectReadyTerrainState.Add(player, 0);
                    }
                    //针对添加单位的效果
                    if(!e.CheckTag(EffectTag.AddUnit))return;
                    if(readyPos.ContainsKey(game.CurrentPlayer))
                    {
                        if(readyPos[game.CurrentPlayer] != pos)
                        {
                            GD.Print("游戏模式demo：准备地形设置单位失败，当前玩家已经在其他方向设置单位。（按理来说这里是不应该被执行到的，请检查代码。）");
                            return;
                        }
                    }
                    else
                    {
                        readyPos.Add(game.CurrentPlayer, pos);
                    }
                    //同方位的所有地形禁用同大小一个单位的效果
                    if(e == t.GetEffect(putBigIndex))
                    {
                        foreach(Terrain tt in terrainList)
                        {
                            GD.Print($"移除{pos}方位的大单位放置效果，{tt}");
                            tt.SetEffectEnable(putBigIndex,false);
                        }
                        //记录状态当前玩家已选择大单位
                        selectReadyTerrainState[player] |= 2;
                    }
                    else if(e == t.GetEffect(putSmallIndex))
                    {
                        foreach(Terrain tt in terrainList)
                        {
                            GD.Print($"移除{pos}方位的小单位放置效果，{tt.Name}");
                            tt.SetEffectEnable(putSmallIndex,false);
                        }
                        //记录状态当前玩家已选择小单位
                        selectReadyTerrainState[player] |= 1;
                    }
                };
                //一个玩家只能选一个方位的准备区块来准备单位
                t.BeforeEffctEnableCheck += r =>
                {
                    Player player = game.CurrentPlayer;
                    if (readyPos.ContainsKey(player))
                    {
                        if (readyPos[player] != pos)
                        {
                            GD.Print($"玩家已选择了{readyPos[player]}作为准备方位，不能选择{pos}");
                            return false;
                        }
                    }
                    return r;
                };
            }
        }

        //给除了准备地形以外的所有地形添加：第五回合之前禁用效果
        foreach(Terrain t in map.innerTerrains)
        {
            t.BeforeEffctEnableCheck += r =>
            {
                if (turnLogic.turnCount < 5)
                {
                    return false;
                }
                return r;
            };
        }
    
        roundCricle.nextTurnCheck += r =>
        {
            Player player = game.CurrentPlayer;
            if (selectReadyTerrainState.ContainsKey(player))
            {
                if (selectReadyTerrainState[player] != 3)
                {
                    GD.Print($"玩家{player}未选择准备地形, 当前状态{selectReadyTerrainState[player]}，不能进入下一回合");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        };
    }
    
    public override void TurnStart(Turn turn)
    {
        GD.Print($"游戏模式demo：回合{turn.turnCount}开始");
    }

}

