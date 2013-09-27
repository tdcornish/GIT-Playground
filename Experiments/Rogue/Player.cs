namespace Rogue
{
  internal class Player
  {
    public Player()
    {
      Symbol = '@';
      VisionRange = 5;
    }

    public char Symbol { get; private set; }
    public int CurrentRow { get; set; }
    public int CurrentCol { get; set; }
    public int VisionRange;

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
