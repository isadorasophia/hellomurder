using Bang;
using HelloMurder.Assets;
using HelloMurder.Core;
using HelloMurder.Services;
using Microsoft.Xna.Framework.Input;
using Murder;
using Murder.Assets;
using Murder.Core.Input;
using Murder.Serialization;
using Murder.Services;
using System.Text.Json;

namespace HelloMurder;

/// <summary>
/// <inheritdoc cref="IMurderGame"/>
/// </summary>
public class HelloMurderGame : IMurderGame
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public string Name => "HelloMurder";

    public JsonSerializerOptions Options => HelloMurderSerializerOptionsExtensions.Options;

    public ComponentsLookup ComponentsLookup => new HelloMurderComponentsLookup();

    public GameProfile CreateGameProfile() => new HelloMurderGameProfile();

    private static UiLocalizationResourcesAsset? _resources = null;

    /// <summary>
    /// Easy to use accessor for localization resources. We want this to be friendly since it will be used
    /// a ~looot~.
    /// </summary>
    public static UiLocalizationResourcesAsset Resources => _resources ??= LibraryServices.GetUiLocalizationResources();

    public static HelloMurderGameProfile Profile => (HelloMurderGameProfile)Game.Profile;

    public int GetDefaultFont() => (int)MurderFonts.PixelFont;

    public int GameWidth => 640;
    public int GameHeight => 360;
    public void Initialize()
    {
        Game.Input.RegisterButton(MurderInputButtons.Space, Buttons.Y);
        Game.Input.RegisterButton(MurderInputButtons.Backspace, Buttons.X);

        Game.Input.RegisterButton(InputButtons.SubmitWithEnter, Keys.Enter);
        Game.Input.RegisterButton(InputButtons.SubmitWithEnter, Buttons.Start);

        Game.Input.RegisterButton(MurderInputButtons.Pause, Keys.Escape, Keys.P);
        Game.Input.RegisterButton(MurderInputButtons.Pause, Buttons.Start);
    }

    public void OnAfterContentLoaded()
    {
        // we probably load that from preferences.
        Game.Input.RegisterButton(MurderInputButtons.Submit, Keys.Space, Keys.Enter);
        Game.Input.RegisterButton(MurderInputButtons.Cancel, Keys.Escape, Keys.Delete, Keys.Back, Keys.Tab);

        // Restore the default input bindings.
        Game.Input.LoadFromPreferences(Game.Data.Preferences);
    }
}
