using System.IO;
using Uaml.Core;
using Uaml.Internal;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;

[ScriptedImporter(1, "uaml")]
public class UamlImporter : ScriptedImporter
{
    public Schema schema;

    public override void OnImportAsset(AssetImportContext ctx)
    {
        if (!schema)
        {
            ctx.LogImportError("need a schema");
            return;
        }

        var fileName = Path.GetFileName(ctx.assetPath);
        var text = File.ReadAllText(ctx.assetPath);
        var document = Parser.Parse(schema, text);

        if (!Generator.Generate(document, ctx.assetPath))
        {
            // TODO: trigger reimport to load the generated class and add as component
        }

        var root = Spawner.Spawn(document);

        ctx.AddObjectToAsset("UAML", root);
        ctx.SetMainObject(root);
        ctx.DependsOnSourceAsset(AssetDatabase.GetAssetPath(schema));
    }
}
