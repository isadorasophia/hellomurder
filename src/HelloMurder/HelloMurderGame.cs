using Murder;
using Murder.Serialization;
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
}
