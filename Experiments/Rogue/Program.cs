using SFML.Window;

namespace Rogue
{
  internal class Program
  {
    private const int Objects = 50;
    private const int Width = 60;
    private const int Height = 40;
    public static int Top = 0;
    public static int Left = 0;

    private static MapGenerator Generator;
    private static Level[] Dungeon;
    private static int Depth;
    private static Player MainCharacter;

    public static void Main()
    {
      Generator = new MapGenerator();
      CreateDungeon(20);

      Depth = 0;
      MainCharacter = new Player(Dungeon[Depth]);
      MainCharacter.CurrentLevel.UpdateVisible(MainCharacter);

      var window = new RogueWindow(MainCharacter.CurrentLevel, MainCharacter);
      while (window.IsOpen())
      {
        window.DispatchEvents();
        MainCharacter.CurrentLevel.UpdateVisible(MainCharacter);
        window.Display();
      }
    }

    private static void CreateDungeon(int depth)
    {
      Dungeon = new Level[depth];
      for (int i = 0; i < depth; i++)
      {
        Level level = Generator.GenerateLevel(Width, Height, Objects);
        level.Depth = i;
        Dungeon[i] = level;
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
        case Keyboard.Key.Space:
          TileType playerCurrentTile =
            MainCharacter.CurrentLevel.Get(MainCharacter.Location)
                        .Type;
          switch (playerCurrentTile)
          {
            case TileType.Upstairs:
              MainCharacter.ChangeLevel(Direction.Up, Dungeon);
              break;
            case TileType.Downstairs:
              MainCharacter.ChangeLevel(Direction.Down, Dungeon);
              break;
          }
          break;
      }
    }
  }
}
