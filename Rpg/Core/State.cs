namespace Rpg.Core;

/// <summary>
/// 角色狀態（對應規格 STATE: Normal | 石化 | 中毒 | 受到鼓舞）
/// </summary>
public enum State
{
    Normal,      // 正常
    Petrochemical, // 石化
    Poisoned,    // 中毒
    Cheerup      // 受到鼓舞
}
