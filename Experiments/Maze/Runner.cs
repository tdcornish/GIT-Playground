using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Maze.Enums;

namespace Maze
{
  internal class Runner
  {
    public static void Main()
    {
      var maze = new Maze();
      var player = new Player(maze.StartRoom);

      Console.SetWindowSize(90, 40);
      Console.WriteLine(player);
      var parser = new Parser();
      while (true)
      {
        GameLoop(player, parser);
      }
    }

    private static void GameLoop(Player player, Parser parser)
    {
      string input = Console.ReadLine();

      Verb verb = parser.ParseInput(input);
      switch (verb)
      {
        case Verb.Go:
          MovePlayer(player, parser);
          break;
        case Verb.Take:
          TakeItem(player, parser);
          break;
        case Verb.Inventory:
          DisplayInventory(player);
          break;
        case Verb.Use:

        case Verb.Unknown:
          Console.WriteLine("\nUnknown Command");
          break;
      }
      Console.WriteLine();
      Console.WriteLine(player);
    }

    private static void MovePlayer(Player player, Parser parser)
    {
      bool canMove = player.Move(parser.ParseDirection());
      if (canMove)
      {
        return;
      }
      Console.WriteLine();
      Console.WriteLine("\nNothing of interest in that direction");
    }

    private static void TakeItem(Player player, Parser parser)
    {
      string itemToTake = parser.GetNoun();
      bool itemInRoom = player.CurrentRoom.IsItemInRoom(itemToTake);
      if (itemInRoom)
      {
        Item takenItem;
        bool took = player.Take(itemToTake, out takenItem);
        if (took)
        {
          Console.WriteLine("\nTook " + itemToTake);
        }
        else
        {
          Console.WriteLine("\n" + takenItem.FailureDescription);
        }

      }
      else
      {
        Console.WriteLine("\nNo items here by that name");
      }
    }

    private static void DisplayInventory(Player player)
    {
      bool emptyInventory = player.Inventory.Count == 0;
      if (emptyInventory)
      {
        Console.WriteLine("\nEmpty");
        return;
      }
      foreach (Item item in player.Inventory)
      {
        Console.WriteLine(item);
      }
    }
  }
}
