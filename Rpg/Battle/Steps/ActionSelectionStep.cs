using Rpg.Core;
using Rpg.Game;

namespace Rpg.Battle.Steps;

public class ActionSelectionStep : IBattleStep
{
    public bool CanExecute(TakeTurnContext context)
        => context.IsCompleted<PrintStatusStep>()
        && context.IsCompleted<ApplyEffectStep>();

    public IBattleStep Execute(TakeTurnContext context)
    {
        var role = context.CurrentRole;

        switch (role)
        {
            case Core.AI ai:
                var validActions = role.Actions.Where(a => role.Mp >= a.MpCost).ToList();
                context.SelectAction(ai.SelectionStrategy.SelectAction(ai, validActions));
                break;

            case Slime:
                context.SelectAction(role.Actions[0]);
                break;

            case Hero:
                GameOutput.PrintActionChoice(role, role.Actions);
                HandleHeroActionSelection(context, role);
                break;
        }

        context.MarkCompleted<ActionSelectionStep>();
        return new TargetSelectionStep();
    }

    private static void HandleHeroActionSelection(TakeTurnContext context, Role role)
    {
        while (true)
        {
            if (IsValidActionSelection(role, out var selectedAction))
            {
                var action = role.Actions[selectedAction];
                if (role.Mp >= action.MpCost)
                {
                    context.SelectAction(action);
                    break;
                }
                GameOutput.PrintMpIsSufficient();
                GameOutput.PrintActionChoice(role, role.Actions);
            }
            else
            {
                GameOutput.PrintActionChoice(role, role.Actions);
            }
        }
    }

    private static bool IsValidActionSelection(Role role, out int idx)
    {
        return int.TryParse(Console.ReadLine()?.Trim(), out idx) && idx >= 0 && idx < role.Actions.Count;
    }
}
