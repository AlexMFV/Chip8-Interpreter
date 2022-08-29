using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chip8CSharp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var window = new Window(Specs.DISPLAY_WIDTH*Specs.DISPLAY_SCALE,
                Specs.DISPLAY_HEIGHT*Specs.DISPLAY_SCALE, "Chip-8 Emulator"))
            {
                window.VSync = OpenTK.VSyncMode.Off;
                window.Run(Specs.REFRESH_RATE, Specs.REFRESH_RATE);
            }
        }
    }
}
