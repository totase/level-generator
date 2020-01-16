# Level generator

A simple level generator made for Unity, inspired by the level generation of [Spelunky](https://spelunkyworld.com/original.html). It generates a number of rooms based on a grid size, made up of x rows and y columns.

## How it works

The `LevelManager` script generates a grid based on the `rows` and `columns` variables. The `LevelSetup` function will then cycle through each row and column, and place rooms until the bottom is reached.

For each room, a new direction will be given (left, right or down). If the current room is on the edge of the level board, the new room will automatically be placed below.

### Rooms

The position for each room will be calculated by the `roomHeight` and `roomWidth` variables.

_Note_: Since the rooms in this repository is static sprites, editing these variables will produce gaps between each rooms.

### Demo

In the demo below, level are generated on a 3x3 grid, with 6x8 rooms.

![Generation demo demo](./docs/demo.gif "Demo of the level generation")

### Starting project

The project doesn't require any setup, although it may not work properly with newer versions of Unity.
Tested with version 2019.2.9f1 Personal.
