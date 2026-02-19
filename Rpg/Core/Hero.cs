namespace Rpg.Core;

/// <summary>
/// 玩家英雄，對應 PDF 的 Hero。
/// </summary>
public class Hero : Role
{
    public Hero(string name, int hp, int mp, int str) : base(name, hp, mp, str) { }
}
