using Rpg.Action;
using Rpg.Core;

namespace Rpg.Battle;

/// <summary>
/// S2：選擇目標。若 n≤m 或 m=0 則自動決定。
/// </summary>
public class TargetSelectionPhase : BattlePhase
{
    public List<Role> GetCandidates(Role actor, IAction action, BattleContext context)
    {
        var enemyTroop = actor.TroopId == context.PlayerTroop.Id ? context.EnemyTroop : context.PlayerTroop;
        var allyTroop = actor.TroopId == context.PlayerTroop.Id ? context.PlayerTroop : context.EnemyTroop;

        return action.TargetType switch
        {
            TargetType.Self => new List<Role> { actor },
            TargetType.Enemy => enemyTroop.Allies.Where(r => r.IsAlive).ToList(),
            TargetType.Ally => allyTroop.Allies.Where(r => r.IsAlive && r != actor).ToList(),
            TargetType.All => context.GetAllAliveRoles().ToList(),
            _ => new List<Role>()
        };
    }

    public List<Role> SelectTargets(Role actor, IAction action, BattleContext context, Func<string> readLine)
    {
        var candidates = GetCandidates(actor, action, context);
        var m = action.TargetCount;

        if (m == 0 || candidates.Count <= m)
            return candidates.Take(m == 0 ? int.MaxValue : m).ToList();

        if (actor is Core.AI ai)
        {
            return ai.SelectionStrategy.SelectTargets(ai, candidates, m);
        }

        var input = readLine();
        var indices = input.Split(',', ' ').Select(s => int.TryParse(s.Trim(), out var i) ? i : -1).Where(i => i >= 0).ToList();
        return indices.Take(m).Select(i => candidates[i]).ToList();
    }
}
