#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "screen.h"

#define MEMSIZE 4096

void C8Fetch();
void C8Decode();
void C8Execute();
void LoadROM();

unsigned short pc = 0x200; //program counter
unsigned short ireg = 0x0;
unsigned char memory[MEMSIZE];
unsigned char v[0x10]; //16 registers from 0, 1, 2 ... C, D, F
unsigned short stack[0x10];
unsigned char t_sound = 0x0;
unsigned char t_delay = 0x0;
unsigned short opcode = 0x0;

void ClearMemory()
{
    //Initialize memory buffer
    for(int i = 0; i < 4096; i++)
        memory[i] = 0x0;
}

void InitializeFont()
{
    //Initialize the font to memory - 0x50 - 0x9f
    memcpy(memory+0x50, font, sizeof font);
}

void LoadROM(FILE *fs)
{
    //Reads the ROM to the memory starting at address 0x200 (512 dec) until the end of the memory
    fread(memory+0x200, 1, MEMSIZE-0x200, fs);
}

void C8Fetch()
{
    opcode = memory[pc] << 8 | memory[pc + 1];
    pc += 2;
}

void C8Decode()
{
    //Clear the screen
    if(opcode == 0x00e0) //0xe0 for short
        memset(display, false, sizeof display);

    //1NNN - Set PC to NNN memory location
    if(opcode >> 12 == 0x1)
        pc = (opcode & 0x0fff);

    //6XNN - Set the register V[X] to NN
    if(opcode >> 12 == 0x6)
        v[(opcode & 0xf00) >> 8] = (opcode & 0xff);

    //7XNN - Add the value NN to V[X]
    if(opcode >> 12 == 0x7)
        v[(opcode & 0xf00) >> 8] += (opcode & 0xff);

    //ANNN - Set index of Ireg, to value of NNN
    if(opcode >> 12 == 0x0a)
        ireg = (opcode & 0xfff);

    //DXYN - Draw sprites to screen
    if(opcode >> 12 == 0x0d)
    {
        int x = v[(opcode & 0xf00) >> 8];
        int y = v[(opcode & 0xf0) >> 4];
        int n = opcode & 0xf;
        v[0xf] = 0x0;

        for(int i = 0; i < n; i++)
        {
            unsigned char data = memory[ireg + i];
            for(int j = 0; j < 8; j++)
            {
                if(display[x+(8-j)][y+i] == true && ((data >> j) & 0x01) == 0x1) //?
                {
                    display[x+(8-j)][y+i] = false;
                    v[0xf] = 0x1;
                }
                else
                {
                    if(display[x+(8-j)][y+i] == false && ((data >> j) & 0x01) == 0x1)
                        display[x+(8-j)][y+i] = true;
                }

                if(x+j+1 > 63)
                    break;
            }
            if(y+i+1 > 31)
                break;
        }
    }
}

void C8Execute()
{
    
}