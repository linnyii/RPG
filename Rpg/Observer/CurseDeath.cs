using Rpg.Core;

namespace Rpg.Observer;

/// <summary>
/// 受詛咒者死亡時，施咒者 + 剩餘 MP（同一施咒者不疊加）。
/// </summary>
public class CurseDeath : IAddHpObserver
{
    public void UpdateHp(List<Role> roles)
    {
        foreach (var dead in roles.Where(r => !r.IsAlive))
        {
            foreach (var caster in dead.CursedBy.Distinct().Where(c => c.IsAlive))
            {
                caster.AddHp(dead.Mp);
            }
        }
    }
}
