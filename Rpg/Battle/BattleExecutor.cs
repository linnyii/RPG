using Rpg.Core;
using Rpg.Battle.Steps;

namespace Rpg.Battle;

public class BattleExecutor(BattleContext context)
{
    public BattleResult Run()
    {
        while (true)
        {
            foreach (var role in GetAliveRoles())
            {
                if (!role.IsAlive) continue;

                var takeTurnContext = new TakeTurnContext
                {
                    CurrentRole = role,
                    BattleContext = context
                };
                takeTurnContext.Run(new PrintStatusStep());

                var result = CheckIsBattleEnd();
                if (result != BattleResult.Ongoing) return result;
            }

            foreach (var role in context.GetAllAliveRoles())
                role.DecrementStateRounds();
        }
    }

    private IEnumerable<Role> GetAliveRoles()
    {
        foreach (var role in context.PlayerTroop.Allies.Where(r => r.IsAlive))
            yield return role;
        foreach (var role in context.EnemyTroop.Allies.Where(r => r.IsAlive))
            yield return role;
    }

    private BattleResult CheckIsBattleEnd()
    {
        var hero = context.Hero;
        if (hero is { IsAlive: false })
            return BattleResult.PlayerLose;
        return context.EnemyTroop.Allies.Any(r => r.IsAlive) ? BattleResult.Ongoing : BattleResult.PlayerWin;
    }
}
