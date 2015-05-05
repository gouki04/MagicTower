using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class SpriteEditor : EditorWindow
{
    private Texture2D mTexture;
    private string mPlistPath;

    [MenuItem("Window/SpriteEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SpriteEditor));
    }
	
    void OnEnable()
    { }

    void OnGUI()
    {
        mTexture = (Texture2D)EditorGUILayout.ObjectField("Sprite", mTexture, typeof(Texture2D), false);

        EditorGUILayout.BeginVertical();
        mPlistPath = EditorGUILayout.TextField(mPlistPath);
        if (GUILayout.Button("Init From Plist"))
        {
            var path = AssetDatabase.GetAssetPath(mTexture);
            var texture_importer = AssetImporter.GetAtPath(path) as TextureImporter;
            texture_importer.isReadable = true;

            var meta_data = new List<SpriteMetaData>();

            var json_content = File.ReadAllText(mPlistPath);
            var json = SimpleJSON.JSON.Parse(json_content);

            var width = json["meta"]["size"]["w"].AsInt;
            var height = json["meta"]["size"]["h"].AsInt;

            var frames = json["frames"].AsArray;
            foreach (SimpleJSON.JSONNode frame in frames)
            {
                var sprite_meta_data = new SpriteMetaData();
                sprite_meta_data.alignment = 9;
                sprite_meta_data.border = new Vector4(0, 0, 0, 0);
                sprite_meta_data.pivot = new Vector2(0, 0);
                sprite_meta_data.rect = new Rect(frame["frame"]["x"].AsFloat, frame["frame"]["y"].AsFloat, frame["frame"]["w"].AsFloat, frame["frame"]["h"].AsFloat);
                sprite_meta_data.name = frame["filename"].Value.Replace(".png", "");

                meta_data.Add(sprite_meta_data);
            }

            texture_importer.spritesheet = meta_data.ToArray();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
        EditorGUILayout.EndVertical();
    }
}
