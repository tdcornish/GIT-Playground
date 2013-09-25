using System;

namespace Rogue
{
  internal class Tile
  {
    public Tile(TileType type)
    {
      Symbol = (char)type;
      Type = type;
      Visible = true;
      MakeTile(type);
    }

    public char Symbol { get; private set; }
    public bool IsPassable { get; private set; }
    public ConsoleColor Color { get; private set; }
    public bool Visible { get; set; }
    public TileType Type { get; set; }

    private void MakeTile(TileType type)
    {
      switch (type)
      {
        case TileType.Unused:
          IsPassable = false;
          Color = ConsoleColor.Black;
          break;
        case TileType.DirtWall:
          IsPassable = false;
          Color = ConsoleColor.White;
          break;
        case TileType.DirtFloor:   //Corrider as well, can't have two a different case becaues they have the same label
          IsPassable = true;
          Color = ConsoleColor.White;
          break;
        case TileType.Door:
        case TileType.Upstairs:
        case TileType.Downstairs:
        case TileType.Chest:
          IsPassable = true;
          Color = ConsoleColor.DarkYellow;
          break;
      }
    }
  }

  internal enum TileType
  {
    Unused = ' ',
    DirtWall = '#',
    DirtFloor = '.',
    Corrider = '.',
    Door = '/',
    Upstairs = '>',
    Downstairs = '<',
    Chest = '*'
  }
}
