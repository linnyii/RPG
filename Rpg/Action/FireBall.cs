using Rpg.Core;

namespace Rpg.Action;

/// <summary>
/// 火球。MP: 50，所有敵軍，各 50 傷害。
/// </summary>
public class FireBall : IAction
{
    public string Name => "火球";
    public int MpCost => 50;
    public int TargetCount => 0; // 全體，自動選
    public TargetType TargetType => TargetType.Enemy;

    public void Execute(Role attacker, List<Role> targets, Battle.BattleContext context)
    {
        attacker.Mp -= MpCost;
        foreach (var target in targets)
        {
            var damage = 50;
            if (attacker.State == State.Cheerup)
                damage += 50;
            var dead = target.TakeDamage(damage);
            context.Game?.OnRoleDealtDamage(attacker, target, damage, dead);
        }
    }
}
