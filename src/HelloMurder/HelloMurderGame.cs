using Murder;

namespace HelloMurder
{
    /// <summary>
    /// <inheritdoc cref="IMurderGame"/>
    /// </summary>
    public class HelloMurderGame : IMurderGame
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Name => "HelloMurder";
    }
}
