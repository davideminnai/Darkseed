using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor utility per configurare automaticamente gli sprite pixel art.
/// Usa: Window > Sprite Importer Config
/// </summary>
public class SpriteImporterConfig : EditorWindow
{
    [MenuItem("Tools/Configure Pixel Art Sprites")]
    public static void ShowWindow()
    {
        GetWindow<SpriteImporterConfig>("Pixel Art Config");
    }

    private string importPath = "Assets/Sprites";
    private float pixelsPerUnit = 16f;
    private bool generateCollider = true;
    private SpriteSortPoint sortPoint = SpriteSortPoint.Center;
    private FilterMode filterMode = FilterMode.Point; // Point = pixelated

    private void OnGUI()
    {
        GUILayout.Label("Pixel Art Sprite Configuration", EditorStyles.boldLabel);
        GUILayout.Space(10);

        importPath = EditorGUILayout.TextField("Import Path:", importPath);
        pixelsPerUnit = EditorGUILayout.FloatField("Pixels Per Unit:", pixelsPerUnit);
        generateCollider = EditorGUILayout.Toggle("Generate Collider:", generateCollider);
        filterMode = (FilterMode)EditorGUILayout.EnumPopup("Filter Mode:", filterMode);

        GUILayout.Space(10);

        if (GUILayout.Button("Configure Selected Sprites", GUILayout.Height(30)))
        {
            ConfigureSelectedSprites();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Configure All in Path", GUILayout.Height(30)))
        {
            ConfigureAllInPath();
        }

        GUILayout.Space(10);
        EditorGUILayout.HelpBox(
            "Per sprite pixel art:\n" +
            "- Pixels Per Unit: 16 o 32 (dipende dalla risoluzione)\n" +
            "- Filter Mode: Point (no smoothing)\n" +
            "- Compression: None",
            MessageType.Info);
    }

    private void ConfigureSelectedSprites()
    {
        Object[] selected = Selection.objects;
        int count = 0;

        foreach (Object obj in selected)
        {
            if (obj is Texture2D texture)
            {
                string path = AssetDatabase.GetAssetPath(texture);
                SpriteImporter importer = AssetImporter.GetAtPath(path) as SpriteImporter;

                if (importer != null)
                {
                    ConfigureSpriteImporter(importer);
                    count++;
                }
            }
        }

        if (count > 0)
        {
            EditorUtility.DisplayDialog("Successo", $"Configurati {count} sprite!", "OK");
            AssetDatabase.Refresh();
        }
        else
        {
            EditorUtility.DisplayDialog("Nessun risultato", "Seleziona degli sprite PNG prima.", "OK");
        }
    }

    private void ConfigureAllInPath()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { importPath });
        int count = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            SpriteImporter importer = AssetImporter.GetAtPath(path) as SpriteImporter;

            if (importer != null)
            {
                ConfigureSpriteImporter(importer);
                count++;
            }
        }

        EditorUtility.DisplayDialog("Successo", $"Configurati {count} sprite in '{importPath}'", "OK");
        AssetDatabase.Refresh();
    }

    private void ConfigureSpriteImporter(SpriteImporter importer)
    {
        importer.filterMode = filterMode;
        importer.textureType = SpriteTextureType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.spritesheet = null; // Reset sprite sheet
        
        if (generateCollider)
        {
            importer.generatePhysicsShape = true;
        }

        AssetDatabase.ImportAsset(importer.assetPath, ImportAssetOptions.ForceUpdate);
    }
}