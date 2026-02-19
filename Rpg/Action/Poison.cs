using Rpg.Core;

namespace Rpg.Action;

/// <summary>
/// 中毒。MP: 80，1 位敵軍，三回合中毒。
/// </summary>
public class Poison : IAction
{
    public string Name => "中毒";
    public int MpCost => 80;
    public int TargetCount => 1;
    public TargetType TargetType => TargetType.Enemy;

    public void Execute(Role attacker, List<Role> targets, Battle.BattleContext context)
    {
        attacker.Mp -= MpCost;
        foreach (var target in targets)
        {
            target.State = State.Poisoned;
            target.StateRounds = 3;
        }
    }
}
