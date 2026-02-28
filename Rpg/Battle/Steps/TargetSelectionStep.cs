using Rpg.Action;
using Rpg.Core;
using Rpg.Game;

namespace Rpg.Battle.Steps;

public class TargetSelectionStep : IBattleStep
{
    public bool CanExecute(TakeTurnContext context)
        => context.IsCompleted<ActionSelectionStep>();

    public IBattleStep Execute(TakeTurnContext context)
    {
        var role = context.CurrentRole;
        var action = context.SelectedAction!;
        var candidates = GetCandidates(role, action, context.BattleContext);
        var targetCount = action.TargetCount == 0 ? candidates.Count : action.TargetCount;

        if (role is AI ai)
        {
            context.SelectedTargets = ai.SelectionStrategy.SelectTargets(ai, candidates, targetCount);
        }
        else
        {
            if (targetCount > 0 && candidates.Count > targetCount)
            {
                if (targetCount == 1)
                    GameOutput.PrintTargetChoice(candidates);
                else
                    GameOutput.PrintTargetChoiceMulti(candidates, targetCount);

                var parts = context.ReadLine()
                    .Split(',', ' ')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToList();

                context.SelectedTargets = parts
                    .Select(s => int.TryParse(s, out var i) ? i : -1)
                    .Where(i => i >= 0 && i < candidates.Count)
                    .Take(targetCount)
                    .Select(i => candidates[i])
                    .ToList();
            }
            else
            {
                context.SelectedTargets = candidates.Take(targetCount).ToList();
            }
        }

        context.MarkCompleted<TargetSelectionStep>();
        return new ActionExecutionStep();
    }

    private static List<Role> GetCandidates(Role actor, IAction action, BattleContext battleContext)
    {
        var enemyTroop = actor.TroopId == battleContext.PlayerTroop.Id
            ? battleContext.EnemyTroop
            : battleContext.PlayerTroop;
        var allyTroop = actor.TroopId == battleContext.PlayerTroop.Id
            ? battleContext.PlayerTroop
            : battleContext.EnemyTroop;

        return action.TargetType switch
        {
            TargetType.Self => [actor],
            TargetType.Enemy => enemyTroop.Allies.Where(r => r.IsAlive).ToList(),
            TargetType.Ally => allyTroop.Allies.Where(r => r.IsAlive && r != actor).ToList(),
            TargetType.All => battleContext.GetAllAliveRoles().ToList(),
            _ => throw new UnsupportedTargetTypeException(action.TargetType)
        };
    }
}
