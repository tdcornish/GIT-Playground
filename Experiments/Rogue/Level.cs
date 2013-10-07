namespace Rogue
{
  internal class Level
  {
    public int Height;
    public int Width;
    public Tile[,] Map;
    public bool[,] Visible;

    public Point UpstairsLocation;
    public Point DownstairsLocation;
    public int Depth;

    public Level(int width, int height)
    {
      Width = width;
      Height = height;
      Map = new Tile[Height, Width];
      Visible = new bool[Height, Width];
      for (int row = 0; row < Height; row++)
      {
        for (int col = 0; col < Width; col++)
        {
          Visible[row, col] = false;
        }
      }
    }

    public void Set(int row, int col, TileType type)
    {
      Tile value = new Tile(type);
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

    public void UpdateVisible(Player player)
    {
      int startRow = player.Location.Row - player.VisionRange;
      int startCol = player.Location.Col - player.VisionRange;
      int endRow = player.Location.Row + player.VisionRange;
      int endCol = player.Location.Col + player.VisionRange;

      startRow = startRow < 0 ? 0 : startRow;
      endRow = endRow >= Height ? Height - 1 : endRow;
      startCol = startCol < 0 ? 0 : startCol;
      endCol = endCol >= Width ? Width - 1 : endCol;

      for (int row = startRow; row < endRow; row++)
      {
        for (int col = startCol; col < endCol; col++)
        {
          if (player.CanSee(row, col))
          {
            Visible[row, col] = true;
          }
        }
      }
    }
  }
}
