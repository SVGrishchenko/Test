using System.Xml;

namespace TestWpfApp
{
    public interface IXmlReader
    {
        string GetValue(string url, string node);
    }

    public class XmlReader : IXmlReader
    {
        public string GetValue(string url, string node)
        {
            string value = null;

            using (XmlTextReader reader = new XmlTextReader(url))
            {
                while (reader.Read())
                {
                    var XNodeName = reader.Name;
                    var XValue = reader.Value;
                    if (XNodeName == "outputValue")
                    {
                        if (reader.Read())
                        {
                            value = reader.Value;
                            break;
                        }
                    }
                }
            }

            return value;
        }
    }
}
