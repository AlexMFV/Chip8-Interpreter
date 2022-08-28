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
        public static bool hasInput = false;
        public static byte keyPressed = 0x0;

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

        public static void BootSequence()
        {
            ClearMemory();
            InstantiateFont();
            LoadRomToMemory();
            System.Threading.Timer t_delay = new System.Threading.Timer(DecrementDelay, null, 0, 1000 / REFRESH_RATE);
            System.Threading.Timer t_sound = new System.Threading.Timer(DecrementSound, null, 0, 1000 / REFRESH_RATE);
        }

        public static void DecrementDelay(object a)
        {
            if (delay_timer < 0)
                delay_timer = 0;

            if (delay_timer > 0)
                delay_timer--;
        }

        public static void DecrementSound(object a)
        {
            if(sound_timer < 0)
                sound_timer = 0;

            if (sound_timer > 0)
                sound_timer--;
        }

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

        public static void InstantiateFont()
        {
            for(int i = 0; i < font.Length; i++)
                memory[0x50+i] = font[i];
        }

        public static void LoadRomToMemory()
        {
            for(int i = 0; i < rom.Length; i++)
                memory[pc + i] = rom[i];
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

        #endregion

        #region Opcodes

        public static void ProcessOpcode()
        {
            Console.WriteLine("{0} - {1:X}", pc, opcode);

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
                case 0x1:
                    pc = (ushort)(opcode & 0xfff);
                    break;

                case 0x2:
                    tracker++;
                    stack[tracker] = pc;
                    pc = (ushort)(opcode & 0xfff);
                    break;

                case 0x3:
                    if (v[op_X] == (ushort)(opcode & 0xff))
                        pc += 0x2;
                    break;

                case 0x4:
                    if (v[op_X] != (ushort)(opcode & 0xff))
                        pc += 0x2;
                    break;

                case 0x5:
                    if (v[op_X] == v[op_Y])
                        pc += 0x2;
                    break;

                case 0x6:
                    v[op_X] = (byte)(opcode & 0xff);
                    break;

                case 0x7:
                    v[op_X] += (byte)(opcode & 0xff);
                    break;

                case 0x8:
                    switch (op_N)
                    {
                        case 0x0: v[op_X] = v[op_Y]; break; //v[x] = v[y]
                        case 0x1: v[op_X] = (byte)(v[op_X] | v[op_Y]); break; //OR gate
                        case 0x2: v[op_X] = (byte)(v[op_X] & v[op_Y]); break; //AND gate
                        case 0x3: v[op_X] = (byte)(v[op_X] ^ v[op_Y]); break; //XOR gate
                        case 0x4:
                            if (v[op_X] + v[op_Y] > 0xff)
                                v[0xf] = 0x1;
                            else
                                v[0xf] = 0x0;
                            v[op_X] += v[op_Y];
                            break; //Add
                        case 0x5:
                            if (v[op_X] > v[op_Y])
                                v[0xf] = 0x1;
                            else
                                v[0xf] = 0x0;

                            v[op_X] -= v[op_Y];
                            break; //Subtract
                        case 0x6: break;
                        case 0x7:
                            if (v[op_Y] > v[op_X])
                                v[0xf] = 0x1;
                            else
                                v[0xf] = 0x0;

                            v[op_X] = (byte)(v[op_Y] - v[op_X]);
                            break; //Subtract
                        case 0xe: break;
                        default: break;
                    }
                    break;

                case 0x9:
                    if (v[op_X] != v[op_Y])
                        pc += 0x2;
                    break;

                case 0xa:
                    ireg = (ushort)(opcode & 0xfff);
                    break;

                case 0xc:
                    Random rand = new Random(); //Maybe declare this as global, as to not instantiate it every time
                    int number = rand.Next(0, (opcode & 0xff));
                    v[op_X] = (byte)(number & (opcode & 0xff));
                    break;

                case 0xd:
                    ProcessDisplay();
                    break;

                case 0xf:
                    switch(opcode & 0xff)
                    {
                        case 0x07: v[op_X] = delay_timer; break; //Needs to be decreased
                        case 0x15: delay_timer = v[op_X]; break;
                        case 0x18: sound_timer = v[op_Y]; break;
                        case 0x1e:
                            ireg += v[op_X];
                            if (ireg > 0xfff)
                                v[0xf] = 0x1;
                            break;
                        case 0x0a:
                            if (!hasInput)
                            {
                                pc -= 0x2;
                                break;
                            }
                            v[op_X] = keyPressed; //Hex value
                            break;
                        case 0x29:
                            ireg += (ushort)(v[op_X] + 0x50);
                            break;
                        case 0x33: break;
                        case 0x55: break;
                        case 0x65: break;
                    }
                    break;

                default: break;
            }
            hasInput = false;
            keyPressed = 0x0;
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