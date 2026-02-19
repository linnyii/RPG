using Rpg.Core;

namespace Rpg.AttackHandler;

/// <summary>
/// Chain of Responsibility 抽象，對應 PDF。
/// </summary>
public abstract class AttackHandler
{
    protected AttackHandler? Next { get; set; }

    public void SetNext(AttackHandler handler)
    {
        Next = handler;
    }

    public void Handle(Role attacker, Role target)
    {
        if (CanHandle(attacker, target))
        {
            HandleAttackDetail(attacker, target);
            return;
        }
        Next?.Handle(attacker, target);
    }

    protected abstract bool CanHandle(Role attacker, Role target);
    protected abstract void HandleAttackDetail(Role attacker, Role target);
}
