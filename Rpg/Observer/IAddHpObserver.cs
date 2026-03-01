using Rpg.Core;

namespace Rpg.Observer;

public interface IAddHpObserver
{
    void UpdateHp(Role deadRole);
}
