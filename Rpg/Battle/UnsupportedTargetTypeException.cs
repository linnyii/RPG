using Rpg.Action;
using Rpg.Enum;

namespace Rpg.Battle;

public class UnsupportedTargetTypeException(TargetType targetType) : Exception($"Unsupported TargetType: {targetType}")
{
}
