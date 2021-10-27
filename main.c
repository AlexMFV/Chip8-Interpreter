#include "raylib.h"
#include "fonts.h"
#include "screen.h"
#include "emul.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "common.h"
#include "debugger.h"

#define RAYGUI_IMPLEMENTATION
#include "raygui.h"

int main() 
{
    InitWindow(screenWidth, screenHeight, "CHIP-8 Emulator");
    SetTargetFPS(60);

    ClearMemory();

    //Opens the ROM file (make this dynamic later inside "rom.h")
    FILE *fs;
    fs = fopen("spaceinvaders.ch8", "rb");
    LoadROM(fs);
    fclose(fs);

    InitializeDisplay();

    // Main game loop
    while (!WindowShouldClose())
    {
        ClearBackground(BLACK);
        GuiLoadStyle("lavanda.rgs");
        
        int aux = 0;
        while(aux < 11)
        {
            C8Fetch();
            C8Decode();
            aux++;
        }

        BeginDrawing();

        C8Execute();
        DrawDisplay(); //Push the drawDisplay to C8Execute();

        if(_debug)
        {
            DrawDebugger();
            UpdateDebug();
        }

        EndDrawing();
    }

    CloseWindow();

    return 0;
}