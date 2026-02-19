using Rpg.Core;

namespace Rpg.Action;

/// <summary>
/// 石化。MP: 100，1 位敵軍，三回合石化。
/// </summary>
public class Petrochemical : IAction
{
    public string Name => "石化";
    public int MpCost => 100;
    public int TargetCount => 1;
    public TargetType TargetType => TargetType.Enemy;

    public void Execute(Role attacker, List<Role> targets, Battle.BattleContext context)
    {
        attacker.Mp -= MpCost;
        foreach (var target in targets)
        {
            target.State = State.Petrochemical;
            target.StateRounds = 3;
        }
    }
}
