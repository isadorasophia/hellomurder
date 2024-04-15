using Bang.Components;

namespace HelloMurder.Messages;

/// <summary>
/// Indicates that an agent is trying to perform an action symbolized by an InputButton
/// </summary>
public readonly struct AgentInputMessage : IMessage
{
    public readonly int Button;

    public AgentInputMessage(int button)
    {
        Button = button;
    }
}
