using Rpg.Core;

namespace Rpg.Observer;

/// <summary>
/// 對應 PDF 的 AddHpObserver。
/// </summary>
public interface IAddHpObserver
{
    void UpdateHp(Role deadRole);
}
