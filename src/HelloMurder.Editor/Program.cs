using Murder.Editor;

namespace HelloMurder.Editor
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var editor = new Architect(new HelloMurderArchitect()))
            {
                editor.Run();
            }
        }
    }
}
