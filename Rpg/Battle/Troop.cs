using Rpg.Core;

namespace Rpg.Battle;

public class Troop(int id)
{
    public string Name { get; set; } = "";
    public int Id { get; set; } = id;
    public List<Role> Allies { get; } = [];
    public Troop? EnemyTroop { get; set; }

    public void SetEnemy(Troop enemy)
    {
        EnemyTroop = enemy;
    }
}
