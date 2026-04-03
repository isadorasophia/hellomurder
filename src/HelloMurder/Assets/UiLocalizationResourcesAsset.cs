using Murder.Assets;
using Murder.Utilities.Attributes;
using System.Numerics;

namespace HelloMurder.Assets;

public class UiLocalizationResourcesAsset : GameAsset
{
    public override string EditorFolder => "#Ui";

    public override char Icon => '';

    public override Vector4 EditorColor => new(1f, .8f, .25f, 1f);

    [Folder]
    public MenuLocalizationResources Menu = new();
}

public class MenuLocalizationResources
{
    // Main menu
    public readonly LocalizedString NewGame;
    public readonly LocalizedString Continue;
    public readonly LocalizedString Options;
    public readonly LocalizedString Exit;

    public readonly LocalizedString SoundsOn;
    public readonly LocalizedString SoundsOff;
    public readonly LocalizedString MusicOn;
    public readonly LocalizedString MusicOff;
    public readonly LocalizedString CurrentLanguage;

    public readonly LocalizedString Back;
}