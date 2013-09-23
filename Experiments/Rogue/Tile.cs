using System;

namespace Rogue
{
  class Tile
  {
    public char Symbol { get; private set; }
    public bool IsPassable { get; private set; }
    public ConsoleColor Color { get; private set; }

    public Tile(char symbol, bool passable, ConsoleColor color)
    {
      Symbol = symbol;
      IsPassable = passable;
      Color = color;
    }
  }
}
