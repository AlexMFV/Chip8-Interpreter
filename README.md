# Chip8-Interpreter
## Interpreter/Emulator for the Chip-8 developed in C

- Thanks to [Tobias V. Langhoff](https://github.com/tobiasvl)  for this [awesome guide](https://tobiasvl.github.io/blog/write-a-chip-8-emulator/), used as a resource in the development of this project.

## Project Status

| Implemented | Instruction | Definition |
| :---: | :---: | --- |
| ❌ | 0NNN | Execute machine language routine |
| ✅ | 00E0 | Clear the screen |
| ✅ | 1NNN | Jump to memory location (**NNN**) with program counter (**PC**) |
| ❌ | 00EE | Pops the first memory location in the stack to the **PC** |
| ❌ | 2NNN | Pushes the **NNN** memory address to the stack |
| ✅ | 3XNN | Skips one instruction if **v[x]** is equal to **NN** |
| ✅ | 4XNN | Skips one instruction if **v[x]** is not equal to **NN** |
| ✅ | 5XY0 | Skips one instruction if **v[x]** is equal to **v[y]** |
| ❌ | 9XY0 | Skips one instruction if **v[x]** is not equal to **v[y]** |
| ✅ | 6XNN | Sets the register **v[x]** to the value of **NN** |
| ✅ | 7XNN | Add the value of **NN** to **v[x]** |
| ✅ | 8XY0 | Sets the register **v[x]** to the value of **v[y]** |
| ✅ | 8XY1 | Sets the register **v[x]** to the value of Bitwise OR of **v[x]** and **v[y]** |
| ✅ | 8XY2 | Sets the register **v[x]** to the value of Bitwise AND of **v[x]** and **v[y]** |
| ✅ | 8XY3 | Sets the register **v[x]** to the value of Bitwise XOR of **v[x]** and **v[y]** |
| ✅ | 8XY4 | Sets the register **v[x]** to the value of **v[x]** plus the value of **v[y]** |
| ✅ | 8XY5 |  |
| ✅ | 8XY7 |  |
| ❌ | 8XY6 |  |
| ❌ | 8XYE |  |
| ✅ | ANNN | Sets the value of 'I' to **NNN** |
| ❌ | BNNN |  |
| ❌ | CXNN |  |
| ✅ | DXYN | Displays and draws to the screen |
| ❌ | EX9E |  |
| ❌ | EXA1 |  |
| ❌ | FX07 |  |
| ❌ | FX15 |  |
| ❌ | FX18 |  |
| ❌ | FX1E |  |
| ❌ | FX0A |  |
| ❌ | FX29 |  |
| ❌ | FX33 |  |
| ❌ | FX55 |  |
| ❌ | FX65 |  |