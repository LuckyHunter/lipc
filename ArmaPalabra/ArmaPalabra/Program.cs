using System;

namespace ArmaPalabra
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using (MainScreen game = new MainScreen())
            {
                game.Run();
            }
        }
    }
#endif
}

