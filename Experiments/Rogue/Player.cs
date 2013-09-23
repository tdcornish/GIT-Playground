namespace Rogue
{
  internal class Player
  {
    public Player(Level level)
    {
      Symbol = '@';

      CurrentRow = Randomizer.GetRand(1, level.Height);
      CurrentCol = Randomizer.GetRand(1, level.Width - 1);

      Tile playerStartTile = level.Get(CurrentRow, CurrentCol);
      while (playerStartTile != Level.TileTypes["DirtFloor"] && playerStartTile != Level.TileTypes["Corrider"])
      {
        CurrentRow = Randomizer.GetRand(1, level.Height);
        CurrentCol = Randomizer.GetRand(1, level.Width - 1);
        playerStartTile = level.Get(CurrentRow, CurrentCol);
      }
    }

    public char Symbol { get; private set; }
    public int CurrentRow { get; set; }
    public int CurrentCol { get; set; }

    public void Move(Level level, int deltaRow, int deltaCol)
    {
      int newRow = CurrentRow + deltaRow;
      int newCol = CurrentCol + deltaCol;

      bool outOfBounds = newRow < 0 || newRow >= level.Height || newCol < 0 || newCol >= level.Width;
      if (!outOfBounds)
      {
        Tile nextTile = level.Get(newRow, newCol);
        if (nextTile.IsPassable)
        {
          CurrentRow = newRow;
          CurrentCol = newCol;
        }
      }
    }
  }
}
