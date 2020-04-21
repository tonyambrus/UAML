using System.IO;
using System.Text;
using System.Xml;
using Uaml.UX;

namespace Uaml.Internal
{
    public class Exporter
    {
        public static string Export(FrameworkElement document) => Export(document, Encoding.ASCII);

        public static string Export(FrameworkElement document, Encoding encoding)
        {
            if (!document || !document.IsRoot)
            {
                throw new System.Exception("Invalid document");
            }

            var stream = new MemoryStream();
            Export(document, stream, encoding);
            return encoding.GetString(stream.ToArray());
        }

        public static void Export(FrameworkElement document, Stream stream, Encoding encoding)
        {
            using (var writer = XmlWriter.Create(new StreamWriter(stream), new XmlWriterSettings() { Encoding = encoding, Indent = true }))
            {
                writer.WriteStartDocument();
                WriteElement(document, writer);
                writer.WriteEndDocument();
            }
        }

        private static void WriteElement(FrameworkElement element, XmlWriter writer)
        {
            writer.WriteStartElement(element.ElementName);
            foreach (var attrib in element.GetProperties())
            {
                writer.WriteAttributeString(attrib.Key, attrib.Value);
            }

            foreach (var child in element.Children)
            {
                WriteElement(child, writer);
            }
            writer.WriteEndElement();
        }
    }
}