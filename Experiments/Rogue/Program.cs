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

      var window = new RogueWindow(CurrentLevel, MainCharacter);
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
        case Keyboard.Key.Right:
          MainCharacter.Move(Direction.East);
          break;
        case Keyboard.Key.Down:
          MainCharacter.Move(Direction.South);
          break;
        case Keyboard.Key.Up:
          MainCharacter.Move(Direction.North);
          break;
        case Keyboard.Key.PageUp:
          MainCharacter.Move(Direction.Northeast);
          break;
        case Keyboard.Key.PageDown:
          MainCharacter.Move(Direction.Southeast);
          break;
        case Keyboard.Key.Home:
          MainCharacter.Move(Direction.Northwest);
          break;
        case Keyboard.Key.End:
          MainCharacter.Move(Direction.Southwest);
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
