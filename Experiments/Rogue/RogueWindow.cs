using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;

namespace Rogue
{
  internal class RogueWindow
  {
    private Level Level;
    private int MapHeight;
    private int MapWidth;

    private Sprite[,] SpriteMap;
    private Texture TileSet;
    private int TileSize = 12;
    private RenderWindow Window;

    public RogueWindow(Level level)
    {
      Level = level;

      Window = new RenderWindow(new VideoMode(680, 460), "SFML window", Styles.Close);
      Window.SetActive();
      Window.Closed += OnClosed;
      Window.KeyPressed += OnKeyPressed;

      MapHeight = level.Height;
      MapWidth = level.Width;

      TileSet = new Texture("Textures/tileset.png");
      InitializeSpriteMap();
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

    private void UpdateSprite(int spriteRow, int spriteCol, Vector2f location)
    {
      SpriteMap[spriteRow, spriteCol].TextureRect = GetTextureRect(location);
    }

    public void Display()
    {
      Window.Clear();
      UpdateSpriteMap();
      DrawLevel();
      //DrawPlayer();
      Window.Display();
    }

    private void DrawPlayer()
    {
      throw new NotImplementedException();
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
    }
  }
}
