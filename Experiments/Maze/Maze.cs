using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Maze.Enums;

namespace Maze
{
  internal class Maze
  {
    private static readonly Room Cave = new Room(Descriptions.Cave);
    private static readonly Room DarkForest = new Room(Descriptions.DarkForest);
    private static readonly Room Mountain = new Room(Descriptions.Mountain);
    private static readonly Room Plain = new Room(Descriptions.Plain);
    private static readonly Room River = new Room(Descriptions.River);
    private static readonly Room Start = new Room(Descriptions.Start);
    private static readonly Room TreeTop = new Room(Descriptions.TreeTop);

    public Room StartRoom;

    public Maze()
    {
      InitializeMaze();

      XmlSerializer serializer = new XmlSerializer(typeof(ItemList));
      using (var stream = File.OpenRead("ItemList.xml"))
      {
        var list = serializer.Deserialize(stream);

      }
    }

    private void InitializeMaze()
    {
      StartRoom = Start;

      Start.Exits.Add(Directions.North, DarkForest);
      Start.Exits.Add(Directions.East, Mountain);
      Start.Exits.Add(Directions.West, River);
      //Start.ItemsInRoom.Add(Seed);

      DarkForest.Exits.Add(Directions.South, Start);
      DarkForest.Exits.Add(Directions.West, River);
      DarkForest.Exits.Add(Directions.Up, TreeTop);

      TreeTop.Exits.Add(Directions.Down, DarkForest);
      //TreeTop.ItemsInRoom.Add(Key);
      TreeTop.Events.Add("birdDistracted", false);

      River.Exits.Add(Directions.East, Start);
      River.Exits.Add(Directions.South, Plain);

      Plain.Exits.Add(Directions.North, River);

      Mountain.Exits.Add(Directions.West, Start);
      Mountain.Exits.Add(Directions.East, Cave);

      Cave.Exits.Add(Directions.West, Mountain);
      Cave.Events.Add("doorOpened", false);
    }
  }
}
