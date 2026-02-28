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

        if (role is Core.AI ai)
        {
            var validActions = role.Actions.Where(a => role.Mp >= a.MpCost).ToList();
            context.SelectAction(ai.SelectionStrategy.SelectAction(ai, validActions));
        }
        else
        {
            GameOutput.PrintActionChoice(role, role.Actions);
            while (true)
            {
                var input = Console.ReadLine();
                if (IsValidActionSelection(input, role, out var selectedAction))
                {
                    var a = role.Actions[selectedAction];
                    if (role.Mp >= a.MpCost)
                    {
                        context.SelectAction(a);
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

        context.MarkCompleted<ActionSelectionStep>();
        return new TargetSelectionStep();
    }

    private static bool IsValidActionSelection(string? input, Role role, out int idx)
    {
        return int.TryParse(input?.Trim(), out idx) && idx >= 0 && idx < role.Actions.Count;
    }
}
