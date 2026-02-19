namespace Rpg.Core;

/// <summary>
/// 召喚物 Slime，對應 PDF。100 HP, 0 MP, 50 STR，無技能。
/// </summary>
public class Slime : Role
{
    /// <summary>
    /// 召喚者。
    /// </summary>
    public Role? SummonedBy { get; set; }

    public Slime(Role? summonedBy = null) : base("Slime", 100, 0, 50)
    {
        SummonedBy = summonedBy;
    }

    /// <summary>
    /// 用於區分多隻 Slime，如 Slime1, Slime2。
    /// </summary>
    public void SetDisplayName(string name)
    {
        Name = name;
    }
}
