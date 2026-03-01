using Rpg.Core;
using Rpg.Battle.Steps;

namespace Rpg.Battle;

public class BattleExecutor(BattleContext context)
{
    public BattleResult Run()
    {
        while (true)
        {
            foreach (var role in context.GetAllAliveRoles())
            {
                if (!role.IsAlive) continue;

                var takeTurnContext = new TakeTurnContext
                {
                    CurrentRole = role,
                    BattleContext = context
                };
                takeTurnContext.Execute(new PrintStatusStep());

                var result = CheckIsBattleEnd();
                if (result != BattleResult.Ongoing) return result;
            }

            foreach (var role in context.GetAllAliveRoles())
                role.DecrementStateRounds();
        }
    }

    private BattleResult CheckIsBattleEnd()
    {
        var hero = context.Hero;
        if (hero is { IsAlive: false })
            return BattleResult.PlayerLose;
        return context.EnemyTroop.Allies.Any(r => r.IsAlive) ? BattleResult.Ongoing : BattleResult.PlayerWin;
    }
}
