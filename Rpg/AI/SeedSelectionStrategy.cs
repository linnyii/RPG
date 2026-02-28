using Rpg.Action;
using Rpg.AiStrategy;
using Rpg.Core;

namespace Rpg.AI;

public class SeedSelectionStrategy : IAiSelectionStrategy
{
    public IAction SelectAction(Role ai, List<IAction> validActions)
    {
        //TODO: why need to (AI)??
        var aiRole = (Core.AI)ai;
        while (true)
        {
            var idx = aiRole.Seed % validActions.Count;
            var action = validActions[idx];
            aiRole.Seed++;
            if (aiRole.Mp >= action.MpCost)
                return action;
        }
    }

    public List<Role> SelectTargets(Role ai, List<Role> candidates, int count)
    {
        var aiRole = (Core.AI)ai;
        var n = candidates.Count;
        var result = new List<Role>();
        for (var i = 0; i < count; i++)
        {
            var idx = (aiRole.Seed + i) % n;
            result.Add(candidates[idx]);
        }
        aiRole.Seed++;
        return result;
    }
}
