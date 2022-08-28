using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chip8CSharp
{
    public partial class formScreen : Form
    {
        public formScreen()
        {
            InitializeComponent();
        }

        private void formScreen_Load(object sender, EventArgs e)
        {
            Timer timer = new Timer();
            timer.Interval = 1000 / Specs.REFRESH_RATE;
            timer.Tick += Emulator.GameLoop;    //Game loop will run at 60fps
            this.Paint += Emulator.DrawScreen;  //Already changed in the Designer

            Rom.LoadRom("ibm.ch8");
        }
    }
}
