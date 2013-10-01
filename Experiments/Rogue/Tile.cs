using System;
using System.Collections.Generic;
using SFML.Graphics;

namespace Rogue
{
  internal class Tile
  {
    public static List<TileType> VisionBlockers = new List<TileType>
    {
      TileType.DirtWall, 
      TileType.ClosedDoor
    };

    public Tile(TileType type)
    {
      Symbol = (char)type;
      Type = type;
      MakeTile(type);
    }

    public char Symbol { get; private set; }
    public bool IsPassable { get; private set; }
    public Color Color { get; private set; }
    public TileType Type { get; set; }

    private void MakeTile(TileType type)
    {
      switch (type)
      {
        case TileType.Unused:
          IsPassable = false;
          Color = Color.Black;
          break;
        case TileType.DirtWall:
          IsPassable = false;
          Color = Color.White;
          break;
        case TileType.DirtFloor:   //Corrider as well, can't have two a different case becaues they have the same label
          IsPassable = true;
          Color = Color.White;
          break;
        case TileType.ClosedDoor:
          IsPassable = false;
          Color = Color.Yellow;
          break;
        case TileType.OpenDoor:
        case TileType.Upstairs:
        case TileType.Downstairs:
        case TileType.Chest:
          IsPassable = true;
          Color = Color.Yellow;
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
    ClosedDoor = 'D',
    OpenDoor = '/',
    Upstairs = '>',
    Downstairs = '<',
    Chest = '*'
  }
}
