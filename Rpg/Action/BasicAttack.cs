using Rpg.Core;

namespace Rpg.Action;

/// <summary>
/// 普通攻擊。MP: 0，1 位敵軍，傷害 = STR。
/// </summary>
public class BasicAttack : IAction
{
    public string Name => "普通攻擊";
    public int MpCost => 0;
    public int TargetCount => 1;
    public TargetType TargetType => TargetType.Enemy;

    public void Execute(Role attacker, List<Role> targets, Battle.BattleContext context)
    {
        foreach (var target in targets)
        {
            var damage = attacker.Str;
            if (attacker.State == State.CheerUp)
                damage += 50;
            var dead = target.TakeDamage(damage);
            context.Game?.OnRoleDealtDamage(attacker, target, damage, dead);
        }
    }
}
