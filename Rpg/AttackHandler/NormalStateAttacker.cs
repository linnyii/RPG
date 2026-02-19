using Rpg.Core;

namespace Rpg.AttackHandler;

/// <summary>
/// 預設：目標正常狀態 → 100 傷害。
/// </summary>
public class NormalStateAttacker : AttackHandler
{
    protected override bool CanHandle(Role attacker, Role target) => target.State == State.Normal;

    protected override void HandleAttackDetail(Role attacker, Role target)
    {
        target.TakeDamage(100);
    }
}
