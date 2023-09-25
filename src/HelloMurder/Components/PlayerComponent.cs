using Bang.Components;
using Murder.Attributes;

namespace HelloMurder.Components
{
    [Unique]
    [DoNotPersistEntityOnSave]
    public readonly struct PlayerComponent : IComponent
    {
        public PlayerComponent()
        {
        }
    }
}

