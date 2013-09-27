using System;
using SFML.Window;

namespace Rogue
{
  internal class SFMLWindow
  {
    public static void Display()
    {

      var window = new Window(new VideoMode(640, 480), "SFML window with OpenGL", Styles.Default);

      window.SetActive();
      window.Closed += OnClosed;
      window.KeyPressed += OnKeyPressed;

      int startTime = Environment.TickCount;

      while (window.IsOpen())
      {
        window.DispatchEvents();
        float time = (Environment.TickCount - startTime) / 1000.0F;
        window.Display();
      }
    }

    private static void OnClosed(object sender, EventArgs e)
    {
      var window = (Window)sender;
      window.Close();
    }

    private static void OnKeyPressed(object sender, KeyEventArgs e)
    {
      var window = (Window)sender;
      if (e.Code == Keyboard.Key.Escape)
      {
        window.Close();
      }
    }
  }
}
