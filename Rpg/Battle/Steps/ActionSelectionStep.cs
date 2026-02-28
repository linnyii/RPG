using Rpg.Core;
using Rpg.Game;

namespace Rpg.Battle.Steps;

public class ActionSelectionStep : IBattleStep
{
    public bool CanExecute(TakeTurnContext context)
        => context.IsCompleted<PrintStatusStep>()
        && context.IsCompleted<ApplyEffectStep>();

    public IBattleStep? Execute(TakeTurnContext context)
    {
        var role = context.CurrentRole;

        if (role is AI ai)
        {
            var validActions = role.Actions.Where(a => role.Mp >= a.MpCost).ToList();
            context.SelectedAction = ai.SelectionStrategy.SelectAction(ai, validActions);
        }
        else
        {
            GameOutput.PrintActionChoice(role, role.Actions);
            while (true)
            {
                var input = context.ReadLine();
                if (int.TryParse(input?.Trim(), out var idx) && idx >= 0 && idx < role.Actions.Count)
                {
                    var a = role.Actions[idx];
                    if (role.Mp >= a.MpCost)
                    {
                        context.SelectedAction = a;
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

        context.MarkCompleted<ActionSelectionStep>();
        return new TargetSelectionStep();
    }
}
