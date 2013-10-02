using System;
using SFML.Graphics;
using SFML.Window;

namespace Rogue
{
  internal class RogueWindow
  {
    private Level Level;
    private Player Player;
    private int MapHeight;
    private int MapWidth;

    private Sprite PlayerSprite;
    private Sprite[,] SpriteMap;
    private Texture TileSet;
    private int TileSize = 12;
    private RenderWindow Window;

    public RogueWindow(Level level, Player player)
    {
      Level = level;
      Player = player;

      Window = new RenderWindow(new VideoMode(724, 360), "SFML window", Styles.Close);
      Window.SetActive();
      Window.Closed += OnClosed;
      Window.KeyPressed += OnKeyPressed;

      MapHeight = level.Height;
      MapWidth = level.Width;

      TileSet = new Texture("Textures/tileset.png");
      InitializeSpriteMap();

      PlayerSprite = new Sprite(TileSet)
      {
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
      SpriteMap = new Sprite[MapHeight,MapWidth];
      for (int row = 0; row < MapHeight; row++)
      {
        for (int col = 0; col < MapWidth; col++)
        {
          SpriteMap[row, col] = new Sprite(TileSet)
          {
            TextureRect = GetTextureRect(SpriteCoordinates.Unused),
            Position = new Vector2f(col * TileSize, row * TileSize)
          };
        }
      }
    }

    private void UpdateSpriteMap()
    {
      for (int row = 0; row < MapHeight; row++)
      {
        for (int col = 0; col < MapWidth; col++)
        {
          TileType type = Level.Get(row, col).Type;
          UpdateSprite(row, col, SpriteCoordinates.GetTextureCoordinates(type));
        }
      }
    }

    private void UpdatePlayerSprite()
    {
      PlayerSprite.Position = new Vector2f(Player.CurrentCol * TileSize, Player.CurrentRow * TileSize);
    }

    private void UpdateSprite(int spriteRow, int spriteCol, Vector2f location)
    {
      SpriteMap[spriteRow, spriteCol].TextureRect = GetTextureRect(location);
    }

    private void DrawPlayer()
    {
      Window.Draw(PlayerSprite);
    }

    private void DrawLevel()
    {
      foreach (Sprite sprite in SpriteMap)
      {
        Window.Draw(sprite);
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
