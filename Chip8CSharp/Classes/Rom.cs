﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8CSharp
{
    public static class Rom
    {
        public static void LoadRom(string filename)
        {
            string fullPath = Path.Combine(Environment.CurrentDirectory, "Roms", filename);
            if (File.Exists(fullPath))
                Specs.rom = File.ReadAllBytes(fullPath);
        }

        public static void LoadRom(string name, string path)
        {
            throw new NotImplementedException();
        }
    }
}