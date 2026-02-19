using Rpg.Core;

namespace Rpg.AttackHandler;

/// <summary>
/// Chain of Responsibility 抽象，對應 PDF。
/// </summary>
public abstract class AttackHandler
{
    private AttackHandler? Next { get; set; }

    public void SetNext(AttackHandler handler)
    {
        Next = handler;
    }

    public void Handle(Role target)
    {
        if (CanHandle(target))
        {
            HandleAttackDetail(target);
            return;
        }
        Next?.Handle(target);
    }

    protected abstract bool CanHandle(Role target);
    protected abstract void HandleAttackDetail(Role target);
}
