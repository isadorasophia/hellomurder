using Bang.Components;
using Murder.Assets;

namespace HelloMurder.Components;

internal readonly struct DialogueUiComponent : IComponent
{
    public readonly LocalizedString Content = new();

    public DialogueUiComponent() { }
}
