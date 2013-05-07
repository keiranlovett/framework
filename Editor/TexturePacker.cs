using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class TexturePacker : EditorWindow
{
    Object m_TexturePackerMetafile;
    Object m_TexturePackerImage;

    [MenuItem("Fistbump Framework/Import meshes from Texture Packer")]
    public static void ShowTexturePackerWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(TexturePacker));
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        bool retVal = GUILayout.Button("Add To", GUILayout.Width(76f));
        GUI.color = Color.white;
        GUILayout.Label("Select the parent in the Hierarchy View", GUILayout.MinWidth(10000f));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Select TexturePacker metafile");
        m_TexturePackerMetafile = EditorGUILayout.ObjectField(m_TexturePackerMetafile, typeof(TextAsset), false);
        GUILayout.EndHorizontal();

        if (retVal)
        {
            if(m_TexturePackerMetafile != null)
            {
                Dictionary<string, UITextureInfo> test = loadTexturesFromTexturePackerJSON(m_TexturePackerMetafile.ToString());
                foreach(KeyValuePair<string, UITextureInfo> u in test)
                {
                    Debug.Log(u.Key);
                    Debug.Log(u.Value);
                }
            }
            Debug.Log("yay!");
            //CreateMeshes();
        }
    }

    void CreateMeshes()
    {
        string assetPath = "Assets/test1.prefab";
        // clone the model template
        //Object templatePrefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));
        //GameObject template = (GameObject)EditorUtility.InstantiatePrefab(templatePrefab);

        // this way links will persist when we regenerate the mesh
        Object prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));
        if (!prefab)
        {
            prefab = EditorUtility.CreateEmptyPrefab(assetPath);
        }
        


        // p1       p2
        //  *-------*
        //  |       |
        //  |  0,0  |
        //  |       |
        //  *-------*
        // p0       p3

        float size = 1;
        float halfsize = size/2;

        Vector3 p0;
        Vector3 p1;
        Vector3 p2;
        Vector3 p3;

        const bool faceFront = false;
        const bool faceUp = true;
        if(faceFront)
        {
            p0 = new Vector3(-halfsize, -halfsize, 0);
            p1 = new Vector3(-halfsize, halfsize, 0);
            p2 = new Vector3(halfsize, halfsize, 0);
            p3 = new Vector3(halfsize, -halfsize, 0);
        }
        else if (faceUp)
        {
            p0 = new Vector3(-halfsize, 0, -halfsize);
            p1 = new Vector3(-halfsize, 0, halfsize);
            p2 = new Vector3(halfsize, 0, halfsize);
            p3 = new Vector3(halfsize, 0, -halfsize);    
        }
        float totw = 132;
        float toth = 132;
        int x = 66;
        int y = 1;
        int w = 32;
        int h = 32;

        UIUVRect test = new UIUVRect(x, y, w, h, new Vector2(totw, toth));


        // sort of the same...
        Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Mesh));
        if (!mesh)
        {
            mesh = new Mesh();
            mesh.name = name;
            AssetDatabase.AddObjectToAsset(mesh, assetPath);
        }
        else
        {
            mesh.Clear();
        }
        // generate your mesh in place
        
        
        mesh.name = "Scripted_Plane_New_Mesh";
        mesh.vertices = new[] { p0, p1, p2, p3 };
        mesh.uv = new[] { test.lowerLeftUV, 
                       test.lowerLeftUV + Vector2.up * test.uvDimensions.y, 
                       test.lowerLeftUV + test.uvDimensions, 
                       test.lowerLeftUV + Vector2.right * test.uvDimensions.x };
        mesh.triangles = new[] { 0, 1, 2, 0, 2, 3 };

        foreach (Vector2 v in mesh.uv)
            Debug.Log(v);
        mesh.RecalculateNormals();
        GameObject obj = new GameObject("New_Plane_Fom_Script");
        obj.AddComponent<MeshRenderer>();
        obj.AddComponent<MeshFilter>().sharedMesh = mesh;
        obj.transform.position = new Vector3(1.45f, 0, 1.02f);

        // make sure 
        EditorUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ReplaceNameBased);

        // get rid of the temporary object (otherwise it stays over in scene)
        Object.DestroyImmediate(obj);
    }

    public static Dictionary<string, UITextureInfo> loadTexturesFromTexturePackerJSON(string jsonString)
    {
        Vector2 textureSize = Vector2.zero;
        var textures = new Dictionary<string, UITextureInfo>();

        //var asset = AssetDatabase.LoadAssetAtPath(filename, typeof(TextAsset)) as TextAsset;
        //if (asset == null)
        //{
        //    Debug.LogError("Could not find Texture Packer json config file in Resources folder: " + filename);
        //}

        var decodedHash = jsonString.hashtableFromJson();
        var meta = (IDictionary)decodedHash["meta"];
        if (meta != null)
        {
            var size = (IDictionary)meta["size"];
            if (size != null)
            {
                var textureSizeX = int.Parse(size["w"].ToString());
                var textureSizeY = int.Parse(size["h"].ToString());
                textureSize = new Vector2(textureSizeX, textureSizeY);
                Debug.Log(textureSize);
            }
        }

        var frames = (IDictionary)decodedHash["frames"];
        foreach (System.Collections.DictionaryEntry item in frames)
        {
            // extract the info we need from the TexturePacker json file, mainly uvRect and size
            var frame = (IDictionary)((IDictionary)item.Value)["frame"];
            var frameX = int.Parse(frame["x"].ToString());
            var frameY = int.Parse(frame["y"].ToString());
            var frameW = int.Parse(frame["w"].ToString());
            var frameH = int.Parse(frame["h"].ToString());

            // for trimming support
            var spriteSourceSize = (IDictionary)((IDictionary)item.Value)["spriteSourceSize"];
            var spriteSourceSizeX = int.Parse(spriteSourceSize["x"].ToString());
            var spriteSourceSizeY = int.Parse(spriteSourceSize["y"].ToString());
            var spriteSourceSizeW = int.Parse(spriteSourceSize["w"].ToString());
            var spriteSourceSizeH = int.Parse(spriteSourceSize["h"].ToString());

            var sourceSize = (IDictionary)((IDictionary)item.Value)["sourceSize"];
            var sourceSizeX = int.Parse(sourceSize["w"].ToString());
            var sourceSizeY = int.Parse(sourceSize["h"].ToString());

            var trimmed = (bool)((IDictionary)item.Value)["trimmed"];
            var rotated = (bool)((IDictionary)item.Value)["rotated"];

            var ti = new UITextureInfo();
            ti.frame = new Rect(frameX, frameY, frameW, frameH);
            ti.uvRect = new UIUVRect(frameX, frameY, frameW, frameH, textureSize);
            ti.spriteSourceSize = new Rect(spriteSourceSizeX, spriteSourceSizeY, spriteSourceSizeW, spriteSourceSizeH);
            ti.sourceSize = new Vector2(sourceSizeX, sourceSizeY);
            ti.trimmed = trimmed;
            ti.rotated = rotated;

            textures.Add(item.Key.ToString(), ti);
        }

        // unload the asset
        //asset = null;
        Resources.UnloadUnusedAssets();

        return textures;
    }
}

//Taken from UIToolkit
public struct UITextureInfo
{
    public UIUVRect uvRect;
    public Rect frame;
    public Rect spriteSourceSize;
    public Vector2 sourceSize;
    public bool trimmed;
    public bool rotated;
}


//Taken from UIToolkit
public struct UIUVRect
{
    public Vector2 lowerLeftUV;
    public Vector2 uvDimensions;

    private Vector2 _originalCoordinates; // used internally for clipping
    private int _originalWidth; // used internally for clipping


    /// <summary>
    /// Convenience property to return a UVRect of all zeros
    /// </summary>
    public static UIUVRect zero;


    /// <summary>
    /// Automatically converts coordinates to UV space as specified by textureSize
    /// </summary>
    public UIUVRect(int x, int y, int width, int height, Vector2 textureSize)
    {
        _originalCoordinates.x = x;
        _originalCoordinates.y = y;
        _originalWidth = width;

        lowerLeftUV = new Vector2(x / textureSize.x, 1.0f - ((y + height) / textureSize.y));
        uvDimensions = new Vector2(width / textureSize.x, height / textureSize.y);
    }
}