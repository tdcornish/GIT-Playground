using System;
using System.Text;
using SFML.Window;

namespace Rogue
{
  internal class Program
  {
    private const int Objects = 50;
    private const int Width = 60;
    private const int Height = 20;
    public static int Top = 0;
    public static int Left = 0;

    private static MapGenerator Generator;
    private static Level[] Dungeon;
    private static Level CurrentLevel;
    private static Player MainCharacter;

    public static void Main()
    {
      Generator = new MapGenerator();
      Dungeon = new Level[20];
      for (int i = 0; i < Dungeon.Length; i++)
      {
        Dungeon[i] = Generator.GenerateLevel(Width, Height, Objects);
      }
      CurrentLevel = Dungeon[0];

      MainCharacter = new Player(CurrentLevel);
      CurrentLevel.UpdateVisible(MainCharacter);

      var window = new RogueWindow(CurrentLevel);
      while (window.IsOpen())
      {
        window.DispatchEvents();
        CurrentLevel.UpdateVisible(MainCharacter);
        window.Display();
      }
    }

    public static void ParseInput(Keyboard.Key key)
    {
      switch (key)
      {
        case Keyboard.Key.Left:
          MainCharacter.Move(Direction.West);
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

    //public static void PrintMap(Level level)
    //{
    //  var line = new StringBuilder();
    //  Console.ForegroundColor = ConsoleColor.White;
    //  for (int row = 0; row < Height; row++)
    //  {
    //    for (int col = 0; col < Width; col++)
    //    {
    //      Tile value = level.Get(row, col);
    //      line.Append(level.IsVisible(row, col) ? value.Symbol : ' ');
    //      //line.Append(value.Symbol);
    //    }
    //    line.AppendLine();
    //  }

    //  Console.SetCursorPosition(Left, Top);
    //  Console.WriteLine(line);
    //  PrintPlayer(level.Player);
    //}

    //public static void PrintPlayer(Player player)
    //{
    //  Console.ForegroundColor = ConsoleColor.Red;
    //  Console.SetCursorPosition(Left + player.CurrentCol, Top + player.CurrentRow);
    //  Console.Write(player.Symbol);
    //}
  }
}
