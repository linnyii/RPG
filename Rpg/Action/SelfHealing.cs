using Rpg.Core;

namespace Rpg.Action;

/// <summary>
/// 自我治療。MP: 50，目標自己，+150 HP。
/// </summary>
public class SelfHealing : IAction
{
    public string Name => "自我治療";
    public int MpCost => 50;
    public int TargetCount => 1;
    public TargetType TargetType => TargetType.Self;

    public void Execute(Role attacker, List<Role> targets, Battle.BattleContext context)
    {
        attacker.DeductMp(MpCost);
        attacker.AddHp(150);
        context.Game?.OnRoleHealed(attacker, 150);
    }
}
