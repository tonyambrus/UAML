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
    public class Parser
    {
        private readonly Schema schema;
        private readonly XmlReader reader;
        private readonly Type elementBaseType;
        private readonly Data.Document document;
        private List<string> namespaces = new List<string>();

        public static Data.Document Parse(Schema schema, string text)
        {
            using (var reader = GetReader(text))
            {
                if (!TryGetType(schema.elementClass, out var elementBaseType))
                {
                    throw new Exception();
                }

                var parser = new Parser(schema, reader, elementBaseType);
                parser.ParseElements();
                return parser.document;
            }
        }

        private Parser(Schema schema, XmlReader reader, Type elementBaseType)
        {
            this.schema = schema;
            this.reader = reader;
            this.elementBaseType = elementBaseType;
            this.document = ParseRoot();
        }

        private static XmlReader GetReader(string text) => XmlReader.Create(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(text))));

        private Data.Document ParseRoot()
        {
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                if (!schema.TryGetElement(reader.Name, out var template) || !template.isRoot)
                {
                    throw new Exception($"Unknown UAML root {reader.Name}");
                }

                return new Data.Document
                {
                    root = ParseElement(template, null),
                    schema = schema
                };
            }

            throw new Exception("Didn't find any content");
        }

        private IEnumerable<Data.Element> GetElementChain(Data.Element element)
        {
            while (element != null)
            {
                yield return element;
                element = element.parent;
            }
        }

        private Data.Element ParseElement(Core.Element template, Data.Element parent)
        {
            var attributes = ReadAttributes(reader);

            if (!TryGetElementType(template, attributes, elementBaseType, out var ownerType, out var className))
            {
                throw new Exception($"Failed to get type {className}");
            }

            var typeInfo = ElementRegistry.GetElementType(ownerType);

            var namespaces = attributes
                .Where(p => NamespaceAttribute.IsNamespaceKey(p.Value) && NamespaceAttribute.IsCLRNamespace(p.Value))
                .Select(p => NamespaceAttribute.Parse(p.Value).Value)
                .ToList();

            if (parent == null && schema.namespaces != null)
            {
                namespaces.Add(schema.namespaces);
            }

            var nsHierarchy = GetElementChain(parent)
                .SelectMany(e => e.namespaces)
                .Concat(namespaces);

            var properties = attributes
                .Where(p => typeInfo.hierarchyProps.ContainsKey(p.Key, nsHierarchy))
                .ToDictionary(p => p.Key, p => p.Value);

            var events = attributes
                .Where(p => EventManager.HasRoutedEvent(p.Key, ownerType, nsHierarchy))
                .ToDictionary(p => p.Key, p => p.Value);

            return new Data.Element
            {
                parent = parent,
                name = reader.Name,
                className = className,
                type = ownerType,
                rawAttributes = attributes,
                properties = properties,
                events = events,
                namespaces = namespaces
            };
        }

        private static bool TryGetElementType(Core.Element template, Dictionary<string, Attribute> attributes, Type elementBaseType, out Type type, out string className)
        {
            if (attributes.TryGetValue("x:Class", out var xClass))
            {
                className = xClass.Value;
            }
            else if (!string.IsNullOrWhiteSpace(template.className))
            {
                className = template.className;
            }
            else
            {
                className = elementBaseType.FullName;
            }

            if (!string.IsNullOrWhiteSpace(xClass.Value))
            {
                if (TryGetType(xClass.Value, out var xClassType))
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


        private static Dictionary<string, Attribute> ReadAttributes(XmlReader reader)
        {
            var attributes = new Dictionary<string, Attribute>(StringComparer.OrdinalIgnoreCase);
            if (reader.MoveToFirstAttribute())
            {
                do
                {
                    attributes[reader.Name] = new Attribute(reader.Name, reader.Value);
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

        private void ParseElements()
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

                    var parent = elementStack.Peek();
                    var element = ParseElement(template, parent);
                    parent.children.Add(element);

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
