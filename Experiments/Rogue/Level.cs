namespace Rogue
{
  internal class Level
  {
    public int Height;
    public int Width;
    public Tile[,] Map;
    public bool[,] Visible;

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
      int startRow = player.CurrentRow - player.VisionRange;
      int startCol = player.CurrentCol - player.VisionRange;
      int endRow = player.CurrentRow + player.VisionRange;
      int endCol = player.CurrentCol + player.VisionRange;

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

    //public void MovePlayer(Direction direction)
    //{
    //  switch (direction)
    //  {
    //    case Direction.North:
    //      Player.Move(this, -1, 0);
    //      break;
    //    case Direction.South:
    //      Player.Move(this, 1, 0);
    //      break;
    //    case Direction.West:
    //      Player.Move(this, 0, -1);
    //      break;
    //    case Direction.East:
    //      Player.Move(this, 0, 1);
    //      break;
    //    case Direction.Northeast:
    //      Player.Move(this, -1, 1);
    //      break;
    //    case Direction.Southeast:
    //      Player.Move(this, 1, 1);
    //      break;
    //    case Direction.Northwest:
    //      Player.Move(this, -1, -1);
    //      break;
    //    case Direction.Southwest:
    //      Player.Move(this, 1, -1);
    //      break;
    //  }
    //}
  }
}
