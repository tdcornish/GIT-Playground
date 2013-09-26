namespace Rogue
{
  internal class Level
  {
    public int Height;
    public int Width;
    public Tile[,] Map;
    public Player Player;
   
    public Level(int width, int height)
    {
      Width = width;
      Height = height;
      Map = new Tile[Height,Width];
    }

    public void Set(int row, int col, TileType type)
    {
      Tile value = new Tile(type);
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

    public void SetPlayer()
    {
      Player = new Player
      {
        CurrentRow = Randomizer.GetRand(1, Height), 
        CurrentCol = Randomizer.GetRand(1, Width - 1)
      };

      Tile playerStartTile = Get(Player.CurrentRow, Player.CurrentCol);
      while (playerStartTile.Type != TileType.DirtFloor && playerStartTile.Type != TileType.Corrider)
      {
        Player.CurrentRow = Randomizer.GetRand(1, Height);
        Player.CurrentCol = Randomizer.GetRand(1, Width - 1);
        playerStartTile = Get(Player.CurrentRow, Player.CurrentCol);
      }
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
  }
}
