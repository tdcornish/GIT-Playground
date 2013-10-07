using System.Linq;
using SFML.Graphics;

namespace Rogue
{
  internal class Player
  {
    public Color Color;
    public Level CurrentLevel;
    public Point Location;
    public char Symbol;
    public int VisionRange;

    public Player(Level startLevel)
    {
      CurrentLevel = startLevel;
      Symbol = '@';
      VisionRange = 5;
      Color = new Color(Color.Red);

      SetStartingPosition();
    }

    private void SetStartingPosition()
    {
      Location = new Point(
        Randomizer.GetRand(1, CurrentLevel.Height - 1),
        Randomizer.GetRand(1, CurrentLevel.Width - 1)
        );

      Tile playerStartTile = CurrentLevel.Get(Location);
      while (playerStartTile.Type != TileType.DirtFloor &&
        playerStartTile.Type != TileType.Corrider)
      {
        Location.Row = Randomizer.GetRand(1, CurrentLevel.Height - 1);
        Location.Col = Randomizer.GetRand(1, CurrentLevel.Width - 1);
        playerStartTile = CurrentLevel.Get(Location);
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
      int newRow = Location.Row + deltaRow;
      int newCol = Location.Col + deltaCol;

      bool outOfBounds = newRow < 0 || newRow >= CurrentLevel.Height ||
        newCol < 0 || newCol >= CurrentLevel.Width;
      if (!outOfBounds)
      {
        Tile nextTile = CurrentLevel.Get(newRow, newCol);
        if (nextTile.IsPassable)
        {
          Location.Row = newRow;
          Location.Col = newCol;
        }

        if (nextTile.Type == TileType.ClosedDoor)
        {
          Location.Row = newRow;
          Location.Col = newCol;
          CurrentLevel.Set(newRow, newCol, TileType.OpenDoor);
        }
      }
    }

    public void ChangeLevel(Direction direction, Level[] dungeon)
    {
      switch (direction)
      {
        case Direction.Up:
          CurrentLevel = dungeon[CurrentLevel.Depth - 1];
          Location = CurrentLevel.DownstairsLocation;
          break;
        case Direction.Down:
          CurrentLevel = dungeon[CurrentLevel.Depth + 1];
          Location = CurrentLevel.UpstairsLocation;
          break;
      }
    }

    public bool CanSee(int row, int col)
    {
      Point[] lineOfSight =
        Line.GetPointsOnLine(Location, new Point(row, col)).ToArray();
      IOrderedEnumerable<Point> orderedLineOfSight =
        lineOfSight.OrderBy(p => Line.DistanceBetweenPoints(p, Location));

      Point firstBlocker =
        orderedLineOfSight.FirstOrDefault(
          p => Tile.VisionBlockers.Contains(CurrentLevel.Get(p).Type));
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
      double distance = Line.DistanceBetweenPoints(point, Location);
      return distance <= VisionRange;
    }

    private void SetVisible(Point point)
    {
      CurrentLevel.Visible[point.Row, point.Col] = true;
    }
  }
}
