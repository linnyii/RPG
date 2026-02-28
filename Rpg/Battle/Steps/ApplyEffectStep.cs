using Rpg.Core;
using Rpg.Game;

namespace Rpg.Battle.Steps;

public class ApplyEffectStep : IBattleStep
{
    public bool CanExecute(TakeTurnContext context)
        => context.IsCompleted<PrintStatusStep>();

    public IBattleStep? Execute(TakeTurnContext context)
    {
        var role = context.CurrentRole;

        switch (role.State)
        {
            case State.Petrochemical:
                context.MarkCompleted<ApplyEffectStep>();
                return null;

            case State.Poisoned:
                role.TakeDamage(30);
                if (!role.IsAlive)
                {
                    GameOutput.PrintDeath(role);
                    context.BattleContext.Game!.Notify(role);
                    context.MarkCompleted<ApplyEffectStep>();
                    return null;
                }
                break;

            case State.Normal:
            case State.Cheerup:
                break;

            default:
                throw new UnsupportedStateException(role.State);
        }

        context.MarkCompleted<ApplyEffectStep>();
        return new ActionSelectionStep();
    }
}
