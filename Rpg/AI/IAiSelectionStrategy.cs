using Rpg.Action;
using Rpg.Core;

namespace Rpg.AiStrategy;

public interface IAiSelectionStrategy
{
    IAction SelectAction(Core.AI ai, List<IAction> validActions);
    List<Role> SelectTargets(Core.AI ai, List<Role> candidates, int count);
}
