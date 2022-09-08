using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8CSharp
{
    internal class Window : GameWindow
    {
        public Window(int width, int height, string title) : base(width, height, GraphicsMode.Default, title) { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.Viewport(0, 0, Specs.DISPLAY_WIDTH * Specs.DISPLAY_SCALE, Specs.DISPLAY_HEIGHT * Specs.DISPLAY_SCALE);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, Specs.DISPLAY_WIDTH * Specs.DISPLAY_SCALE, Specs.DISPLAY_HEIGHT * Specs.DISPLAY_SCALE, 0.0, -1.0, 1.0);
            GL.MatrixMode(MatrixMode.Modelview);

            Rom.LoadRom("spaceinvaders.ch8");
            Specs.BootSequence();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Number1: Specs.keyPressed = 0x1; break;
                case Key.Number2: Specs.keyPressed = 0x2; break;
                case Key.Number3: Specs.keyPressed = 0x3; break;
                case Key.Number4: Specs.keyPressed = 0xc; break;
                case Key.Q: Specs.keyPressed = 0x4; break;
                case Key.W: Specs.keyPressed = 0x5; break;
                case Key.E: Specs.keyPressed = 0x6; break;
                case Key.R: Specs.keyPressed = 0xd; break;
                case Key.A: Specs.keyPressed = 0x7; break;
                case Key.S: Specs.keyPressed = 0x8; break;
                case Key.D: Specs.keyPressed = 0x9; break;
                case Key.F: Specs.keyPressed = 0xe; break;
                case Key.Z: Specs.keyPressed = 0xa; break;
                case Key.X: Specs.keyPressed = 0x0; break;
                case Key.C: Specs.keyPressed = 0xb; break;
                case Key.V: Specs.keyPressed = 0xf; break;
            }

            if (Specs.keyPressed != 0xff)
                Specs.hasInput = true;
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Emulator.GameLoop();
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            Emulator.DrawScreen();

            this.SwapBuffers();
            base.OnRenderFrame(e);
        }
    }
}
