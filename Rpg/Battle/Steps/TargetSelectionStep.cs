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
        var candidates = GetCandidatesByTargetType(role, action, context.BattleContext);
        var targetCount = action.TargetCount == 0 ? candidates.Count : action.TargetCount;

        switch (role)
        {
            case Core.AI ai:
                context.SelectTargets(ai.SelectionStrategy.SelectTargets(ai, candidates, targetCount));
                break;

            case Slime:
                var randomIdx = Random.Shared.Next(candidates.Count);
                context.SelectTargets([candidates[randomIdx]]);
                break;

            case Hero:
                HandleHeroTargetSelection(context, targetCount, candidates);
                break;
        }

        context.MarkCompleted<TargetSelectionStep>();
        return new ActionExecutionStep();
    }

    private static void HandleHeroTargetSelection(TakeTurnContext context, int targetCount, List<Role> candidates)
    {
        if (targetCount > 0 && candidates.Count > targetCount)
        {
            if (targetCount == 1)
                GameOutput.PrintTargetChoice(candidates);
            else
                GameOutput.PrintTargetChoiceMulti(candidates, targetCount);

            var parts = (Console.ReadLine() ?? "")
                .Split(',', ' ')
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();

            context.SelectTargets(parts
                .Select(s => int.TryParse(s, out var i) ? i : -1)
                .Where(i => i >= 0 && i < candidates.Count)
                .Take(targetCount)
                .Select(i => candidates[i])
                .ToList());
        }
        else
        {
            context.SelectTargets(candidates.Take(targetCount).ToList());
        }
    }

    private static List<Role> GetCandidatesByTargetType(Role actor, IAction action, BattleContext battleContext)
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
