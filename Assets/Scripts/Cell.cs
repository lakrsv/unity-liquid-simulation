using UnityEngine;
using System.Collections;
using System;

[Flags]
public enum Direction
{
    None,
    Left,
    Right
}

public enum Type
{
    Solid = 0,
    Air = 1,
    Liquid = 2
}

public class Cell {

    //Each TileGrid 16x16 contains 4x4 cells.
    //Each LiquidCell is 4x4 pixels.

    //64 Cells in each TileGrid

    public Type CellType;
    public int Level;

    public Direction Direction;

    public bool NoCalc;

    public static int MaxLevel = 8;

    //public static Cell None = new Cell(Type.NotSolid, 0);

    public Cell(Type type, int level)
    {
        CellType = type;
        Level = level;
        NoCalc = false;
    }
}
