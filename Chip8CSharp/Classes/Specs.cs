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
        public static bool[,] display = new bool[DISPLAY_WIDTH * DISPLAY_SCALE, DISPLAY_HEIGHT * DISPLAY_SCALE]; //64x32, scaled 640, 320

        //Counters and registers
        public static ushort pc = 0x200;
        public static uint ireg = 0x0;
        public static uint[] stack = new uint[16];
        public static byte delay_timer = 0x0; //Decreases by 1 at a rate of 60Hz (60 times per second) until 0
        public static byte sound_timer = 0x0; //Acts the same as delay_timer, but beeps when different than 0'
        public static byte[] v = new byte[16]; //Registers named from 0 to F (hex)
        public static uint opcode = 0x0;

        //Rom
        public static byte[] rom;

        //Helpers opcode (DXY0)
        /// <summary>
        /// First nibble of the opcode
        /// </summary>
        public static byte op_Pre = 0x0;

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

        #region Main Actions

        public static void Fetch()
        {
            opcode = (uint)(memory[pc] << 0x8 | memory[(pc + 0x1)]);
            pc += 0x2;
        }

        public static void Decode()
        {
            //Separate the opcode (DXY0) into "nibbles"s
        }

        public static void Execute()
        {

        }

        #endregion
    }
}