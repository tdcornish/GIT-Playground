using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maze
{
  public class Item
  {
    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("Location")]
    public string Location { get; set; }

    [XmlElement("FailureDescription")]
    public string FailureDescription { get; set; }

    [XmlArray("Requires")]
    public List<string> Requires { get; set; }

    [XmlArray("Flags")]
    public List<string> Flags { get; set; }

    public override string ToString()
    {
      return Name;
    }
  }


  [XmlRoot("ItemList")]
  public class ItemList
  {
    [XmlElement("Item")]
    public List<Item> List { get; set; }
  }

}
