using Rpg.Core;
using Rpg.Observer;

namespace Rpg.Game;

public class RpgGame
{
    private readonly List<IAddHpObserver> _observers = [];

    private Action<Role, Role, int, bool>? OnDamageDealt { get; }
    private Action<Role>? OnRoleDiedOutput { get; }

    public RpgGame()
    {
        OnDamageDealt = (attacker, target, damage, dead) =>
        {
            GameOutput.PrintDamage(attacker, target, damage);
            if (dead)
                GameOutput.PrintDeath(target);
        };
        OnRoleDiedOutput = GameOutput.PrintDeath;
        OnMpIsSufficient = GameOutput.PrintMpIsSufficient;
    }

    public void RegisterObserver(IAddHpObserver observer) => _observers.Add(observer);
    public void UnRegisterObserver(IAddHpObserver observer) => _observers.Remove(observer);

    public void Notify(Role deadRole)
    {
        foreach (var obs in _observers)
            obs.UpdateHp(deadRole);
    }

    public void OnRoleDealtDamage(Role attacker, Role target, int damage, bool dead)
    {
        OnDamageDealt?.Invoke(attacker, target, damage, dead);
        if (!dead) return;
        OnRoleDiedOutput?.Invoke(target);
        Notify(target);
    }

    public void OnRoleDied(Role dead)
    {
        OnRoleDiedOutput?.Invoke(dead);
        Notify(dead);
    }

    public void OnRoleHealed(Role role, int amount)
    {
        //to be implemented
    }

    public void OnSlimeSummoned(Role slime)
    {
        //to be implemented
    }

    public System.Action? OnMpIsSufficient { get; set; }
}
