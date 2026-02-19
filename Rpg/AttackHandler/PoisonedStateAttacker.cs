using Rpg.Core;

namespace Rpg.AttackHandler;

/// <summary>
/// 目標為中毒或石化 → 3×80 傷害。
/// </summary>
public class PoisonedStateAttacker : AttackHandler
{
    protected override bool CanHandle(Role attacker, Role target) =>
        target.State == State.Poisoned || target.State == State.Petrochemical;

    protected override void HandleAttackDetail(Role attacker, Role target)
    {
        for (var i = 0; i < 3; i++)
            target.TakeDamage(80);
    }
}
