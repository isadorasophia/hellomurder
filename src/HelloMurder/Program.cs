using Murder;
using Murder.Diagnostics;

namespace HelloMurder
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                using Game game = new(new HelloMurderGame());
                game.Run();
            }
            catch (Exception ex) when (GameLogger.CaptureCrash(ex)) { }
        }
    }
}
