using Rpg.Core;

namespace Rpg.Battle;

/// <summary>
/// 軍隊，對應 PDF 的 Troop。
/// </summary>
public class Troop
{
    public string Name { get; set; } = "";
    public int Id { get; set; }
    public List<Role> Allies { get; } = new();
    public Troop? EnemyTroop { get; set; }

    public Troop(int id)
    {
        Id = id;
    }

    public void SetEnemy(Troop enemy)
    {
        EnemyTroop = enemy;
    }
}
