#include "raylib.h"
#include "fonts.h"
#include "screen.h"
#include "emul.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <conio.h>
#include "common.h"
#include "debugger.h"

#define RAYGUI_IMPLEMENTATION
#include "raygui.h"

int main() 
{
    InitWindow(screenWidth, screenHeight, "CHIP-8 Emulator");
    SetTargetFPS(60);

    ClearMemory();

    if(_debug)
        InitDebug();

    //Opens the ROM file (make this dynamic later inside "rom.h")
    FILE *fs;
    fs = fopen("ibm.ch8", "rb");
    LoadROM(fs);
    fclose(fs);

    InitializeDisplay();

    bool stopped = false; //No Action
    bool step = true;

    // Main game loop
    while (!WindowShouldClose())
    {
        ClearBackground(BLACK);
        GuiLoadStyle("cyber.rgs");

        if(IsKeyPressed(KEY_SPACE))
            stopped = !stopped;

        if(stopped)
        {
            if(IsKeyPressed(KEY_PERIOD)) //Forward
                step = true;
        }
        
        //int aux = 0;
        //while(aux < 11)
        //{
        //    aux++;
        //}

        if (_debug)
        {
            DrawDebugger();
            UpdateDebug();
        }

        if (!stopped)
        {
            C8Fetch();
            C8Decode();
        }
        else
        {
            if(stopped && step)
            {
                C8Fetch();
                C8Decode();
                step = false;
            }
        }

        BeginDrawing();
        DrawDisplay(); //Push the drawDisplay to C8Execute();

        if(t_delay > 0x0 && !stopped)
            t_delay -= 0x1;
        else
            if(t_delay > 0x0 && stopped && step)
            {
                t_delay -= 0x1;
                step = false;
            }

        EndDrawing();
    }

    CloseWindow();

    return 0;
}