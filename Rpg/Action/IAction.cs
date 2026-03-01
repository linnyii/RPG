using Rpg.Enum;

namespace Rpg.Action;

public interface IAction
{
    int MpCost { get; }
    string Name { get; }

    int TargetCount { get; }

    TargetType TargetType { get; }

    void Execute(Core.Role attacker, List<Core.Role> targets, Battle.BattleContext context);
}