using Rpg.Action;
using Rpg.Core;
using Rpg.Game;

namespace Rpg.Battle;

public class BattleRunner(BattleContext context)
{
    //TODO: should push steps into phase
    private readonly ActionSelectionPhase _s1 = new();
    private readonly TargetSelectionPhase _s2 = new();
    private readonly ActionExecutionPhase _s3 = new();

    public BattleResult Run(Func<string> readLine)
    {
        while (true)
        {
            //TODO
            foreach (var role in GetAllActingOrder())
            {
                if (!role.IsAlive) continue;

                // P 步驟
                GameOutput.PrintTurnStart(role);

                switch (role.State)
                {
                    // E 步驟
                    case State.Petrochemical:
                        continue;
                    case State.Poisoned:
                    {
                        role.TakeDamage(30);
                        if (!role.IsAlive)
                        {
                            GameOutput.PrintDeath(role);
                            context.Game!.Notify(role);
                            var r = CheckBattleEnd();
                            if (r != BattleResult.Ongoing) return r;
                            continue;
                        }

                        break;
                    }
                    case State.Normal:
                    case State.Cheerup:
                        break;
                    default:
                        throw new UnsupportedStateException(role.State);
                }

                // S1
                var validActions = role.Actions.Where(a => role.Mp >= a.MpCost).ToList();
                IAction action;
                if (role is AI ai)
                {
                    action = ai.SelectionStrategy.SelectAction(ai, validActions);
                }
                else
                {
                    GameOutput.PrintActionChoice(role, role.Actions);
                    while (true)
                    {
                        var input = readLine();
                        if (int.TryParse(input?.Trim(), out var idx) && idx >= 0 && idx < role.Actions.Count)
                        {
                            var a = role.Actions[idx];
                            if (role.Mp >= a.MpCost)
                            {
                                action = a;
                                break;
                            }
                            GameOutput.PrintMpInsufficient();
                            GameOutput.PrintActionChoice(role, role.Actions);
                        }
                        else
                        {
                            GameOutput.PrintActionChoice(role, role.Actions);
                        }
                    }
                }

                // S2
                //TODO: rename
                var candidates = GetCandidates(role, action);
                //TODO: rename magic number 
                var targetCount = action.TargetCount == 0 ? candidates.Count : action.TargetCount;
                List<Role> targets;
                //TODO: ai2 is annoying
                if (role is AI ai2)
                {
                    targets = ai2.SelectionStrategy.SelectTargets(ai2, candidates, targetCount);
                }
                else
                {
                    if (targetCount > 0 && candidates.Count > targetCount)
                    {
                        if (targetCount == 1)
                            GameOutput.PrintTargetChoice(candidates);
                        else
                            GameOutput.PrintTargetChoiceMulti(candidates, targetCount);
                        var parts = readLine()!.Split(',', ' ').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();
                        targets = parts.Select(s => int.TryParse(s, out var i) ? i : -1).Where(i => i >= 0 && i < candidates.Count).Take(targetCount).Select(i => candidates[i]).ToList();
                    }
                    else
                    {
                        targets = candidates.Take(targetCount).ToList();
                    }
                }

                GameOutput.PrintSkillUse(role, targets, action);
                action.Execute(role, targets, context);

                var result = CheckBattleEnd();
                if (result != BattleResult.Ongoing) return result;
            }

            foreach (var role in context.GetAllAliveRoles())
                role.DecrementStateRounds();
        }
    }

    private IEnumerable<Role> GetAllActingOrder()
    {
        foreach (var r in context.PlayerTroop.Allies.Where(r => r.IsAlive))
            yield return r;
        foreach (var r in context.EnemyTroop.Allies.Where(r => r.IsAlive))
            yield return r;
    }

    private List<Role> GetCandidates(Role actor, IAction action)
    {
        var enemyTroop = actor.TroopId == context.PlayerTroop.Id ? context.EnemyTroop : context.PlayerTroop;
        var allyTroop = actor.TroopId == context.PlayerTroop.Id ? context.PlayerTroop : context.EnemyTroop;
        return action.TargetType switch
        {
            TargetType.Self => [actor],
            TargetType.Enemy => enemyTroop.Allies.Where(r => r.IsAlive).ToList(),
            TargetType.Ally => allyTroop.Allies.Where(r => r.IsAlive && r != actor).ToList(),
            TargetType.All => context.GetAllAliveRoles().ToList(),
            _ => throw new UnsupportedTargetTypeException(action.TargetType)
        };
    }

    private BattleResult CheckBattleEnd()
    {
        var hero = context.Hero;
        if (hero is { IsAlive: false })
            return BattleResult.PlayerLose;
        return context.EnemyTroop.Allies.Any(r => r.IsAlive) ? BattleResult.Ongoing : BattleResult.PlayerWin;
    }
}
