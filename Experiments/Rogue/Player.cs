using System.Linq;

namespace Rogue
{
  internal class Player
  {
    public int CurrentCol;
    public Level CurrentLevel;
    public int CurrentRow;
    public char Symbol;
    public int VisionRange;

    public Player(Level startLevel)
    {
      CurrentLevel = startLevel;
      Symbol = '@';
      VisionRange = 5;

      SetStartingPosition();
    }

    private void SetStartingPosition()
    {
      CurrentRow = Randomizer.GetRand(1, CurrentLevel.Height - 1);
      CurrentCol = Randomizer.GetRand(1, CurrentLevel.Width - 1);

      Tile playerStartTile = CurrentLevel.Get(CurrentRow, CurrentCol);
      while (playerStartTile.Type != TileType.DirtFloor && playerStartTile.Type != TileType.Corrider)
      {
        CurrentRow = Randomizer.GetRand(1, CurrentLevel.Height - 1);
        CurrentCol = Randomizer.GetRand(1, CurrentLevel.Width - 1);
        playerStartTile = CurrentLevel.Get(CurrentRow, CurrentCol);
      }
    }

    public void Move(Direction direction)
    {
      switch (direction)
      {
        case Direction.North:
          Move(-1, 0);
          break;
        case Direction.South:
          Move(1, 0);
          break;
        case Direction.West:
          Move(0, -1);
          break;
        case Direction.East:
          Move(0, 1);
          break;
        case Direction.Northeast:
          Move(-1, 1);
          break;
        case Direction.Southeast:
          Move(1, 1);
          break;
        case Direction.Northwest:
          Move(-1, -1);
          break;
        case Direction.Southwest:
          Move(1, -1);
          break;
      }
    }

    public void Move(int deltaRow, int deltaCol)
    {
      int newRow = CurrentRow + deltaRow;
      int newCol = CurrentCol + deltaCol;

      bool outOfBounds = newRow < 0 || newRow >= CurrentLevel.Height || newCol < 0 || newCol >= CurrentLevel.Width;
      if (!outOfBounds)
      {
        Tile nextTile = CurrentLevel.Get(newRow, newCol);
        if (nextTile.IsPassable)
        {
          CurrentRow = newRow;
          CurrentCol = newCol;
        }

        if (nextTile.Type == TileType.ClosedDoor)
        {
          CurrentRow = newRow;
          CurrentCol = newCol;
          CurrentLevel.Set(newRow, newCol, TileType.OpenDoor);
        }
      }
    }

    public bool CanSee(int row, int col)
    {
      Point[] lineOfSight = Line.GetPointsOnLine(CurrentRow, CurrentCol, row, col).ToArray();
      IOrderedEnumerable<Point> orderedLineOfSight =
        lineOfSight.OrderBy(p => Line.DistanceBetweenPoints(p, new Point(CurrentRow, CurrentCol)));

      Point firstBlocker = orderedLineOfSight.FirstOrDefault(p => Tile.VisionBlockers.Contains(CurrentLevel.Get(p).Type));
      if (firstBlocker != null)
      {
        SetVisible(firstBlocker);
      }

      foreach (Point point in orderedLineOfSight)
      {
        bool inVisionRadius = InVisionRadius(point);
        if (!inVisionRadius)
        {
          return false;
        }

        TileType tileType = CurrentLevel.Get(point).Type;
        if (Tile.VisionBlockers.Contains(tileType))
        {
          SetVisible(point);
          return false;
        }
      }
      return true;
    }

    private bool InVisionRadius(Point point)
    {
      double distance = Line.DistanceBetweenPoints(point, new Point(CurrentRow, CurrentCol));
      return distance <= VisionRange;
    }

    private void SetVisible(Point point)
    {
      CurrentLevel.Visible[point.Row, point.Col] = true;
    }
  }
}
