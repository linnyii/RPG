using Rpg.Game;

namespace Rpg.Battle.Steps;

public class ActionExecutionStep : IBattleStep
{
    public bool CanExecute(TakeTurnContext context)
        => context.IsCompleted<TargetSelectionStep>();

    public IBattleStep? Execute(TakeTurnContext context)
    {
        var role = context.CurrentRole;
        var action = context.SelectedAction!;
        var targets = context.SelectedTargets!;

        GameOutput.PrintSkillUse(role, targets, action);
        action.Execute(role, targets, context.BattleContext);

        context.MarkCompleted<ActionExecutionStep>();
        return null;
    }
}
