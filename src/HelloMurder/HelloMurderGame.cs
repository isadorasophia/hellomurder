using Bang;
using Microsoft.Xna.Framework.Graphics;
using Murder;
using Murder.Core.Geometry;
using Murder.Core.Graphics;
using System.Text.Json;
using Murder.Serialization;
using Microsoft.Xna.Framework.Input;

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

    public void Initialize()
    {
        Game.Input.RegisterButton(HelloMurder.Core.InputButtons.Submit, Keys.Enter);
        Game.Input.RegisterButton(HelloMurder.Core.InputButtons.Cancel, Keys.Escape);
    }

    public JsonSerializerOptions Options => HelloMurderSerializerOptionsExtensions.Options;

    public ComponentsLookup ComponentsLookup => new HelloMurderComponentsLookup();

    public RenderContext CreateRenderContext(
        GraphicsDevice graphicsDevice,
        Camera2D camera,
        RenderContextFlags settings
    )
    {
        RenderContext renderContext = new(graphicsDevice, camera, settings);

        WindowChangeSettings windowChangeSettings = new(
            new Point(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height)
        );

        renderContext.OnClientWindowChanged(windowChangeSettings);

        return renderContext;
    }

}
