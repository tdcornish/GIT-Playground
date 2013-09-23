using System.Collections.Generic;
using System.Linq;
using Maze.Enums;

namespace Maze
{
  internal class Player
  {
    public Player(Room currentRoom)
    {
      Inventory = new List<Item>();
      CurrentRoom = currentRoom;
    }

    public Room CurrentRoom { get; private set; }
    public List<Item> Inventory { get; private set; }

    public bool Move(Directions direction)
    {
      Room nextRoom;
      var exitExists = CurrentRoom.Exits.TryGetValue(direction, out nextRoom);
      if (exitExists)
      {
        CurrentRoom = nextRoom;
        return true;
      }

      return false;
    }

    public bool Use(string item)
    {
      bool itemInInventory;
      return false;
    }

    public bool Take(string noun, out Item takenItem)
    {
      foreach (var item in CurrentRoom.ItemsInRoom.Where(item => item.Name.ToLower().Equals(noun)))
      {
        if (!ValidateRequiredEventsHaveTakenPlace(item))
        {
          takenItem = item;
          return false;
        }
        CurrentRoom.ItemsInRoom.Remove(item);
        Inventory.Add(item);
        takenItem = item;
        return true;
      }

      takenItem = null;
      return false;
    }

    private bool ValidateRequiredEventsHaveTakenPlace(Item item)
    {
      foreach (var requiredFlag in item.Requires)
      {
        bool requirementMet;
        CurrentRoom.Events.TryGetValue(requiredFlag, out requirementMet);
        if (!requirementMet)
        {
          return false;
        }
      }
      return true;
    }

    public override string ToString()
    {
      return CurrentRoom.ToString();
    }
  }
}
