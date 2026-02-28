using Rpg.Core;

namespace Rpg.Action;

/// <summary>
/// 鼓舞。MP: 100，3 位友軍，三回合受到鼓舞。
/// </summary>
public class Cheerup : IAction
{
    public string Name => "鼓舞";
    public int MpCost => 100;
    public int TargetCount => 3;
    public TargetType TargetType => TargetType.Ally;

    public void Execute(Role attacker, List<Role> targets, Battle.BattleContext context)
    {
        attacker.DeductMp(MpCost);
        foreach (var target in targets)
        {
            target.ApplyState(State.CheerUp, 3);
        }
    }
}
