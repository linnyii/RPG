namespace Rpg.Core;

public class Slime(Role? summonedBy = null) : Role("Slime", 100, 0, 50)
{
    public Role? SummonedBy { get; set; } = summonedBy;

    public void SetDisplayName(string name)
    {
        Name = name;
    }
}
