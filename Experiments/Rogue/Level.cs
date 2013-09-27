using System.Linq;

namespace Rogue
{
  internal class Level
  {
    public int Height;
    public int Width;
    public Tile[,] Map;
    public bool[,] Visible;
    public Player Player;

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

    public void UpdateVisible()
    {
      int startRow = Player.CurrentRow - Player.VisionRange;
      int startCol = Player.CurrentCol - Player.VisionRange;
      int endRow = Player.CurrentRow + Player.VisionRange;
      int endCol = Player.CurrentCol + Player.VisionRange;

      startRow = startRow < 0 ? 0 : startRow;
      endRow = endRow >= Height ? Height - 1 : endRow;
      startCol = startCol < 0 ? 0 : startCol;
      endCol = endCol >= Width ? Width - 1 : endCol;

      for (int row = startRow; row < endRow; row++)
      {
        for (int col = startCol; col < endCol; col++)
        {
          if (CanSee(row, col))
          {
            Visible[row, col] = true;
          }
        }
      }
    }

    private bool CanSee(int row, int col)
    {
      var lineOfSight = Line.GetPointsOnLine(Player.CurrentRow, Player.CurrentCol, row, col).ToArray();
      var orderedLineOfSight = lineOfSight.OrderBy(p => Line.DistanceBetweenPoints(p, new Point(Player.CurrentRow, Player.CurrentCol)));
      
      Point firstWall = orderedLineOfSight.FirstOrDefault(p => Get(p).Type == TileType.DirtWall);
      if (firstWall != null)
      {
        Visible[firstWall.Row, firstWall.Col] = true;
      }

      foreach (Point point in orderedLineOfSight)
      {
        bool inVisionRadius = InVisionRadius(point);
        if (!inVisionRadius)
        {
          return false;
        }

        TileType tileType = Get(point).Type;
        if (tileType == TileType.DirtWall)
        {
          return false;
        }
      }
      return true;
    }

    private bool InVisionRadius(Point point)
    {
      double distance = Line.DistanceBetweenPoints(point, new Point(Player.CurrentRow, Player.CurrentCol));
      return distance <= Player.VisionRange;
    }

    public void SetPlayer()
    {
      Player = new Player
      {
        CurrentRow = Randomizer.GetRand(1, Height),
        CurrentCol = Randomizer.GetRand(1, Width - 1)
      };

      Tile playerStartTile = Get(Player.CurrentRow, Player.CurrentCol);
      while (playerStartTile.Type != TileType.DirtFloor && playerStartTile.Type != TileType.Corrider)
      {
        Player.CurrentRow = Randomizer.GetRand(1, Height);
        Player.CurrentCol = Randomizer.GetRand(1, Width - 1);
        playerStartTile = Get(Player.CurrentRow, Player.CurrentCol);
      }
    }

    public void MovePlayer(Direction direction)
    {
      switch (direction)
      {
        case Direction.North:
          Player.Move(this, -1, 0);
          break;
        case Direction.South:
          Player.Move(this, 1, 0);
          break;
        case Direction.West:
          Player.Move(this, 0, -1);
          break;
        case Direction.East:
          Player.Move(this, 0, 1);
          break;
        case Direction.Northeast:
          Player.Move(this, -1, 1);
          break;
        case Direction.Southeast:
          Player.Move(this, 1, 1);
          break;
        case Direction.Northwest:
          Player.Move(this, -1, -1);
          break;
        case Direction.Southwest:
          Player.Move(this, 1, -1);
          break;
      }
    }
  }
}
