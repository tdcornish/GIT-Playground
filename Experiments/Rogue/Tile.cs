using System.Collections.Generic;

namespace Rogue
{
  internal class Tile
  {
    public static List<TileType> VisionBlockers = new List<TileType>
    {
      TileType.DirtWall,
      TileType.ClosedDoor
    };

    public bool IsPassable;
    public Item Item;
    public char Symbol;
    public TileType Type;

    public Tile(TileType type)
    {
      Symbol = (char)type;
      Type = type;
      SetTileProperties(type);
      Item = null;
    }

    public bool HasItem()
    {
      return Item != null;
    }

    private void SetTileProperties(TileType type)
    {
      switch (type)
      {
        case TileType.Unused:
        case TileType.DirtWall:
        case TileType.ClosedDoor:
          IsPassable = false;
          break;
        case TileType.DirtFloor:
        case TileType.OpenDoor:
        case TileType.Upstairs:
        case TileType.Downstairs:
          IsPassable = true;
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
    ClosedDoor = '+',
    OpenDoor = '/',
    Upstairs = '<',
    Downstairs = '>',
  }
}
