using Rpg.Battle;
using Rpg.Core;
using Rpg.Observer;

namespace Rpg.Game;

/// <summary>
/// 遊戲入口，對應 PDF 的 RpgGame。集中管理 Observer 與 Notify。
/// </summary>
public class RpgGame(Troop troop1, Troop troop2, Role? hero)
{
    public Troop Troop1 { get; } = troop1;
    public Troop Troop2 { get; } = troop2;
    public Role? Hero { get; set; } = hero;
    private readonly List<IAddHpObserver> _observers = new();

    public Action<Role, Role, int, bool>? OnDamageDealt { get; set; }
    public Action<Role>? OnRoleDiedOutput { get; set; }

    public void RegisterObserver(IAddHpObserver observer) => _observers.Add(observer);
    public void UnRegisterObserver(IAddHpObserver observer) => _observers.Remove(observer);

    public void Notify(Role deadRole)
    {
        foreach (var obs in _observers)
            obs.UpdateHp(deadRole);
    }

    /// <summary>
    /// 當行動造成傷害並可能導致死亡時呼叫。若 dead 則觸發 Notify。
    /// </summary>
    public void OnRoleDealtDamage(Role attacker, Role target, int damage, bool dead)
    {
        //TODO: why need so many parameter ?
        OnDamageDealt?.Invoke(attacker, target, damage, dead);
        if (dead)
        {
            OnRoleDiedOutput?.Invoke(target);
            Notify(target);
        }
    }

    /// <summary>
    /// 當角色直接死亡時（如自爆）呼叫。
    /// </summary>
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

    /// <summary>
    /// MP 不足時由 ActionSelectionPhase 呼叫。
    /// </summary>
    public System.Action? OnMpInsufficient { get; set; }
}
