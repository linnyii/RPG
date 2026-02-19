using Rpg.Core;

namespace Rpg.Battle;

/// <summary>
/// 戰鬥上下文，供 Action 與 Observer 使用（補充規格）。
/// </summary>
public class BattleContext(Troop playerTroop, Troop enemyTroop, Role? hero)
{
    public Troop PlayerTroop { get; } = playerTroop;
    public Troop EnemyTroop { get; } = enemyTroop;
    public Role? Hero { get; } = hero;
    public Game.RpgGame? Game { get; init; }

    public IEnumerable<Role> GetAllAliveRoles()
    {
        foreach (var r in PlayerTroop.Allies.Where(r => r.IsAlive))
            yield return r;
        foreach (var r in EnemyTroop.Allies.Where(r => r.IsAlive))
            yield return r;
    }
}
