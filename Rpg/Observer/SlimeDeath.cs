using Rpg.Core;

namespace Rpg.Observer;

/// <summary>
/// Slime 死亡且召喚者存活 → 召喚者 +30 HP。
/// </summary>
public class SlimeDeath : IAddHpObserver
{
    public void UpdateHp(List<Role> roles)
    {
        foreach (var role in roles)
        {
            if (role is not Slime slime || slime.IsAlive)
                continue; // 只處理已死亡的 Slime
            if (slime.SummonedBy != null && slime.SummonedBy.IsAlive)
                slime.SummonedBy.AddHp(30);
        }
    }
}
