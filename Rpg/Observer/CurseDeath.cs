using Rpg.Core;

namespace Rpg.Observer;

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
