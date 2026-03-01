namespace Rpg.Core;

/// <summary>
/// 玩家英雄，對應 PDF 的 Hero。
/// </summary>
public class Hero(string name, int hp, int mp, int str)
    : Role(name, hp, mp, str);
