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
    private static int CurrentLevelNumber;
    private static Player MainCharacter;

    public static void Main()
    {
      Generator = new MapGenerator();
      Dungeon = new Level[20];
      for (int i = 0; i < Dungeon.Length; i++)
      {
        Dungeon[i] = Generator.GenerateLevel(Width, Height, Objects);
      }
      CurrentLevelNumber = 0;
      MainCharacter = new Player( Dungeon[CurrentLevelNumber]);
      MainCharacter.CurrentLevel.UpdateVisible(MainCharacter);

      var window = new RogueWindow(MainCharacter.CurrentLevel, MainCharacter);
      while (window.IsOpen())
      {
        window.DispatchEvents();
        MainCharacter.CurrentLevel.UpdateVisible(MainCharacter);
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
        case Keyboard.Key.Space:
          TileType playerCurrentTile =
            MainCharacter.CurrentLevel.Get(MainCharacter.CurrentRow, MainCharacter.CurrentCol)
                        .Type;
          switch (playerCurrentTile)
          {
            case TileType.Upstairs:
              ChangeLevel(Direction.Up);
              break;
            case TileType.Downstairs:
              ChangeLevel(Direction.Down);
              break;
          }
          break;
      }
    }

    private static void ChangeLevel(Direction direction)
    {
      switch (direction)
      {
        case Direction.Up:
          MainCharacter.CurrentLevel = Dungeon[--CurrentLevelNumber];
          MainCharacter.CurrentRow =
            MainCharacter.CurrentLevel.DownstairsLocation.Row;
          MainCharacter.CurrentCol =
            MainCharacter.CurrentLevel.DownstairsLocation.Col;
          break;
        case Direction.Down:
          MainCharacter.CurrentLevel = Dungeon[++CurrentLevelNumber];
          MainCharacter.CurrentRow =
            MainCharacter.CurrentLevel.UpstairsLocation.Row;
          MainCharacter.CurrentCol =
            MainCharacter.CurrentLevel.UpstairsLocation.Col;
          break;
      }
    }
  }
}
