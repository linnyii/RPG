namespace Rpg.Core;

public class UnsupportedStateException(State state) : Exception($"UnSupported State: {state}")
{
    public State State { get; } = state;
}
