using Rpg.Game;

namespace Rpg.Battle.Steps;

public class PrintStatusStep : IBattleStep
{
    public bool CanExecute(TakeTurnContext context) => true;

    public IBattleStep Execute(TakeTurnContext context)
    {
        GameOutput.PrintTurnStart(context.CurrentRole);
        context.MarkCompleted<PrintStatusStep>();
        return new ApplyEffectStep();
    }
}
