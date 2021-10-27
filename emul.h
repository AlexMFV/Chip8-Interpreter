#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "screen.h"
#include <time.h>
#include <conio.h>

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

unsigned char stack_trace = 0x0;

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
    srand(time(NULL)); //Initializes the random component
}

void C8Fetch()
{
    opcode = memory[pc] << 8 | memory[pc + 1];
    pc += 2;
}

void C8Decode()
{
    //00E0 - Clear the screen
    if(opcode == 0x00e0) //0xe0 for short
    {
        memset(display, false, sizeof display);
        return;
    }

    //00EE - Pops the first memory address from the stack
    //if(opcode == 0x00ee)
    //{
    //    pc = stack[0x0];
//
    //    unsigned char aux = 0x0;
    //    while(aux < sizeof stack - 1)
    //    {
    //        stack[aux] = stack[aux+1];
    //        aux++;
    //    }
//
    //    stack_trace--;
    //    return;
    //}

    //1NNN - Set PC to NNN memory location
    if(opcode >> 12 == 0x1)
    {
        pc = (opcode & 0x0fff);
        return;
    }

    //2NNN - Set PC to NNN memory location
    //if(opcode >> 12 == 0x2)
    //{
    //    stack[stack_trace] = (opcode & 0x0fff);
    //    stack_trace++;
    //    return;
    //}

    //3XNN - Skips one instruction if v[x] == NN
    if(opcode >> 12 == 0x3 && v[(opcode & 0xf00) >> 8] == (opcode & 0xff))
    {
        pc += 2;
        return;
    }
    
    //4XNN - Skips one instruction if v[x] != NN
    if(opcode >> 12 == 0x4 && v[(opcode & 0xf00) >> 8] != (opcode & 0xff))
    {
        pc += 2;
        return;
    }
    
    //5XY0 - Skips one instruction if v[x] == v[y]
    if(opcode >> 12 == 0x5 && v[(opcode & 0xf00) >> 8] == v[(opcode & 0xf0) >> 4])
    {
        pc += 2;
        return;
    }

    //9XY0 - Skips one instruction if v[x] != v[y]
    if(opcode >> 12 == 0x9 && v[(opcode & 0xf00) >> 8] != v[(opcode & 0xf0) >> 4])
    {
        pc += 2;
        return;
    }

    //6XNN - Set the register V[X] to NN
    if(opcode >> 12 == 0x6)
    {
        v[(opcode & 0xf00) >> 8] = (opcode & 0xff);
        return;
    }

    //7XNN - Add the value NN to V[X]
    if(opcode >> 12 == 0x7)
    {
        v[(opcode & 0xf00) >> 8] += (opcode & 0xff);
        return;
    }

    //8XY0 - Set v[x] to the value of v[y]
    if(opcode >> 12 == 0x8)
    {
        v[(opcode & 0xf00) >> 8] = v[(opcode & 0xf0) >> 4];
        return;
    }

    //8XY1 - Set v[x] to the value of v[x] | v[y]
    if(opcode >> 12 == 0x8)
    {
        v[(opcode & 0xf00) >> 8] = (v[(opcode & 0xf00) >> 8] | v[(opcode & 0xf0) >> 4]);
        return;
    }

    //8XY2 - Set v[x] to the value of v[x] & v[y]
    if(opcode >> 12 == 0x8)
    {
        v[(opcode & 0xf00) >> 8] = (v[(opcode & 0xf00) >> 8] & v[(opcode & 0xf0) >> 4]);
        return;
    }

    //8XY3 - Set v[x] to the value of v[x] ^ v[y]
    if(opcode >> 12 == 0x8)
    {
        v[(opcode & 0xf00) >> 8] = (v[(opcode & 0xf00) >> 8] ^ v[(opcode & 0xf0) >> 4]);
        return;
    }

    //8XY4 - Set v[x] to the value of v[x] + v[y]
    if(opcode >> 12 == 0x8)
    {
        if((v[(opcode & 0xf00) >> 8] + v[(opcode & 0xf0) >> 4]) > 255)
            v[0xf] = 0x1;
        else
            v[0xf] = 0x0;
        v[(opcode & 0xf00) >> 8] = (v[(opcode & 0xf00) >> 8] + v[(opcode & 0xf0) >> 4]);
        return;
    }

    //8XY5 - Set v[x] to the value of v[x] - v[y]. Affecting the v[0xf] flag
    if(opcode >> 12 == 0x8)
    {
        if(v[(opcode & 0xf00) >> 8] > v[(opcode & 0xf0) >> 4])
            v[0xf] = 0x1;
        else
            v[0xf] = 0x0;
        v[(opcode & 0xf00) >> 8] = (v[(opcode & 0xf00) >> 8] - v[(opcode & 0xf0) >> 4]);
        return;
    }

    //8XY7 - Set v[x] to the value of v[y] - v[x]. Affecting the v[0xf] flag
    if(opcode >> 12 == 0x8)
    {
        if(v[(opcode & 0xf0) >> 4] > v[(opcode & 0xf00) >> 8])
            v[0xf] = 0x1;
        else
            v[0xf] = 0x0;
        v[(opcode & 0xf00) >> 8] = (v[(opcode & 0xf0) >> 4] - v[(opcode & 0xf00) >> 8]);
        return;
    }

    //8XY6 and 8XYE ambiguous instruction, not done for now

    //ANNN - Set index of Ireg, to value of NNN
    if(opcode >> 12 == 0x0a)
    {
        ireg = (opcode & 0xfff);
        return;
    }

    //BNNN ambiguous instruction, not done for now

    //CXNN - Generate a random number between 0 and NN and do a (generatedNum & NN)
    if(opcode >> 12 == 0x0c)
    {
        unsigned char num = (rand() % ((opcode & 0xff) + 1));
        v[(opcode & 0xf00) >> 8] = (num & (opcode & 0xff));
        return;
    }

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
                if(display[x+(7-j)][y+i] == true && ((data >> j) & 0x01) == 0x1) //?
                {
                    display[x+(7-j)][y+i] = false;
                    v[0xf] = 0x1;
                }
                else
                {
                    if(display[x+(7-j)][y+i] == false && ((data >> j) & 0x01) == 0x1)
                        display[x+(7-j)][y+i] = true;
                }

                if(x+j+1 > 63)
                    break;
            }
            if(y+i+1 > 31)
                break;
        }
        return;
    }

    //FX07 - Sets register v[x] to the values of the delay timer
    if(opcode >> 12 == 0x0f && (opcode & 0xFF) == 0x07)
    {
        v[(opcode & 0xf00) >> 8] = t_delay;
        return;
    }

    //FX15 - Sets the delay timer to the value of v[x]
    if(opcode >> 12 == 0x0f && (opcode & 0xFF) == 0x0f)
    {
        t_delay = v[(opcode & 0xf00) >> 8];
        return;
    }

    //FX18 - Sets the delay timer to the value of v[x]
    if(opcode >> 12 == 0x0f && (opcode & 0xFF) == 0x24)
    {
        t_sound = v[(opcode & 0xf00) >> 8];
        return;
    }

    //FX1E - Add the value of v[x] to the index 'Ireg'
    if(opcode >> 12 == 0x0f && (opcode & 0xFF) == 0x1e)
    {
        ireg += v[(opcode & 0xf00) >> 8];
        //If ireg goes outside the addressing range
        if(ireg > 0x1000)
            v[0xf] = 0x1;
        return;
    }

    //FX0A - 'Blocks' the program and waits for a key press
    //if(opcode >> 12 == 0x0f && (opcode & 0xFF) == 0x1e)
    //{
    //    ireg += v[(opcode & 0xf00) >> 8];
    //    //If ireg goes outside the addressing range
    //    if(ireg > 0x1000)
    //        v[0xf] = 0x1;
    //}

    //FX29 - Sets the value of Ireg to the value of v[x] and points to the correct fonts address
    if(opcode >> 12 == 0x0f && (opcode & 0xFF) == 0x1d)
    {
        ireg = v[(opcode & 0xf00) >> 8] + 0x50;
        return;
    }
}

void C8Execute()
{
    
}