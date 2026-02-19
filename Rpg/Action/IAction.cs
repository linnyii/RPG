namespace Rpg.Action;

/// <summary>
/// 對應 PDF 的 Action/Skills 介面。
/// 規格使用 Attack(Attacker, Roles)，實作上以 Execute 較語意清楚。
/// </summary>
public interface IAction
{
    string Name { get; }
    int MpCost { get; }
    
    /// <summary>
    /// 需要的目標數量。0 = 無需選（如 Summon），1 = 單體，依此類推。
    /// </summary>
    int TargetCount { get; }
    
    /// <summary>
    /// 目標類型：Enemy, Ally, Self, All
    /// </summary>
    TargetType TargetType { get; }

    void Execute(Core.Role attacker, List<Core.Role> targets, Battle.BattleContext context);
}

public enum TargetType
{
    Self,   // 自己（如 SelfHealing）
    Enemy,  // 敵軍
    Ally,   // 友軍
    All     // 全場（如 SelfExplosion）
}
