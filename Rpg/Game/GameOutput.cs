using Rpg.Action;
using Rpg.Core;

namespace Rpg.Game;

/// <summary>
/// 輸出格式，對應補充規格。
/// </summary>
public static class GameOutput
{
    public static string StateName(State s) => s switch
    {
        State.Normal => "正常",
        State.Petrochemical => "石化",
        State.Poisoned => "中毒",
        State.Cheerup => "受到鼓舞",
        _ => "正常"
    };

    public static void PrintTurnStart(Role role)
    {
        Console.WriteLine($"輪到 [{role.TroopId}]{role.Name} (HP: {role.Hp}, MP: {role.Mp}, STR: {role.Str}, State: {StateName(role.State)})。");
    }

    public static void PrintActionChoice(Role role, List<IAction> actions)
    {
        var parts = actions.Select((a, i) => $"({i}) {a.Name}");
        Console.WriteLine($"選擇行動：{string.Join(" ", parts)}");
    }

    public static void PrintMpInsufficient()
    {
        Console.WriteLine("你缺乏 MP，不能進行此行動。");
    }

    public static void PrintTargetChoice(List<Core.Role> candidates)
    {
        var parts = candidates.Select((r, i) => $"({i}) [{r.TroopId}]{r.Name}");
        Console.WriteLine($"選擇 1 位目標: {string.Join(" ", parts)}");
    }

    public static void PrintTargetChoiceMulti(List<Role> candidates, int count)
    {
        var parts = candidates.Select((r, i) => $"({i}) [{r.TroopId}]{r.Name}");
        Console.WriteLine($"選擇 {count} 位目標: {string.Join(" ", parts)}");
    }

    public static void PrintSkillUse(Role attacker, List<Role> targets, IAction action)
    {
        if (action is BasicAttack)
        {
            Console.WriteLine($"[{attacker.TroopId}]{attacker.Name} 攻擊 [{targets[0].TroopId}]{targets[0].Name}。");
        }
        else
        {
            var targetNames = string.Join(", ", targets.Select(t => $"[{t.TroopId}]{t.Name}"));
            Console.WriteLine($"[{attacker.TroopId}]{attacker.Name} 對 {targetNames} 使用了 {action.Name}。");
        }
    }

    public static void PrintDamage(Role attacker, Core.Role target, int damage)
    {
        Console.WriteLine($"[{attacker.TroopId}]{attacker.Name} 對 [{target.TroopId}]{target.Name} 造成 {damage} 點傷害。");
    }

    public static void PrintDeath(Core.Role role)
    {
        Console.WriteLine($"[{role.TroopId}]{role.Name} 死亡。");
    }

    public static void PrintVictory()
    {
        Console.WriteLine("你獲勝了！");
    }

    public static void PrintDefeat()
    {
        Console.WriteLine("你失敗了。");
    }
}
