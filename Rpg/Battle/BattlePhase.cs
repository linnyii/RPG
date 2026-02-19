using Rpg.Action;
using Rpg.Core;

namespace Rpg.Battle;

/// <summary>
/// 戰鬥階段抽象，對應 PDF。
/// </summary>
public abstract class BattlePhase
{
    public Role CurrentRole { get; set; } = null!;
    public IAction? SelectedAction { get; set; }
}
