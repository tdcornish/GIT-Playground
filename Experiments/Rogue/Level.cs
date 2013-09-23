﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Rogue
{
  internal class Level
  {
    private const int Objects = 50;
    private const int RoomChance = 45;

    public static Dictionary<string, Tile> TileTypes = new Dictionary<string, Tile>
    {
      {"Unused", new Tile (' ', false, ConsoleColor.Black) },
      {"DirtWall", new Tile('#', false, ConsoleColor.White) },
      {"DirtFloor", new Tile('.', true, ConsoleColor.White) },
      {"Corrider", new Tile('.', true, ConsoleColor.Blue) },
      {"Door", new Tile('/', true, ConsoleColor.DarkYellow) },
      {"Upstairs", new Tile('<', true, ConsoleColor.DarkMagenta) },
      {"Downstairs", new Tile('>', true, ConsoleColor.DarkMagenta) }, 
      {"Chest", new Tile('*', true, ConsoleColor.Yellow) } 
    };

    public int Height;
    public Tile[,] Map;
    public Player Player;
    public int Width;

    public Level(int width, int height)
    {
      Width = width;
      Height = height;

      GenerateMap();
      Player = new Player(this);
    }

    public void Set(int row, int col, Tile value)
    {
      Map[row, col] = value;
    }

    public Tile Get(int row, int col)
    {
      return Map[row, col];
    }

    public Tile GetPoint(Point point)
    {
      return Get(point.Row, point.Col);
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

    #region private
    private void GenerateMap()
    {
      Init();
      MakeRoom(10, 40, 4, 4, Direction.North);
      int currentFeatures = 1;
      TryBuildFeatures(currentFeatures);
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
        currentFeatures = CurrentFeatures(directionToBuild, row, rowMod, col, colMod, currentFeatures);
      }
    }

    private void FindLocationToBuildAt(ref int row, ref int col, ref int rowMod, ref int colMod, ref Direction? directionToBuild)
    {
      for (int testing = 0; testing < 1000; testing++)
      {
        row = Randomizer.GetRand(1, Height - 1);
        col = Randomizer.GetRand(1, Width - 1);

        Tile testTile = Get(row, col);
        if (testTile.IsPassable || testTile == TileTypes["DirtWall"])
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
          bool adjacentDoor = adjacentPoints.Any(p => p.Item3 == TileTypes["Door"]);
          if (adjacentDoor)
          {
            directionToBuild = null;
          }

          break;
        }
      }
    }

    private int CurrentFeatures(Direction? directionToBuild, int row, int rowMod, int col, int colMod, int currentFeatures)
    {
      if (directionToBuild.HasValue)
      {
        int feature = Randomizer.GetRand(0, 100);
        if (feature <= RoomChance)
        {
          if (MakeRoom(row + rowMod, col + colMod, 8, 6, directionToBuild.Value))
          {
            currentFeatures++;
            Set(row, col, TileTypes["Door"]);
            Set(row + rowMod, col + colMod, TileTypes["DirtFloor"]);
          }
        }
        else if (feature >= RoomChance)
        {
          if (MakeCorridor(row + rowMod, col + colMod, 6, directionToBuild.Value))
          {
            currentFeatures++;
            Set(row, col, TileTypes["Door"]);
          }
        }
      }
      return currentFeatures;
    }

    private void Init()
    {
      Map = new Tile[Height, Width];
      for (int row = 0; row < Height; row++)
      {
        for (int col = 0; col < Width; col++)
        {
          Map[row, col] = TileTypes["Unused"];
        }
      }
    }

    private bool MakeRoom(int row, int col, int maxWidth, int maxHeight, Direction direction)
    {
      int roomHeight = Randomizer.GetRand(4, maxHeight);
      int roomWidth = Randomizer.GetRand(4, maxWidth);

      Point[] points = GetRoomPoints(row, col, roomWidth, roomHeight, direction).ToArray();

      if (points.Any(point => !InBounds(point.Row, point.Col) || GetPoint(point) != TileTypes["Unused"]))
      {
        return false;
      }

      foreach (Point point in points)
      {
        bool isWall = IsWall(row, col, roomWidth, roomHeight, point, direction);
        Set(point.Row, point.Col, isWall ? TileTypes["DirtWall"] : TileTypes["DirtFloor"]);
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
      return GetSurroundingPoints(point).Select(p => Tuple.Create(p.Item1, p.Item2, Get(p.Item1.Row, p.Item1.Col)));
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

    private bool MakeCorridor(int row, int col, int length, Direction direction)
    {
      int len = Randomizer.GetRand(2, length);

      bool corriderMade = false;
      switch (direction)
      {
        case Direction.North:
          corriderMade = BuildNorthCorrider(row, col, len);
          goto default;
        case Direction.East:
          corriderMade = BuildEastCorrider(row, col, len);
          goto default;
        case Direction.South:
          corriderMade = BuildSouthCorrider(row, col, len);
          goto default;
        case Direction.West:
          corriderMade = BuildWestCorrider(row, col, len);
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
        if (eastWestOutOfBounds || Get(row, colTemp) != TileTypes["Unused"])
        {
          return false;
        }
      }

      for (int colTemp = col; colTemp > endCol; colTemp--)
      {
        Set(row, colTemp, TileTypes["Corrider"]);
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
        if (northSouthOutOfBounds || Get(rowTemp, col) != TileTypes["Unused"])
        {
          return false;
        }
      }

      for (int rowTemp = row; rowTemp < endRow; rowTemp++)
      {
        Set(rowTemp, col, TileTypes["Corrider"]);
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
        if (eastWestOutOfBounds || Get(row, colTemp) != TileTypes["Unused"])
        {
          return false;
        }
      }

      for (int colTemp = col; colTemp < endCol; colTemp++)
      {
        Set(row, colTemp, TileTypes["Corrider"]);
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
        if (!northSouthOutOfBounds || Get(rowTemp, col) != TileTypes["Unused"])
        {
          return false;
        }
      }

      for (int rowTemp = row; rowTemp > endRow; rowTemp--)
      {
        Set(rowTemp, col, TileTypes["Corrider"]);
      }
      return true;
    }

    private bool InBounds(int row, int col)
    {
      return row >= 0 && row < Height && col >= 0 && col < Width;
    }

    private bool PointInBounds(Point point)
    {
      return InBounds(point.Row, point.Col);
    }
    #endregion
  }
}
