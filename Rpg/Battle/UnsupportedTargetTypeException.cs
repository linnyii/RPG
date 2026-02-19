using Rpg.Action;

namespace Rpg.Battle;

public class UnsupportedTargetTypeException(TargetType targetType) : Exception($"Unsupported TargetType: {targetType}")
{
}
