using Murder.Assets;
using Murder.Attributes;
using Murder.Utilities;
using System.Numerics;

namespace HelloMurder.Assets;

public class LibraryAsset : GameAsset
{
    public override string EditorFolder => "#\uf02dLibraries";

    public override char Icon => '\uf02d';

    public override Vector4 EditorColor => "#FA5276".ToVector4Color();

    [GameAssetId(typeof(UiLocalizationResourcesAsset))]
    public readonly Guid UiLocalizationResources = Guid.Empty;
}
