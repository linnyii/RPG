using Rpg.Action;

namespace Rpg.Core;

public abstract class Role(string name, int hp, int mp, int str)
{
    public string Name { get; protected set; } = name;
    public int Hp { get; private set; } = hp;
    public int Mp { get; private set; } = mp;
    public int Str { get; } = str;
    public State State { get; private set; } = State.Normal;
    private int StateRounds { get; set; }

    public List<Role> CursedBy { get; } = [];

    public List<IAction> Actions { get; } = [];

    public int TroopId { get; init; }

    public bool IsAlive => Hp > 0;

    public bool TakeDamage(int damage)
    {
        Hp = Math.Max(0, Hp - damage);
        return !IsAlive;
    }

    public void AddHp(int amount)
    {
        Hp = Math.Min(1000, Hp + amount);
    }

    public void DeductMp(int cost)
    {
        Mp -= cost;
    }

    public void ApplyState(State state, int rounds)
    {
        State = state;
        StateRounds = rounds;
    }

    public void ClearState()
    {
        State = State.Normal;
        StateRounds = 0;
    }

    public void DecrementStateRounds()
    {
        if (State == State.Normal) return;
        StateRounds--;
        if (IsStillUnNormalStatue()) return;
        ClearState();
    }

    private bool IsStillUnNormalStatue()
    {
        return StateRounds > 0;
    }
}
