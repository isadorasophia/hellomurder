using Bang.Components;
using Murder.Assets;

namespace HelloMurder.Components;

internal readonly struct InteractHighlighterComponent : IComponent
{
    public readonly LocalizedString Title = new();

    public InteractHighlighterComponent() { }
}
