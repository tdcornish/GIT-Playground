using System.Collections.Generic;

namespace Rogue
{
  internal class Level
  {
    public int Depth;
    public Point DownstairsLocation;
    public int Height;
    public Tile[,] Map;

    public Point UpstairsLocation;
    public bool[,] Visible;
    public int Width;
    public List<Item> ItemsOnFloor;

    public Level(int width, int height)
    {
      Width = width;
      Height = height;
      Map = new Tile[Height, Width];
      ItemsOnFloor = new List<Item>();
      Visible = new bool[Height, Width];
      for (int row = 0; row < Height; row++)
      {
        for (int col = 0; col < Width; col++)
        {
          Visible[row, col] = false;
        }
      }
    }

    public void AddToItemList(Item item)
    {
      ItemsOnFloor.Add(item);
    }

    public void Set(int row, int col, TileType type)
    {
      var value = new Tile(type);
      Map[row, col] = value;
    }

    public Tile Get(int row, int col)
    {
      return Map[row, col];
    }

    public Tile Get(Point point)
    {
      return Get(point.Row, point.Col);
    }

    public bool IsVisible(int row, int col)
    {
      return Visible[row, col];
    }
  }
}
