using Rpg.Core;

namespace Rpg.Action;

/// <summary>
/// 自爆。MP: 200，全場 150 傷害，行動者自殺。
/// </summary>
public class SelfExplosion : IAction
{
    public string Name => "自爆";
    public int MpCost => 200;
    public int TargetCount => 0;
    public TargetType TargetType => TargetType.All;

    public void Execute(Role attacker, List<Role> targets, Battle.BattleContext context)
    {
        attacker.Mp -= MpCost;
        attacker.TakeDamage(attacker.Hp); // 自殺
        context.Game?.OnRoleDied(attacker);

        foreach (var target in targets.Where(t => t != attacker && t.IsAlive))
        {
            var dead = target.TakeDamage(150);
            context.Game?.OnRoleDealtDamage(attacker, target, 150, dead);
        }
    }
}
