using System;
using System.Collections.Generic;
using System.IO;
using Uaml.Core;
using Uaml.Internal;
using Uaml.Internal.Data;
using UnityEditor;
using UnityEngine;

namespace Uaml.Internal
{
    public static class Generator
    {
        public static bool Generate(Data.Document document, string assetPath)
        {
            var classFullName = document.root.className;

            var idx = classFullName.LastIndexOf('.');
            var classNs = idx != -1 ? classFullName.Substring(0, idx) : null;
            var className = idx != 1 ? classFullName.Substring(idx + 1) : classFullName;

            var fileName = Path.GetFileNameWithoutExtension(assetPath);
            if (className != fileName)
            {
                // TODO: if x:Class specified, class name must be same as file
            }

            return Generate(document, assetPath, classNs, className);
        }

        private static bool Generate(Data.Document document, string assetPath, string classNs, string className)
        {
            var baseType = document.root.name;
            var assetDir = Path.GetDirectoryName(assetPath);
            var assetFilename = Path.GetFileNameWithoutExtension(assetPath);
            var assetExt = Path.GetExtension(assetPath);

            if (className != assetFilename)
            {
                throw new Exception();
            }

            var userPath = GetPath(assetDir, assetFilename, ".cs");
            var hasUserPath = File.Exists(userPath);
            if (!hasUserPath)
            {
                WriteFile(userPath, u =>
                {
                    u.AppendLine($"using UnityEngine;");
                    u.AppendLine($"using Uaml.UX;");
                    u.AppendLine($"using Uaml.Events;");
                    u.AppendLine();
                    using (u.ScopeLine($"namespace {classNs}", classNs != null))
                    {
                        using (u.ScopeLine($"public partial class {className} : {baseType}"))
                        {
                            using (u.ScopeLine($"protected override void Awake()"))
                            {
                                u.AppendLine($"base.Awake();");
                                u.AppendLine($"InitializeComponent();");
                            }
                        }
                    }
                });
            }

            var genPath = GetPath(assetDir, assetFilename, ".g.cs");
            WriteFile(genPath, u =>
            {
                u.AppendLine($"using UnityEngine;");
                u.AppendLine($"using Uaml.UX;");
                u.AppendLine();
                using (u.ScopeLine($"namespace {classNs}", classNs != null))
                {
                    using (u.ScopeLine($"public partial class {className} : {baseType}"))
                    {
                        u.AppendLine($"private bool _contentLoaded;");
                        u.AppendLine();
                        using (u.ScopeLine($"public void InitializeComponent()"))
                        {
                            using (u.ScopeLine("if (_contentLoaded)"))
                            {
                                u.AppendLine("return;");
                            }
                            u.AppendLine("_contentLoaded = true;");
                            u.AppendLine();
                            u.AppendLine("Uaml.Core.Application.LoadComponent(this);");
                            u.AppendLine($"Debug.Log(\"Loaded component {className}\");");
                        }
                    }
                }
            });

            return hasUserPath;
        }

        private static string GetPath(string dir, string filename, string postfix)
        {
            var path = Path.Combine(dir, $"{filename}{postfix}");
            if (!Path.IsPathRooted(path))
            {
                path = Path.GetFullPath(Path.Combine(UnityEngine.Application.dataPath, "..", path));
            }

            return path;
        }

        private static void WriteFile(string path, Action<Printer> callback)
        {
            var p = new Printer();
            callback(p);

            var text = p.ToString();

            Debug.Log(path + ":\n" + text);
            File.WriteAllText(path, text);
        }
    }
}