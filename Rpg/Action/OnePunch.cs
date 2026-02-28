using Rpg.Core;
using Rpg.AttackHandler;

namespace Rpg.Action;

/// <summary>
/// 一拳。MP: 180，1 位敵軍，依目標狀態/血量由 AttackHandler 鏈決定效果。
/// </summary>
public class OnePunch : IAction
{
    private readonly AttackHandler.AttackHandler _handlerChain;

    public OnePunch()
    {
        var largeHp = new LargeHpAttcker();
        var poisoned = new PoisonedStateAttacker();
        var cheerUp = new CheerUpStateAttacker();
        var petro = new PetrochemicalStateAttacker();
        var normal = new NormalStateAttacker();

        largeHp.SetNext(poisoned);
        poisoned.SetNext(cheerUp);
        cheerUp.SetNext(petro);
        petro.SetNext(normal);

        _handlerChain = largeHp;
    }

    public string Name => "一拳";
    public int MpCost => 180;
    public int TargetCount => 1;
    public TargetType TargetType => TargetType.Enemy;

    public void Execute(Role attacker, List<Role> targets, Battle.BattleContext context)
    {
        attacker.Mp -= MpCost;
        foreach (var target in targets)
        {
            var hpBefore = target.Hp;
            _handlerChain.Handle(target);
            var damage = hpBefore - target.Hp;
            var dead = !target.IsAlive;
            context.Game?.OnRoleDealtDamage(attacker, target, damage, dead);
        }
    }
}
