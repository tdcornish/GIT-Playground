using System;
using SFML.Window;
using SFML.Graphics;

namespace Rogue
{
  internal class RogueWindow
  {
    private readonly RenderWindow Window;

    private readonly uint TileSize = 10;
    private readonly Level Level;

    private Sprite[,] TileMap;
    private int MapHeight;
    private int MapWidth;

    public RogueWindow(Level level)
    {
      Level = level;

      Window = new RenderWindow(new VideoMode(640, 480), "SFML window", Styles.Close);
      Window.SetActive();
      Window.Closed += OnClosed;
      Window.KeyPressed += OnKeyPressed;

      Texture grass = new Texture("Textures/grass1.png");
      TileMap = new Sprite[level.Height, level.Width];
      for (int row = 0; row < level.Height; row++)
      {
        for (int col = 0; col < level.Width; col++)
        {
          TileMap[row, col] = new Sprite(grass)
          {
            Position = new Vector2f(row + row * grass.Size.Y, col + col * grass.Size.X)
          };
        }
      }
   } 

    public void Display()
    {
      Window.Clear();
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
      foreach (var sprite in TileMap)
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
