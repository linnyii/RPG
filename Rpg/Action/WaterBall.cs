using Rpg.Core;

namespace Rpg.Action;

/// <summary>
/// 水球。MP: 50，1 位敵軍，120 傷害。
/// </summary>
public class WaterBall : IAction
{
    public string Name => "水球";
    public int MpCost => 50;
    public int TargetCount => 1;
    public TargetType TargetType => TargetType.Enemy;

    public void Execute(Role attacker, List<Role> targets, Battle.BattleContext context)
    {
        attacker.DeductMp(MpCost);
        foreach (var target in targets)
        {
            var damage = 120;
            if (attacker.State == State.CheerUp)
                damage += 50;
            var dead = target.TakeDamage(damage);
            context.Game?.OnRoleDealtDamage(attacker, target, damage, dead);
        }
    }
}
