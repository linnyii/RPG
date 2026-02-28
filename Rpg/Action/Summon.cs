using Rpg.Core;

namespace Rpg.Action;

/// <summary>
/// 召喚 Slime。MP: 150，加入己軍尾端。Slime 死亡且召喚者存活時，召喚者 +30 HP。
/// </summary>
public class Summon : IAction
{
    public string Name => "召喚";
    public int MpCost => 150;
    public int TargetCount => 0;
    public TargetType TargetType => TargetType.Self;

    public void Execute(Role attacker, List<Role> targets, Battle.BattleContext context)
    {
        attacker.DeductMp(MpCost);

        var troop = attacker.TroopId == context.PlayerTroop.Id ? context.PlayerTroop : context.EnemyTroop;
        var slime = new Slime(attacker) { TroopId = troop.Id };
        var slimeCount = troop.Allies.Count(a => a is Slime) + 1;
        slime.SetDisplayName($"Slime{slimeCount}");
        troop.Allies.Add(slime);
        slime.Actions.Add(new BasicAttack());

        context.Game?.OnSlimeSummoned(slime);
    }
}
