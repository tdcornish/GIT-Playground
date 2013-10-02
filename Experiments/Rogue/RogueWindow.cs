using System;
using SFML.Graphics;
using SFML.Window;

namespace Rogue
{
  internal class RogueWindow
  {
    public bool VisionEnabled = true;
    public const int TileSize = 12;

    private static Level Level;
    private static Player Player;
    private static VisibleSprite[,] SpriteMap;
    private int MapHeight;
    private int MapWidth;

    private Sprite PlayerSprite;
    private Texture TileSet;
    private RenderWindow Window;

    public RogueWindow(Level level, Player player)
    {
      Level = level;
      Player = player;

      Window = new RenderWindow(new VideoMode(724, 480), "SFML window", Styles.Close);
      Window.SetActive();
      Window.Closed += OnClosed;
      Window.KeyPressed += OnKeyPressed;

      MapHeight = level.Height;
      MapWidth = level.Width;

      TileSet = new Texture("Textures/tileset.png");
      InitializeSpriteMap();

      PlayerSprite = new Sprite(TileSet)
      {
        Color = Player.Color,
        TextureRect = GetTextureRect(SpriteCoordinates.Player),
        Position = new Vector2f(player.CurrentCol * TileSize, player.CurrentRow * TileSize)
      };
    }

    public void Display()
    {
      Window.Clear();
      UpdateSpriteMap();
      UpdatePlayerSprite();
      DrawLevel();
      DrawPlayer();
      Window.Display();
    }

    private IntRect GetTextureRect(Vector2f location)
    {
      int row = (int)location.X * TileSize;
      int col = (int)location.Y * TileSize;
      return new IntRect(row, col, TileSize, TileSize);
    }

    private void InitializeSpriteMap()
    {
      SpriteMap = new VisibleSprite[MapHeight,MapWidth];
      for (int row = 0; row < MapHeight; row++)
      {
        for (int col = 0; col < MapWidth; col++)
        {
          var sprite = new VisibleSprite(TileSet, false);
          sprite.SetTextureRect(GetTextureRect(SpriteCoordinates.Unused));
          sprite.SetPosition(col, row);
          SpriteMap[row, col] = sprite;
        }
      }
    }

    private void UpdateSpriteMap()
    {
      for (int row = 0; row < MapHeight; row++)
      {
        for (int col = 0; col < MapWidth; col++)
        {
          Tile tile = Level.Get(row, col);
          UpdateSprite(row, col, tile, Level.Visible[row, col]);
        }
      }
    }

    private void UpdatePlayerSprite()
    {
      int x = Player.CurrentCol * TileSize;
      int y = Player.CurrentRow * TileSize;
      PlayerSprite.Position = new Vector2f(x, y);
    }

    private void UpdateSprite(int spriteRow, int spriteCol, Tile tile, bool visible)
    {
      VisibleSprite toUpdate = SpriteMap[spriteRow, spriteCol];
      Vector2f location = SpriteCoordinates.GetTileTypeTextureCoordinates(tile.Type);
      toUpdate.SetTextureRect(GetTextureRect(location));
      toUpdate.Sprite.Color = tile.Color;
      toUpdate.Visible = visible;
    }

    private void DrawPlayer()
    {
      Window.Draw(PlayerSprite);
    }

    private void DrawLevel()
    {
      foreach (VisibleSprite sprite in SpriteMap)
      {
        if (sprite.Visible && VisionEnabled)
        {
          Window.Draw(sprite.Sprite);
        }

        else if(!VisionEnabled)
        {
          Window.Draw(sprite.Sprite);
        }
      }
    }

    public bool IsOpen()
    {
      return Window.IsOpen();
    }

    public void DispatchEvents()
    {
      Window.DispatchEvents();
    }

    private static void OnClosed(object sender, EventArgs e)
    {
      var window = (RenderWindow)sender;
      window.Close();
    }

    private static void OnKeyPressed(object sender, KeyEventArgs e)
    {
      var window = (RenderWindow)sender;
      if (e.Code == Keyboard.Key.Escape)
      {
        window.Close();
      }
      else
      {
        Program.ParseInput(e.Code);
      }
    }
  }
}
