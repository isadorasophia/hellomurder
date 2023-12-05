using Bang.Components;
using Murder.Assets;
using System.Collections.Immutable;

namespace HelloMurder.Components;

internal readonly struct DialogueUiComponent : IComponent
{
    public readonly ImmutableArray<LocalizedString> Content = ImmutableArray<LocalizedString>.Empty;

    public DialogueUiComponent() { }
}
