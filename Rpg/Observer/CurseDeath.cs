using Rpg.Core;

namespace Rpg.Observer;

/// <summary>
/// 受詛咒者死亡時，施咒者 + 剩餘 MP（同一施咒者不疊加）。
/// </summary>
public class CurseDeath : IAddHpObserver
{
    public void UpdateHp(Role deadRole)
    {
        if (deadRole.IsAlive)
            return;
        foreach (var caster in deadRole.CursedBy.Distinct().Where(c => c.IsAlive))
        {
            caster.AddHp(deadRole.Mp);
        }
    }
}
