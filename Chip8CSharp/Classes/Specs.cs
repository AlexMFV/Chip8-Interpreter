using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8CSharp
{
    public static class Specs
    {
        //Constants
        public static int DISPLAY_WIDTH = 64;
        public static int DISPLAY_HEIGHT = 32;
        public static int DISPLAY_SCALE = 10;
        public static int REFRESH_RATE = 60;

        //Specifications
        public static byte[] memory = new byte[4096]; //4KB of RAM - char = 1 byte
        public static byte[,] display = new byte[DISPLAY_HEIGHT, DISPLAY_WIDTH]; //64x32, scaled 640, 320

        //Counters and registers
        public static ushort pc = 0x200;
        public static ushort ireg = 0x0;
        public static ushort[] stack = new ushort[16];
        public static short tracker = -0x1;
        public static byte delay_timer = 0x0; //Decreases by 1 at a rate of 60Hz (60 times per second) until 0
        public static byte sound_timer = 0x0; //Acts the same as delay_timer, but beeps when different than 0'
        public static byte[] v = new byte[16]; //Registers named from 0 to F (hex)
        public static ushort opcode = 0x0;

        //Rom
        public static byte[] rom;

        //Helpers opcode (DXY0)
        /// <summary>
        /// First nibble of the opcode
        /// </summary>
        public static byte op_Prefix = 0x0;

        /// <summary>
        /// Second nibble of the opcode
        /// </summary>
        public static byte op_X = 0x0;

        /// <summary>
        /// Third nibble of the opcode
        /// </summary>
        public static byte op_Y = 0x0;

        /// <summary>
        /// Forth and final nibble of the opcode
        /// </summary>
        public static byte op_N = 0x0;

        //Fonts
        public static byte[] font = new byte[80] {
            0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
            0x20, 0x60, 0x20, 0x20, 0x70, // 1
            0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
            0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
            0x90, 0x90, 0xF0, 0x10, 0x10, // 4
            0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
            0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
            0xF0, 0x10, 0x20, 0x40, 0x40, // 7
            0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
            0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
            0xF0, 0x90, 0xF0, 0x90, 0x90, // A
            0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
            0xF0, 0x80, 0x80, 0x80, 0xF0, // C
            0xE0, 0x90, 0x90, 0x90, 0xE0, // D
            0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
            0xF0, 0x80, 0xF0, 0x80, 0x80  // F
                                                };

        public static void ClearMemory()
        {
            for(int i = 0; i < memory.Length; i++)
                memory[i] = 0x0;
        }

        public static void ClearDisplay()
        {
            for (int i = 0; i < DISPLAY_HEIGHT; i++)
                for (int j = 0; j < DISPLAY_WIDTH; j++)
                    display[i,j] = 0x0;
        }

        #region Main Actions

        public static void Fetch()
        {
            opcode = (ushort)(memory[pc] << 0x8 | memory[(pc + 0x1)]);
            pc += 0x2;
        }

        public static void Decode()
        {
            //Separate the opcode (DXYN) into "nibbles"s
            op_Prefix = (byte)(opcode >> 12 & 0xf);
            op_X = (byte)(opcode >> 8 & 0xf);
            op_Y = (byte)(opcode >> 4 & 0xf);
            op_N = (byte)(opcode & 0xf);
        }

        public static void Execute()
        {

        }

        #endregion

        #region Opcodes

        public static void ProcessOpcode()
        {
            //0x00e0 - Clears the screen
            if (opcode == 0x00e0)
                ClearDisplay();

            //0x00ee - Pops the last item from the 'stack' and assigns 'PC' to it's value
            if (opcode == 0x00ee)
            {
                pc = stack[tracker];
                tracker--;
            }    

            switch (op_Prefix)
            {
                case 0x1: pc = (ushort)(opcode & 0xfff); break;
                case 0x6: v[op_X] = (byte)(opcode & 0xff); break;
                case 0x7: v[op_X] += (byte)(opcode & 0xff); break;
                case 0xa: ireg = (ushort)(opcode & 0xfff); break;
                case 0xd: ProcessDisplay(); break;
                default: break;
            }
        }

        public static void ProcessDisplay()
        {
            int x = v[op_X]; //% 64;
            int y = v[op_Y]; //% 32;
            v[0xf] = 0x0;

            for(int i = 0; i < op_N; i++)
            {
                byte data = memory[ireg + i];
                for(int inc = 0; inc < 8; inc++)
                {
                    if(((data >> inc) & 0x01) == 0x1 && display[y+i, x+ (7-inc)] == 0xff)
                    {
                        display[y+i, x+ (7-inc)] = 0x0;
                        v[0xf] = 0x1;
                    }
                    else
                    {
                        if (((data >> inc) & 0x01) == 0x1 && display[y+i, x+(7-inc)] == 0x0)
                            display[y+i, x+ (7-inc)] = 0xff;
                    }

                    if (x + inc > 63)
                        break;
                }

                if (y + i > 31)
                    break;
            }
        }

        #endregion
    }
}