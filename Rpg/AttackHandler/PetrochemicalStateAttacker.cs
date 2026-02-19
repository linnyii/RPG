using Rpg.Core;

namespace Rpg.AttackHandler;

/// <summary>
/// 石化狀態（與 Poisoned 合併由 PoisonedStateAttacker 處理，此為 fallback）。
/// </summary>
public class PetrochemicalStateAttacker : AttackHandler
{
    protected override bool CanHandle(Role target) => target.State == State.Petrochemical;

    protected override void HandleAttackDetail(Role target)
    {
        //TODO: why need for loop? can take damage with 240 ?
        for (var i = 0; i < 3; i++)
            target.TakeDamage(80);
    }
}
