using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Chip8CSharp
{
    public static class Emulator
    {
        public static void DrawScreen()
        {
            for (int i = 0; i < Specs.DISPLAY_HEIGHT; i++)
                for (int j = 0; j < Specs.DISPLAY_WIDTH; j++)
                    if(Specs.display[i, j] != 0x0)
                        DrawPixel(i*Specs.DISPLAY_SCALE, j*Specs.DISPLAY_SCALE);
        }

        public static void DrawPixel(int y, int x)
        {
            GL.Begin(PrimitiveType.QuadsExt);
            GL.Color3(Color.Purple);
            GL.Vertex2(x, y);
            GL.Vertex2(x + Specs.DISPLAY_SCALE, y);
            GL.Vertex2(x + Specs.DISPLAY_SCALE, y + Specs.DISPLAY_SCALE);
            GL.Vertex2(x, y + Specs.DISPLAY_SCALE);
            GL.End();
        }

        public static void GameLoop()
        {
            //Since the screen updates at 60Hz during each Hz we run 11 times
            //this equates to around 700 instructions per second
            //while maintaining the original 60Hz screen refresh rate
            //Eventually this can be made configurable
            for (int i = 0; i < 11; i++)
            {
                Specs.Fetch();

                Specs.Decode();

                Specs.ProcessOpcode();
            }

            //The decrements are outside the main loop so that they decrease 60 times per second
            Specs.DecrementDelay();
            Specs.DecrementSound();
        }
    }
}
