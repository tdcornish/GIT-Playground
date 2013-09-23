using System;

namespace Rogue
{
  class Randomizer
  {
    private static readonly Random Rand = new Random();

    public static int GetRand(int min, int max)
    {
      return Rand.Next(min, max);
    }

    public static Direction RandomDirection()
    {
      int dir = GetRand(0, 4);
      switch (dir)
      {
        case 0:
          return Direction.North;
        case 1:
          return Direction.East;
        case 2:
          return Direction.South;
        case 3:
          return Direction.West;
        default:
          throw new InvalidOperationException();
      }
    }
  }
}
