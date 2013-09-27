using System;
using System.Text;

namespace Rogue
{
  internal class Program
  {
    public static int Top = 0;
    public static int Left = 0;

    private const int Objects = 50;
    private const int Width = 75;
    private const int Height = 20;

    private static MapGenerator _Generator;

    public static void Main()
    {
      _Generator = new MapGenerator();
      var level = _Generator.GenerateLevel(Width, Height, Objects);
      level.SetPlayer();

      Console.CursorVisible = false;
      while (true)
      {
        level.UpdateVisible();
        PrintMap(level);
        ReadInput(level);
      }
    }

    private static void ReadInput(Level level)
    {
      ConsoleKeyInfo keyPressed = Console.ReadKey();
      switch (keyPressed.Key)
      {
        case ConsoleKey.D4:
        case ConsoleKey.LeftArrow:
          level.MovePlayer(Direction.West);
          break;
        case ConsoleKey.D6:
        case ConsoleKey.RightArrow:
          level.MovePlayer(Direction.East);
          break;
        case ConsoleKey.D2:
        case ConsoleKey.DownArrow:
          level.MovePlayer(Direction.South);
          break;
        case ConsoleKey.D8:
        case ConsoleKey.UpArrow:
          level.MovePlayer(Direction.North);
          break;
        case ConsoleKey.D9:
        case ConsoleKey.PageUp:
          level.MovePlayer(Direction.Northeast);
          break;
        case ConsoleKey.D3:
        case ConsoleKey.PageDown:
          level.MovePlayer(Direction.Southeast);
          break;
        case ConsoleKey.D7:
        case ConsoleKey.Home:
          level.MovePlayer(Direction.Northwest);
          break;
        case ConsoleKey.D1:
        case ConsoleKey.End:
          level.MovePlayer(Direction.Southwest);
          break;
      }
    }

    public static void PrintMap(Level level)
    {
      StringBuilder line = new StringBuilder();
      Console.ForegroundColor = ConsoleColor.White;
      for (int row = 0; row < Height; row++)
      {

        for (int col = 0; col < Width; col++)
        {
          Tile value = level.Get(row, col);
          line.Append(level.IsVisible(row, col) ? value.Symbol : ' ');
          //line.Append(value.Symbol);
        }
        line.AppendLine();
      }

      Console.SetCursorPosition(Left, Top);
      Console.WriteLine(line);
      PrintPlayer(level.Player);
    }

    public static void PrintPlayer(Player player)
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.SetCursorPosition(Left + player.CurrentCol, Top + player.CurrentRow);
      Console.Write(player.Symbol);
    }
  }
}
