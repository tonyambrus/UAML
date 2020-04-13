﻿using System.IO;
using System.Text;
using System.Xml;
using Uaml.UX;

namespace Uaml.Internal
{
    public class Exporter
    {
        public static string Export(ElementBase document) => Export(document, Encoding.ASCII);

        public static string Export(ElementBase document, Encoding encoding)
        {
            if (!document || !document.IsRoot)
            {
                throw new System.Exception("Invalid document");
            }

            var stream = new MemoryStream();
            Export(document, stream, encoding);
            return encoding.GetString(stream.ToArray());
        }

        public static void Export(ElementBase document, Stream stream, Encoding encoding)
        {
            using (var writer = XmlWriter.Create(new StreamWriter(stream), new XmlWriterSettings() { Encoding = encoding, Indent = true }))
            {
                writer.WriteStartDocument();
                WriteElement(document, writer);
                writer.WriteEndDocument();
            }
        }

        private static void WriteElement(ElementBase element, XmlWriter writer)
        {
            writer.WriteStartElement(element.Name);
            foreach (var attrib in element.GetProperties())
            {
                writer.WriteAttributeString(attrib.Key, attrib.Value);
            }

            foreach (var child in element.children)
            {
                WriteElement(child, writer);
            }
            writer.WriteEndElement();
        }
    }
}