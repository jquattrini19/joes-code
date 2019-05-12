using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PayIt
{
  public class XmlHelper
  {
    public static void CreateXMLFile(string filePath, string tagName)
    {
      XElement mainElement = new XElement(tagName);
      mainElement.Save(filePath);
    }

    public static void SaveToXML(string filePath, XElement mainElement)
    {
      mainElement.Save(filePath);
    }

    public static XElement GetData(string filePath)
    {
      XElement mainElement = XElement.Load(filePath);
      return mainElement;
    }

    public static string GetAttributeValue(XElement element, string name)
    {
        return element.Attribute(name).Value;
    }

  }
}
