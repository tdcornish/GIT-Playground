
using Maze.Enums;

namespace Maze
{
  internal class Parser
  {
    public string[] InputTokens;

    public Verb ParseInput(string input)
    {
      this.InputTokens = input.Split(' ');

      var verb = GetVerb();
      switch (verb)
      {
        case "north":
        case "south":
        case "east":
        case "west":
        case "up":
        case "down":
          return Verb.Go;
        case "inventory":
          return Verb.Inventory;
        case "take":
          return Verb.Take;
        case "use":
          return Verb.Use;
        default:
          return Verb.Unknown;
      }
    }

    public Directions ParseDirection()
    {
      var direction = GetVerb();
      switch (direction)
      {
        case "south":
          return Directions.South;
        case "north":
          return Directions.North;
        case "west":
          return Directions.West;
        case "east":
          return Directions.East;
        case "up":
          return Directions.Up;
        case "down":
          return Directions.Down;
        default:
          return Directions.None;
      }
    }

    public string GetVerb()
    {
      return InputTokens[0].ToLower().Trim();
    }

    public string GetNoun()
    {
      return InputTokens[1].ToLower().Trim();
    }
  }
}
