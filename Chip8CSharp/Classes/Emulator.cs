using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chip8CSharp
{
    public static class Emulator
    {
        public static void GameLoop(object sender, EventArgs e)
        {
            Specs.Fetch();
            Specs.Decode();
            Specs.Execute();
        }

        public static void DrawScreen(object sender, PaintEventArgs e)
        {

        }
    }
}
