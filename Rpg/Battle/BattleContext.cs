using Rpg.Core;

namespace Rpg.Battle;

/// <summary>
/// 戰鬥上下文，供 Action 與 Observer 使用（補充規格）。
/// </summary>
public class BattleContext
{
    public Troop PlayerTroop { get; set; }
    public Troop EnemyTroop { get; set; }
    public Role? Hero { get; set; }
    public Game.RpgGame? Game { get; set; }

    public BattleContext(Troop playerTroop, Troop enemyTroop, Role? hero)
    {
        PlayerTroop = playerTroop;
        EnemyTroop = enemyTroop;
        Hero = hero;
    }

    public IEnumerable<Role> GetAllAliveRoles()
    {
        foreach (var r in PlayerTroop.Allies.Where(r => r.IsAlive))
            yield return r;
        foreach (var r in EnemyTroop.Allies.Where(r => r.IsAlive))
            yield return r;
    }
}
