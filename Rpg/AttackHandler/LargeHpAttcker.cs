using Rpg.Core;

namespace Rpg.AttackHandler;

/// <summary>
/// 目標 HP ≥ 500 → 300 傷害。
/// </summary>
public class LargeHpAttcker : AttackHandler
{
    protected override bool CanHandle(Role attacker, Role target) => target.Hp >= 500;

    protected override void HandleAttackDetail(Role attacker, Role target)
    {
        target.TakeDamage(300);
    }
}
