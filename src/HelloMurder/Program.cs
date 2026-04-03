using Murder;
using Murder.Diagnostics;

namespace HelloMurder
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            // so mac is happy with us.
            Environment.SetEnvironmentVariable("FNA_GRAPHICS_ENABLE_HIGHDPI", "1");

            using Game game = new(new HelloMurderGame());
            game.Run();
        }
    }
}
