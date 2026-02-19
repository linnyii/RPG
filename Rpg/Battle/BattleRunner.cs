using Rpg.Action;
using Rpg.Core;
using Rpg.Game;

namespace Rpg.Battle;

/// <summary>
/// 負責戰鬥主迴圈：P→E→S1→S2→S3，回合結束狀態倒數。
/// </summary>
public class BattleRunner
{
    private readonly BattleContext _context;
    private readonly ActionSelectionPhase _s1 = new();
    private readonly TargetSelectionPhase _s2 = new();
    private readonly ActionExecutionPhase _s3 = new();

    public BattleRunner(BattleContext context)
    {
        _context = context;
    }

    public BattleResult Run(Func<string> readLine)
    {
        var game = _context.Game!;
        var playerTroop = _context.PlayerTroop;
        var enemyTroop = _context.EnemyTroop;

        game.OnDamageDealt = (attacker, target, damage, dead) =>
        {
            GameOutput.PrintDamage(attacker, target, damage);
            if (dead)
                GameOutput.PrintDeath(target);
        };
        game.OnRoleDiedOutput = GameOutput.PrintDeath;
        game.OnMpInsufficient = GameOutput.PrintMpInsufficient;

        while (true)
        {
            foreach (var role in GetAllActingOrder())
            {
                if (!role.IsAlive) continue;

                // P 步驟
                GameOutput.PrintTurnStart(role);

                // E 步驟
                if (role.State == State.Petrochemical)
                    continue;
                if (role.State == State.Poisoned)
                {
                    role.TakeDamage(30);
                    if (!role.IsAlive)
                    {
                        GameOutput.PrintDeath(role);
                        game.Notify(role);
                        var r = CheckBattleEnd();
                        if (r != BattleResult.Ongoing) return r;
                        continue;
                    }
                }

                // S1
                var validActions = role.Actions.Where(a => role.Mp >= a.MpCost).ToList();
                IAction action;
                if (role is Core.AI ai)
                {
                    action = ai.SelectionStrategy.SelectAction(ai, validActions);
                }
                else
                {
                    GameOutput.PrintActionChoice(role, role.Actions);
                    while (true)
                    {
                        var input = readLine();
                        if (int.TryParse(input?.Trim(), out var idx) && idx >= 0 && idx < role.Actions.Count)
                        {
                            var a = role.Actions[idx];
                            if (role.Mp >= a.MpCost)
                            {
                                action = a;
                                break;
                            }
                            GameOutput.PrintMpInsufficient();
                            GameOutput.PrintActionChoice(role, role.Actions);
                        }
                        else
                        {
                            GameOutput.PrintActionChoice(role, role.Actions);
                        }
                    }
                }

                // S2
                var candidates = GetCandidates(role, action);
                var needCount = action.TargetCount == 0 ? candidates.Count : action.TargetCount;
                List<Core.Role> targets;
                if (role is Core.AI ai2)
                {
                    targets = ai2.SelectionStrategy.SelectTargets(ai2, candidates, needCount);
                }
                else
                {
                    if (needCount > 0 && candidates.Count > needCount)
                    {
                        if (needCount == 1)
                            GameOutput.PrintTargetChoice(candidates);
                        else
                            GameOutput.PrintTargetChoiceMulti(candidates, needCount);
                        var parts = readLine()!.Split(',', ' ').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();
                        targets = parts.Select(s => int.TryParse(s, out var i) ? i : -1).Where(i => i >= 0 && i < candidates.Count).Take(needCount).Select(i => candidates[i]).ToList();
                    }
                    else
                    {
                        targets = candidates.Take(needCount).ToList();
                    }
                }

                GameOutput.PrintSkillUse(role, targets, action);
                action.Execute(role, targets, _context);

                var result = CheckBattleEnd();
                if (result != BattleResult.Ongoing) return result;
            }

            // 回合結束：狀態倒數
            foreach (var r in _context.GetAllAliveRoles())
                r.DecrementStateRounds();
        }
    }

    private IEnumerable<Core.Role> GetAllActingOrder()
    {
        foreach (var r in _context.PlayerTroop.Allies.Where(r => r.IsAlive))
            yield return r;
        foreach (var r in _context.EnemyTroop.Allies.Where(r => r.IsAlive))
            yield return r;
    }

    private List<Core.Role> GetCandidates(Core.Role actor, IAction action)
    {
        var enemyTroop = actor.TroopId == _context.PlayerTroop.Id ? _context.EnemyTroop : _context.PlayerTroop;
        var allyTroop = actor.TroopId == _context.PlayerTroop.Id ? _context.PlayerTroop : _context.EnemyTroop;
        return action.TargetType switch
        {
            TargetType.Self => new List<Core.Role> { actor },
            TargetType.Enemy => enemyTroop.Allies.Where(r => r.IsAlive).ToList(),
            TargetType.Ally => allyTroop.Allies.Where(r => r.IsAlive && r != actor).ToList(),
            TargetType.All => _context.GetAllAliveRoles().ToList(),
            _ => new List<Core.Role>()
        };
    }

    private BattleResult CheckBattleEnd()
    {
        var hero = _context.Hero;
        if (hero != null && !hero.IsAlive)
            return BattleResult.PlayerLose;
        if (!_context.EnemyTroop.Allies.Any(r => r.IsAlive))
            return BattleResult.PlayerWin;
        return BattleResult.Ongoing;
    }
}
