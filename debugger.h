#ifndef _DEBUG_
#define _DEBUG_

#define RAYGUI_STATIC
#include <stdlib.h>
#include "raygui.h"
#include "emul.h"

const short screenW = w * scaler;
const short screenH = h * scaler;

unsigned short *db_stack[n_lines];
unsigned short db_pc = 0x200;
unsigned short db_opcode = 0x0;
char buffer[4];
int instBuffer[10];
int regBuffer[10];
bool isLoop = false;
unsigned short prev_pc = 0x0;

void DrawDebugger();
void UpdateDebug();
void InitDebug();
char ConvertToChar(unsigned char num);

void InitDebug()
{
    for(int i = 0; i < n_lines; i++)
        db_stack[i] = 0x0;
}

void DrawDebugger()
{
    DrawRectangle(screenW, 0, debug_w, screenH, Fade(DARKPURPLE, 0.3f)); //Right Panel Base
    DrawRectangle(0, screenH, screenW + debug_w, debug_h, Fade(DARKBLUE, 0.3f)); //Bottom Panel Base

    if(!isLoop && pc >= db_pc + n_lines)
        db_pc = pc;
    else
        if(isLoop && pc > prev_pc)
        {
            isLoop = false;
            if(pc > db_pc + n_lines)
                db_pc = pc;
        }

    for (int i = 0; i < n_lines; i++)
    {
        //if (pc <= 0x201)
        //    db_pc = pc;
        //else
        //    db_pc = pc - 0x2;

        db_opcode = memory[db_pc + (i * 2)] << 8 | memory[db_pc + 1 + (i * 2)];

        if(db_pc + (i * 2) == pc && db_opcode >> 12 == 0x1)
        {
            if(!isLoop && (db_opcode & 0x0fff) < pc)
            {
                isLoop = true;
                db_pc = (db_opcode & 0x0fff);
                prev_pc = pc;
            }
        }

        if (db_pc + (i * 2) == pc)
            DrawRectangle(screenW + 6, top_offset + (font_size * i), debug_w - 12, font_size - 1, RED);

        buffer[0] = ConvertToChar((db_opcode >> 12));
        buffer[1] = ConvertToChar((db_opcode & 0xf00) >> 8);
        buffer[2] = ConvertToChar((db_opcode & 0xf0) >> 4);
        buffer[3] = ConvertToChar((db_opcode & 0xf));

        itoa(db_pc + (i * 2), instBuffer, 16);
        strcat(instBuffer, " - ");
        strcat(instBuffer, buffer);

        DrawText(instBuffer, screenW + 10, top_offset + (font_size * i), font_size, WHITE);

        for(int i = 0; i < sizeof(v)/2; i++)
        {
            char extra[10];
            strcat(regBuffer, "#V");
            itoa(i, extra, 10);            
            strcat(regBuffer, extra);
            strcat(regBuffer, " - ");
            itoa(v[i], extra, 16);            
            strcat(regBuffer, extra);

            DrawText(regBuffer, 5, (screenH + 10) + (20*i), font_size, WHITE);

            memset(regBuffer, 0, sizeof regBuffer);
        }

        for(int i = 0; i < sizeof(v)/2; i++)
        {
            char extra[10];
            strcat(regBuffer, "#V");
            itoa(i+8, extra, 10);
            strcat(regBuffer, extra);
            strcat(regBuffer, " - ");
            itoa(v[i+8], extra, 16);
            strcat(regBuffer, extra);
            
            DrawText(regBuffer, 150, (screenH + 10) + (20*i), font_size, WHITE);

            memset(regBuffer, 0, sizeof regBuffer);
        }

        char extra[10];
        itoa(t_delay, extra, 10);
        strcat(regBuffer, "delay: ");
        strcat(regBuffer, extra);
        DrawText(regBuffer, 300, (screenH + 10), font_size, WHITE);
        memset(regBuffer, 0, sizeof regBuffer);

        itoa(t_sound, extra, 10);
        strcat(regBuffer, "sound: ");
        strcat(regBuffer, extra);
        DrawText(regBuffer, 300, (screenH + 30), font_size, WHITE);
        memset(regBuffer, 0, sizeof regBuffer);

        memset(instBuffer, 0, sizeof instBuffer);
        memset(buffer, 0, sizeof buffer);
    }

    //Normal Text Renderer
    //for(int i = 0; i < n_lines; i++)
    //{
    //    if(pc <= 0x201)
    //        db_pc = pc;
    //    else
    //        db_pc = pc - 0x2;
    //
    //    db_opcode = memory[db_pc + (i*2)] << 8 | memory[db_pc + 1 + (i*2)];
    //
    //    if(db_pc + (i*2) == pc)
    //        DrawRectangle(screenW + 6, top_offset + (font_size * i), debug_w - 12, font_size-1, RED);
    //
    //    buffer[0] = ConvertToChar((db_opcode >> 12));
    //    buffer[1] = ConvertToChar((db_opcode & 0xf00) >> 8);
    //    buffer[2] = ConvertToChar((db_opcode & 0xf0) >> 4);
    //    buffer[3] = ConvertToChar((db_opcode & 0xf));
    //
    //    itoa(db_pc + (i*2), instBuffer, 10);
    //    strcat(instBuffer," - ");
    //    strcat(instBuffer, buffer);
    //
    //    DrawText(instBuffer, screenW + 10, top_offset + (font_size * i), font_size, WHITE);
    //
    //    memset(instBuffer, 0, sizeof instBuffer);
    //    memset(buffer, 0, sizeof buffer);
    //}
}

void UpdateDebug()
{

}

char ConvertToChar(unsigned char num)
{
    switch (num)
    {
        case 0: return '0'; break;
        case 1: return '1'; break;
        case 2: return '2'; break;
        case 3: return '3'; break;
        case 4: return '4'; break;
        case 5: return '5'; break;
        case 6: return '6'; break;
        case 7: return '7'; break;
        case 8: return '8'; break;
        case 9: return '9'; break;
        case 10: return 'A'; break;
        case 11: return 'B'; break;
        case 12: return 'C'; break;
        case 13: return 'D'; break;
        case 14: return 'E'; break;
        case 15: return 'F'; break;
    }
}

#endif