using Rpg.Action;

namespace Rpg.Battle;

public class UnsupportedTargetTypeException(TargetType targetType) : Exception($"Unsupported TargetType: {targetType}")
{
    public TargetType TargetType { get; } = targetType;
}
