using Rpg.Action;
using Rpg.Core;

namespace Rpg.Battle;

/// <summary>
/// S3：扣 MP、執行行動。
/// </summary>
public class ActionExecutionPhase : BattlePhase
{
    public void Execute(Role attacker, IAction action, List<Role> targets, BattleContext context)
    {
        action.Execute(attacker, targets, context);
    }
}
