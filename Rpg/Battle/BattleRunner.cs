using Rpg.Core;
using Rpg.Battle.Steps;

namespace Rpg.Battle;

public class BattleRunner(BattleContext context)
{
    public BattleResult Run(Func<string> readLine)
    {
        while (true)
        {
            foreach (var role in GetAllActingOrder())
            {
                if (!role.IsAlive) continue;

                var takeTurnContext = new TakeTurnContext
                {
                    CurrentRole = role,
                    BattleContext = context,
                    ReadLine = readLine
                };
                takeTurnContext.Run(new PrintStatusStep());

                if (takeTurnContext.EarlyResult is { } earlyResult && earlyResult != BattleResult.Ongoing)
                    return earlyResult;

                var result = CheckBattleEnd();
                if (result != BattleResult.Ongoing) return result;
            }

            foreach (var r in context.GetAllAliveRoles())
                r.DecrementStateRounds();
        }
    }

    private IEnumerable<Role> GetAllActingOrder()
    {
        foreach (var r in context.PlayerTroop.Allies.Where(r => r.IsAlive))
            yield return r;
        foreach (var r in context.EnemyTroop.Allies.Where(r => r.IsAlive))
            yield return r;
    }

    private BattleResult CheckBattleEnd()
    {
        var hero = context.Hero;
        if (hero is { IsAlive: false })
            return BattleResult.PlayerLose;
        return context.EnemyTroop.Allies.Any(r => r.IsAlive) ? BattleResult.Ongoing : BattleResult.PlayerWin;
    }
}
