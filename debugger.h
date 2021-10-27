#define RAYGUI_STATIC
#include "raygui.h"
#include "common.h"

const short screenW = w * scaler;
const short screenH = h * scaler;

const char *listViewExList[8] = { "This", "is", "a", "list view", "with", "disable", "elements", "amazing!" };

void DrawDebugger();
void UpdateDebug();

void DrawDebugger()
{
    DrawRectangle(screenW, 0, debug_w, screenH, Fade(DARKPURPLE, 0.3f)); //Right Panel Base
    DrawRectangle(0, screenH, screenW + debug_w, debug_h, Fade(DARKBLUE, 0.3f)); //Bottom Panel Base

    GuiListView((Rectangle){ screenW + 10, 10, debug_w - 20, screenH - 20 }, listViewExList, 0, 1);
}

void UpdateDebug()
{

}