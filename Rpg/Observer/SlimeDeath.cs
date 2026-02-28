using Rpg.Core;

namespace Rpg.Observer;

/// <summary>
/// Slime 死亡且召喚者存活 → 召喚者 +30 HP。
/// </summary>
public class SlimeDeath : IAddHpObserver
{
    public void UpdateHp(Role deadRole)
    {
        if (deadRole is not Slime slime || slime.IsAlive)
            return;
        if (slime.SummonedBy is { IsAlive: true })
            slime.SummonedBy.AddHp(30);
    }
}
