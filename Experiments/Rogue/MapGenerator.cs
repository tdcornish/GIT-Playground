using System;
using System.Collections.Generic;
using System.Linq;

namespace Rogue
{
  internal class MapGenerator
  {
    private const int RoomChance = 65;

    private int Objects;
    private Level Level;

    public Level GenerateLevel(int width, int height, int numberOfObjects)
    {
      Objects = numberOfObjects;
      Level = new Level(width, height);

      Init();
      MakeRoom(height/2, width/2, 5, 8, Randomizer.RandomDirection());
      int currentFeatures = 1;
      TryBuildFeatures(currentFeatures);

      AddStairs(TileType.Upstairs);
      AddStairs(TileType.Downstairs);

      return Level;
    }

    private void TryBuildFeatures(int currentFeatures)
    {
      for (int countingTries = 0; countingTries < 1000; countingTries++)
      {
        if (currentFeatures == Objects)
        {
          break;
        }

        int row = 0;
        int rowMod = 0;
        int col = 0;
        int colMod = 0;
        Direction? directionToBuild = null;

        FindLocationToBuildAt(ref row, ref col, ref rowMod, ref colMod, ref directionToBuild);
        currentFeatures = BuildFeature(directionToBuild, row, rowMod, col, colMod, currentFeatures);
      }
    }

    private void FindLocationToBuildAt(ref int row, ref int col, ref int rowMod, ref int colMod, ref Direction? directionToBuild)
    {
      for (int testing = 0; testing < 1000; testing++)
      {
        row = Randomizer.GetRand(1, Level.Height - 1);
        col = Randomizer.GetRand(1, Level.Width - 1);

        TileType testTile = Level.Get(row, col).Type;
        if (testTile == TileType.DirtWall || testTile == TileType.DirtFloor)
        {
          IEnumerable<Tuple<Point, Direction, Tile>> surroundings = GetSurroundings(new Point(row, col));

          Tuple<Point, Direction, Tile> canReach = surroundings.FirstOrDefault(s => s.Item3.IsPassable);
          if (canReach == null)
          {
            continue;
          }

          Direction canReachFrom = canReach.Item2;
          switch (canReachFrom)
          {
            case Direction.North:
              rowMod = 1;
              colMod = 0;
              directionToBuild = Direction.South;
              break;
            case Direction.East:
              rowMod = 0;
              colMod = -1;
              directionToBuild = Direction.West;
              break;
            case Direction.South:
              rowMod = -1;
              colMod = 0;
              directionToBuild = Direction.North;
              break;
            case Direction.West:
              rowMod = 0;
              colMod = 1;
              directionToBuild = Direction.East;
              break;
            default:
              throw new InvalidOperationException();
          }

          IEnumerable<Tuple<Point, Direction, Tile>> adjacentPoints = GetSurroundings(new Point(row, col));
          bool adjacentDoor = adjacentPoints.Any(p => p.Item3.Type == TileType.ClosedDoor);
          if (adjacentDoor)
          {
            directionToBuild = null;
          }

          break;
        }
      }
    }

    private int BuildFeature(Direction? directionToBuild, int row, int rowMod, int col, int colMod, int currentFeatures)
    {
      if (directionToBuild.HasValue)
      {
        int feature = Randomizer.GetRand(0, 100);
        if (feature <= RoomChance)
        {
          if (MakeRoom(row + rowMod, col + colMod, 8, 6, directionToBuild.Value))
          {
            currentFeatures++;
            Level.Set(row, col, TileType.ClosedDoor);
            Level.Set(row + rowMod, col + colMod, TileType.DirtFloor);
          }
        }
        else if (feature >= RoomChance)
        {
          if (MakeCorridor(row + rowMod, col + colMod, 6, directionToBuild.Value))
          {
            currentFeatures++;
            Level.Set(row, col, TileType.ClosedDoor);
          }
        }
      }
      return currentFeatures;
    }

    private void Init()
    {
      Level.Map = new Tile[Level.Height, Level.Width];
      for (int row = 0; row < Level.Height; row++)
      {
        for (int col = 0; col < Level.Width; col++)
        {
          Level.Map[row, col] = new Tile(TileType.Unused);
        }
      }
    }

    private void AddStairs(TileType stair)
    {
      for (int i = 0; i < 1000; i++)
      {
        int row = Randomizer.GetRand(1, Level.Height);
        int col = Randomizer.GetRand(1, Level.Width);

        var surroundingPoints = GetSurroundingPoints(new Point(row, col));
        int ways =
          surroundingPoints.Select(point => Level.Get(point.Item1))
                           .Where(tile => tile.Type == TileType.DirtFloor || tile.Type == TileType.Corrider)
                           .Count(tile => tile.Type != TileType.ClosedDoor);
        if (ways == 4)
        {
          Level.Set(row, col, stair);
          break;
        }
      }
    }

    private bool MakeRoom(int row, int col, int maxWidth, int maxHeight, Direction direction)
    {
      int roomHeight = Randomizer.GetRand(4, maxHeight);
      int roomWidth = Randomizer.GetRand(4, maxWidth);

      Point[] points = GetRoomPoints(row, col, roomWidth, roomHeight, direction).ToArray();

      if (points.Any(point => !InBounds(point.Row, point.Col) || Level.Get(point).Type != TileType.Unused))
      {
        return false;
      }

      foreach (Point point in points)
      {
        bool isWall = IsWall(row, col, roomWidth, roomHeight, point, direction);
        Level.Set(point.Row, point.Col, isWall ? TileType.DirtWall : TileType.DirtFloor);
      }
      return true;
    }

    private IEnumerable<Point> GetRoomPoints(int row, int col, int roomWidth, int roomHeight, Direction facing)
    {
      Func<int, int, int> lowerBound = GetFeatureLowerBound;
      Func<int, int, int> upperBound = GetFeatureUpperBound;

      switch (facing)
      {
        case Direction.North:
          for (int currCol = lowerBound(col, roomWidth); currCol < upperBound(col, roomWidth); currCol++)
          {
            for (int currRow = row; currRow > row - roomHeight; currRow--)
            {
              var point = new Point(currRow, currCol);
              yield return point;
            }
          }
          break;
        case Direction.East:
          for (int currCol = col; currCol < col + roomWidth; currCol++)
          {
            for (int currRow = lowerBound(row, roomHeight); currRow < upperBound(row, roomHeight); currRow++)
            {
              var point = new Point(currRow, currCol);
              yield return point;
            }
          }
          break;
        case Direction.South:
          for (int currCol = lowerBound(col, roomWidth); currCol < upperBound(col, roomWidth); currCol++)
          {
            for (int currRow = row; currRow < row + roomHeight; currRow++)
            {
              var point = new Point(currRow, currCol);
              yield return point;
            }
          }
          break;
        case Direction.West:
          for (int currCol = col; currCol > col - roomWidth; currCol--)
          {
            for (int currRow = lowerBound(row, roomHeight); currRow < upperBound(row, roomHeight); currRow++)
            {
              var point = new Point(currRow, currCol);
              yield return point;
            }
          }
          break;
      }
    }

    private IEnumerable<Tuple<Point, Direction>> GetSurroundingPoints(Point point)
    {
      int row = point.Row;
      int col = point.Col;
      bool allPointsInBounds = InBounds(row - 1, col) && InBounds(row, col + 1) && InBounds(row + 1, col) &&
        InBounds(row, col - 1);

      Tuple<Point, Direction>[] points;
      if (allPointsInBounds)
      {
        points = new[]
        {
          Tuple.Create(new Point(row - 1, col), Direction.North),
          Tuple.Create(new Point(row, col + 1), Direction.East),
          Tuple.Create(new Point(row + 1, col), Direction.South),
          Tuple.Create(new Point(row, col - 1), Direction.West)
        };
      }
      else
      {
        points = new Tuple<Point, Direction>[0];
      }

      return points.Where(p => PointInBounds(p.Item1));
    }

    private IEnumerable<Tuple<Point, Direction, Tile>> GetSurroundings(Point point)
    {
      return GetSurroundingPoints(point).Select(p => Tuple.Create(p.Item1, p.Item2, Level.Get(p.Item1.Row, p.Item1.Col)));
    }

    private int GetFeatureLowerBound(int c, int len)
    {
      return c - len / 2;
    }

    private int IsFeatureWallBound(int c, int len)
    {
      return c + (len - 1) / 2;
    }

    private int GetFeatureUpperBound(int c, int len)
    {
      return c + (len + 1) / 2;
    }

    private bool IsWall(int row, int col, int roomWidth, int roomHeight, Point point, Direction direction)
    {
      Func<int, int, int> lowerBound = GetFeatureLowerBound;
      Func<int, int, int> upperBound = IsFeatureWallBound;

      int tcol = point.Col;
      int trow = point.Row;
      switch (direction)
      {
        case Direction.North:
          return tcol == lowerBound(col, roomWidth) || tcol == upperBound(col, roomWidth) || trow == row ||
            trow == row - roomHeight + 1;
        case Direction.East:
          return tcol == col || tcol == col + roomWidth - 1 || trow == lowerBound(row, roomHeight) ||
            trow == upperBound(row, roomHeight);
        case Direction.South:
          return tcol == lowerBound(col, roomWidth) || tcol == upperBound(col, roomWidth) || trow == row ||
            trow == row + roomHeight - 1;
        case Direction.West:
          return tcol == col || tcol == col - roomWidth + 1 || trow == lowerBound(row, roomHeight) ||
            trow == upperBound(row, roomHeight);
      }

      throw new InvalidOperationException();
    }

    private bool MakeCorridor(int row, int col, int colLength, Direction direction)
    {
      int deltaCol = Randomizer.GetRand(2, colLength);

      bool corriderMade = false;
      switch (direction)
      {
        case Direction.North:
          corriderMade = BuildNorthCorrider(row, col, deltaCol);
          goto default;
        case Direction.East:
          corriderMade = BuildEastCorrider(row, col, deltaCol);
          goto default;
        case Direction.South:
          corriderMade = BuildSouthCorrider(row, col, deltaCol);
          goto default;
        case Direction.West:
          corriderMade = BuildWestCorrider(row, col, deltaCol);
          goto default;
        default:
          if (!corriderMade)
          {
            return false;
          }
          break;
      }
      return true;
    }

    private bool BuildWestCorrider(int row, int col, int len)
    {
      int endCol = col - len;
      bool northWestOutOfBounds = !InBounds(row, col);
      if (northWestOutOfBounds)
      {
        return false;
      }

      for (int colTemp = col; colTemp > endCol; colTemp--)
      {
        bool eastWestOutOfBounds = !InBounds(row, colTemp);
        if (eastWestOutOfBounds || Level.Get(row, colTemp).Type != TileType.Unused)
        {
          return false;
        }
      }

      for (int colTemp = col; colTemp > endCol; colTemp--)
      {
        Level.Set(row, colTemp, TileType.Corrider);
      }
      return true;
    }

    private bool BuildSouthCorrider(int row, int col, int len)
    {
      int endRow = row + len;
      bool eastWestOutOfBounds = !InBounds(row, col);
      if (eastWestOutOfBounds)
      {
        return false;
      }

      for (int rowTemp = row; rowTemp < endRow; rowTemp++)
      {
        bool northSouthOutOfBounds = !InBounds(rowTemp, col);
        if (northSouthOutOfBounds || Level.Get(rowTemp, col).Type != TileType.Unused)
        {
          return false;
        }
      }

      for (int rowTemp = row; rowTemp < endRow; rowTemp++)
      {
        Level.Set(rowTemp, col, TileType.Corrider);
      }
      return true;
    }

    private bool BuildEastCorrider(int row, int col, int len)
    {
      int endCol = col + len;
      bool northSouthOutOfBounds = InBounds(row, col);
      if (northSouthOutOfBounds)
      {
        return false;
      }

      for (int colTemp = col; colTemp < endCol; colTemp++)
      {
        bool eastWestOutOfBounds = InBounds(row, colTemp);
        if (eastWestOutOfBounds || Level.Get(row, colTemp).Type != TileType.Unused)
        {
          return false;
        }
      }

      for (int colTemp = col; colTemp < endCol; colTemp++)
      {
        Level.Set(row, colTemp, TileType.Corrider);
      }
      return true;
    }

    private bool BuildNorthCorrider(int row, int col, int len)
    {
      int endRow = row - len;
      bool eastWestOutOfBounds = !InBounds(row, col);
      if (eastWestOutOfBounds)
      {
        return false;
      }

      for (int rowTemp = row; rowTemp > endRow; rowTemp--)
      {
        bool northSouthOutOfBounds = !InBounds(row, col);
        if (!northSouthOutOfBounds || Level.Get(rowTemp, col).Type != TileType.Unused)
        {
          return false;
        }
      }

      for (int rowTemp = row; rowTemp > endRow; rowTemp--)
      {
        Level.Set(rowTemp, col, TileType.Corrider);
      }
      return true;
    }

    private bool InBounds(int row, int col)
    {
      return row >= 0 && row < Level.Height && col >= 0 && col < Level.Width;
    }

    private bool PointInBounds(Point point)
    {
      return InBounds(point.Row, point.Col);
    }
  }
}