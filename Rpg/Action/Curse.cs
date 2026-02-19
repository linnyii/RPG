using Rpg.Core;

namespace Rpg.Action;

/// <summary>
/// 詛咒。MP: 100，1 位敵軍。死亡時施咒者 + 剩餘 MP（同一施咒者不疊加）。
/// </summary>
public class Curse : IAction
{
    public string Name => "詛咒";
    public int MpCost => 100;
    public int TargetCount => 1;
    public TargetType TargetType => TargetType.Enemy;

    public void Execute(Role attacker, List<Role> targets, Battle.BattleContext context)
    {
        attacker.Mp -= MpCost;
        foreach (var target in targets)
        {
            if (!target.CursedBy.Contains(attacker))
                target.CursedBy.Add(attacker);
        }
    }
}
