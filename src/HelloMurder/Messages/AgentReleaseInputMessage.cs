﻿using Bang.Components;

namespace HelloMurder.Messages;

public readonly struct AgentReleaseInputMessage : IMessage
{
    public readonly int Button;
    public AgentReleaseInputMessage(int button)
    {
        this.Button = button;
    }
}
