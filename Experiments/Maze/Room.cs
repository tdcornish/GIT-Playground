using System.Collections.Generic;
using System.Linq;
using Maze.Enums;

namespace Maze
{
  public class Room
  {
    public string Description { get; set; }
    public List<Item> ItemsInRoom { get; set; }
    public Dictionary<Directions, Room> Exits { get; private set; }
    public Dictionary<string, bool> Events { get; private set; }

    public Room(string description)
    {
      Events = new Dictionary<string, bool>();
      Description = description;
      ItemsInRoom = new List<Item>();
      Exits = new Dictionary<Directions, Room>();
    }

    public bool IsItemInRoom(string item)
    {
      return ItemsInRoom.Any(itemInList => itemInList.Name.ToLower().Equals(item));
    }

    public string ExitsToString()
    {
      return Exits.Aggregate("", (current, exit) => current + exit.Key.ToString() + " ");
    }

    public override string ToString()
    {
      return Description;
    }
  }
}
