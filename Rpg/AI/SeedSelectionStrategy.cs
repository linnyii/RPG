using Rpg.Action;
using Rpg.AiStrategy;
using Rpg.Core;

namespace Rpg.AI;

public class SeedSelectionStrategy : IAiSelectionStrategy
{
    public IAction SelectAction(Core.AI ai, List<IAction> validActions)
    {
        while (true)
        {
            var idx = ai.Seed % validActions.Count;
            var action = validActions[idx];
            ai.Seed++;
            if (ai.Mp >= action.MpCost)
                return action;
        }
    }

    public List<Role> SelectTargets(Core.AI ai, List<Role> candidates, int count)
    {
        var selectTargets = new List<Role>();
        for (var i = 0; i < count; i++)
        {
            var idx = (ai.Seed + i) % candidates.Count;
            selectTargets.Add(candidates[idx]);
        }
        ai.Seed++;
        return selectTargets;
    }
}
