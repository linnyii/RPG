using Rpg.Core;

namespace Rpg.AttackHandler;

/// <summary>
/// 目標為受到鼓舞 → 100 傷害並恢復 Normal。
/// </summary>
public class CheerUpStateAttacker : AttackHandler
{
    protected override bool CanHandle(Role target) => target.State == State.CheerUp;

    protected override void HandleAttackDetail(Role target)
    {
        target.TakeDamage(100);
        target.SetStateToNormal();
    }
}
