using Rpg.AiStrategy;

namespace Rpg.Core;

/// <summary>
/// AI 敵人，對應 PDF 的 AI。持有 Seed 用於決策。
/// </summary>
public class AI : Role
{
    public int Seed { get; set; }
    public IAiSelectionStrategy SelectionStrategy { get; set; }

    public AI(string name, int hp, int mp, int str, IAiSelectionStrategy strategy) : base(name, hp, mp, str)
    {
        SelectionStrategy = strategy;
    }
}
