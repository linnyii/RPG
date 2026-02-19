namespace Rpg.Core;

public class UnsupportedStateException(State state) : Exception($"UnSupported State: {state}")
{
}
