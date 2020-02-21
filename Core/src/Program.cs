using System;

namespace MegamanX
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MegamanX())
                game.Run();
        }
    }
}
