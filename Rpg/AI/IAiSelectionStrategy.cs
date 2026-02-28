using Rpg.Action;
using Rpg.Core;

namespace Rpg.AiStrategy;

/// <summary>
/// 對應 PDF 的 IAiSelectionStrategy。
/// </summary>
public interface IAiSelectionStrategy
{
    IAction SelectAction(Core.AI ai, List<IAction> validActions);
    List<Role> SelectTargets(Core.AI ai, List<Role> candidates, int count);
}
