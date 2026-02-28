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
        OnMpIsSufficient = GameOutput.PrintMpInsufficient;
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
        // 若需輸出治療訊息可在此處理
    }

    public void OnSlimeSummoned(Role slime)
    {
        // 若需輸出召喚訊息可在此處理
    }

    public System.Action? OnMpIsSufficient { get; set; }
}
