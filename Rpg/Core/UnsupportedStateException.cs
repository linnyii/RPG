using Rpg.Enum;

namespace Rpg.Core;

public class UnsupportedStateException(State state) : Exception($"UnSupported State: {state}")
{
}
