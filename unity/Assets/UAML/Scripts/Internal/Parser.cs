using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Uaml.Core;
using Uaml.Events;
using Uaml.Internal.Reflection;
using Uaml.UX;

namespace Uaml.Internal
{
    public static class Parser
    {
        private static XmlReader GetReader(string text)
        {
            return XmlReader.Create(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(text))));
        }

        public static Data.Document Parse(Schema schema, string text)
        {
            using (var reader = GetReader(text))
            {
                if (!TryGetType(schema.elementClass, out var elementBaseType))
                {
                    throw new Exception();
                }

                var document = ParseRoot(schema, reader, elementBaseType);
                ParseElements(document, schema, reader, elementBaseType);
                return document;
            }
        }

        private static Data.Document ParseRoot(Schema schema, XmlReader reader, Type elementBaseType)
        {
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                if (!schema.TryGetElement(reader.Name, out var template) || !template.isRoot)
                {
                    throw new System.Exception($"Unknown UAML root {reader.Name}");
                }

                return new Data.Document
                {
                    root = ParseElement(template, schema, reader, elementBaseType),
                    schema = schema
                };
            }

            throw new Exception("Didn't find any content");
        }

        private static Data.Element ParseElement(Core.Element template, Schema schema, XmlReader reader, Type elementBaseType)
        {
            var attributes = ReadAttributes(reader);

            if (!TryGetElementType(template, attributes, elementBaseType, out var type, out var className))
            {
                throw new Exception($"Failed to get type {className}");
            }

            var typeInfo = ElementRegistry.GetElementType(type);

            return new Data.Element
            {
                name = reader.Name,
                className = className,
                type = type,
                rawAttributes = attributes,
                properties = attributes.Where(p => typeInfo.hierarchyProps.ContainsKey(p.Key)).ToDictionary(p => p.Key, p => p.Value),
                events = attributes.Where(p => EventManager.HasRoutedEvent(p.Key)).ToDictionary(p => p.Key, p => p.Value)
            };
        }

        private static bool TryGetElementType(Core.Element template, Dictionary<string, string> attributes, Type elementBaseType, out Type type, out string className)
        {
            if (attributes.TryGetValue("x:Class", out var xClass))
            {
                className = xClass;
            }
            else if (!string.IsNullOrWhiteSpace(template.className))
            {
                className = template.className;
            }
            else
            {
                className = elementBaseType.FullName;
            }

            if (!string.IsNullOrWhiteSpace(xClass))
            {
                if (TryGetType(xClass, out var xClassType))
                {
                    type = xClassType;
                    return true;
                }
                else
                {
                    // xClass hasn't been generated yet
                }
            }

            if (!string.IsNullOrWhiteSpace(template.className))
            {
                if (TryGetType(template.className, out var templateType))
                {
                    type = templateType;
                    return true;
                }
                else
                {
                    // Type should exist if specified in schema
                    type = null;
                    return false;
                }
            }

            type = elementBaseType;
            return true;
        }


        private static Dictionary<string, string> ReadAttributes(XmlReader reader)
        {
            var attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (reader.MoveToFirstAttribute())
            {
                do
                {
                    attributes[reader.Name] = reader.Value;
                }
                while (reader.MoveToNextAttribute());

                // Move back to element
                reader.MoveToElement();
            }

            return attributes;
        }

        private static bool TryGetType(string typeName, out Type type)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(typeName))
                {
                    type = Type.GetType(typeName, true);
                    return true;
                }
            }
            catch
            {
            }

            type = null;
            return false;
        }

        private static void ParseElements(Data.Document document, Schema schema, XmlReader reader, Type elementBaseType)
        {
            var elementStack = new Stack<Data.Element>();
            elementStack.Push(document.root);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (!schema.TryGetElement(reader.Name, out var template))
                    {
                        throw new Exception($"Unknown element '{reader.Name}'");
                    }

                    var element = ParseElement(template, schema, reader, elementBaseType);
                    elementStack.Peek().children.Add(element);

                    if (!reader.IsEmptyElement)
                    {
                        elementStack.Push(element);
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    elementStack.Pop();
                }
            }
        }
    }
}
