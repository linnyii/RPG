using Rpg.Action;
using Rpg.Core;

namespace Rpg.Battle;

public class TakeTurnContext
{
    public required Role CurrentRole { get; init; }
    public required BattleContext BattleContext { get; init; }
    public string ReadLine() => Console.ReadLine() ?? "";

    public IAction? SelectedAction { get; set; }
    public List<Role>? SelectedTargets { get; set; }


    private HashSet<Type> CompletedSteps { get; } = [];

    public bool IsCompleted<T>() where T : IBattleStep
        => CompletedSteps.Contains(typeof(T));

    public void MarkCompleted<T>() where T : IBattleStep
        => CompletedSteps.Add(typeof(T));

    public void Run(IBattleStep firstStep)
    {
        var step = firstStep;
        while (step != null)
        {
            if (!step.CanExecute(this))
                throw new InvalidOperationException($"前置條件不滿足: {step.GetType().Name}");
            step = step.Execute(this);
        }
    }
}
