using Murder.Assets;
using Murder.Attributes;

namespace HelloMurder.Assets;

public class HelloMurderGameProfile : GameProfile
{
    [GameAssetId(typeof(LibraryAsset))]
    public readonly Guid Library;
}
