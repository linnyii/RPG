using Rpg.Core;

namespace Rpg.AttackHandler;

/// <summary>
/// 目標為受到鼓舞 → 100 傷害並恢復 Normal。
/// </summary>
public class CheerUpStateAttacker : AttackHandler
{
    protected override bool CanHandle(Role target) => target.State == State.Cheerup;

    protected override void HandleAttackDetail(Role target)
    {
        target.TakeDamage(100);
        target.State = State.Normal;
        target.StateRounds = 0;
    }
}
