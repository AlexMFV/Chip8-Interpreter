#ifndef HEADER_SCREEN
#define HEADER_SCREEN

#include <stdio.h>

#define w 64
#define h 32

void DrawDisplay();
void InitializeDisplay();

const short scaler = 10;
const short screenWidth = w * scaler;    //640
const short screenHeight = h * scaler;   //320
bool display[w][h];

void DrawDisplay()
{
    unsigned short _h = 0;
    while(_h < h)
    {
        unsigned short _w = 0;
        while(_w < w)
        {
            //Pixel Scaler
            unsigned short _hAux = 0;
            while(_hAux < scaler)
            {
                unsigned short _wAux = 0;
                while(_wAux < scaler)
                {
                    DrawPixel(_w*scaler+_wAux, _h*scaler+_hAux, display[_w][_h] == false ? BLACK : WHITE);
                    _wAux++;
                }
                _hAux++;
            }
            _w++;
        }
        _h++;
    }
}

void InitializeDisplay()
{
    //Initialize screen with turned off pixels
    for(int i = 0; i < h; i++)
        for(int j = 0; j < w; j++)
            display[j][i] = false;
}

#endif