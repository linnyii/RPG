using Rpg.Core;

namespace Rpg.AttackHandler;

/// <summary>
/// 目標為中毒或石化 → 3×80 傷害。
/// </summary>
public class PoisonedStateAttacker : AttackHandler
{
    protected override bool CanHandle(Role target) =>
        target.State is State.Poisoned or State.Petrochemical;

    protected override void HandleAttackDetail(Role target)
    {
        target.TakeDamage(240);
    }
}
