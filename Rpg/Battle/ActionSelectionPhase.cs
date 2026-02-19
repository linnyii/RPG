using Rpg.Action;
using Rpg.Core;

namespace Rpg.Battle;

/// <summary>
/// S1：選擇行動。檢查 MP，不合法則重選。
/// </summary>
public class ActionSelectionPhase : BattlePhase
{
    public List<IAction> GetValidActions(Role role)
    {
        return role.Actions.Where(a => role.Mp >= a.MpCost).ToList();
    }

    public IAction SelectAction(Role role, BattleContext context, Func<string> readLine)
    {
        var valid = GetValidActions(role);
        while (true)
        {
            var input = readLine();
            if (int.TryParse(input, out var idx) && idx >= 0 && idx < role.Actions.Count)
            {
                var action = role.Actions[idx];
                if (role.Mp >= action.MpCost)
                    return action;
                context.Game?.OnMpInsufficient?.Invoke();
            }
        }
    }
}
