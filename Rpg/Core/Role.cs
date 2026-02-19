using Rpg.Action;

namespace Rpg.Core;

/// <summary>
/// 角色基底，對應 PDF 的 Role。
/// </summary>
public abstract class Role
{
    public string Name { get; protected set; }
    public int Hp { get; set; }
    public int Mp { get; set; }
    public int Str { get; set; }
    public State State { get; set; } = State.Normal;
    
    /// <summary>
    /// 狀態剩餘回合數，歸零時還原為 Normal。
    /// </summary>
    public int StateRounds { get; set; }

    /// <summary>
    /// 受誰詛咒（同一施咒者可多次加入，規格說不疊加）
    /// </summary>
    public List<Role> CursedBy { get; } = new();

    /// <summary>
    /// 可執行的行動列表（含 BasicAttack + 技能）
    /// </summary>
    public List<IAction> Actions { get; } = new();

    public int TroopId { get; set; }

    protected Role(string name, int hp, int mp, int str)
    {
        Name = name;
        Hp = hp;
        Mp = mp;
        Str = str;
    }

    public bool IsAlive => Hp > 0;

    /// <summary>
    /// 扣除 HP，返回是否死亡。
    /// </summary>
    public bool TakeDamage(int damage)
    {
        Hp = Math.Max(0, Hp - damage);
        return !IsAlive;
    }

    /// <summary>
    /// 增加 HP。
    /// </summary>
    public void AddHp(int amount)
    {
        Hp = Math.Min(1000, Hp + amount);
    }

    public void DecrementStateRounds()
    {
        if (State == State.Normal) return;
        StateRounds--;
        //TODO: Magic number
        if (StateRounds <= 0)
        {
            State = State.Normal;
            StateRounds = 0;
        }
    }
}
