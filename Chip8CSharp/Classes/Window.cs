using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
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
            this.TargetUpdateFrequency = Specs.REFRESH_RATE;
            this.TargetUpdatePeriod = Specs.REFRESH_RATE / 1000;

            this.TargetRenderFrequency = this.TargetUpdateFrequency;
            this.TargetRenderPeriod = this.TargetUpdatePeriod;

            GL.Viewport(0, 0, Specs.DISPLAY_WIDTH * Specs.DISPLAY_SCALE, Specs.DISPLAY_HEIGHT * Specs.DISPLAY_SCALE);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, Specs.DISPLAY_WIDTH * Specs.DISPLAY_SCALE, Specs.DISPLAY_HEIGHT * Specs.DISPLAY_SCALE, 0.0, -1.0, 1.0);
            GL.MatrixMode(MatrixMode.Modelview);

            Rom.LoadRom("ibm.ch8");
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
