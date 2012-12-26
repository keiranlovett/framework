/*using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TexturePacker : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Fistbump Framework/Import meshes from Texture Packer")]
    public static void ShowWindow()
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

        if (retVal)
        {
            CreateMeshes();
        }
    }

    void CreateMeshes()
    {
        

        String planeAssetName = "testMesh";
        GameObject go = null; // (GameObject)AssetDatabase.LoadAssetAtPath("Assets/TEST/" + planeAssetName, typeof(GameObject));
        if (go == null)
        {
            Transform transform;
            UnityEngine.Object prefab = PrefabUtility.CreateEmptyPrefab(null);


            go = new GameObject(planeAssetName);
            MeshFilter meshFilter = (MeshFilter)go.AddComponent(typeof(MeshFilter));
            go.AddComponent(typeof(MeshRenderer));

            Mesh mesh = new Mesh();

            int widthSegments = 1;
            int lengthSegments = 1;
            float width = 1.0f;
            float length = 1.0f;
            bool addCollider = false;
            bool createAtOrigin = true;
            Vector2 anchorOffset = Vector2.zero;


            int hCount2 = widthSegments + 1;
            int vCount2 = lengthSegments + 1;
            int numTriangles = widthSegments * lengthSegments * 6;
            int numVertices = hCount2 * vCount2;

            Vector3[] vertices = new Vector3[numVertices];
            Vector2[] uvs = new Vector2[numVertices];
            int[] triangles = new int[numTriangles];

            int index = 0;
            float uvFactorX = 1.0f / widthSegments;
            float uvFactorY = 1.0f / lengthSegments;
            float scaleX = width / widthSegments;
            float scaleY = length / lengthSegments;
            for (float y = 0.0f; y < vCount2; y++)
            {
                for (float x = 0.0f; x < hCount2; x++)
                {
                    if (true)// orientation == Orientation.Horizontal
                    {
                        vertices[index] = new Vector3(x * scaleX - width / 2f - anchorOffset.x, 0.0f, y * scaleY - length / 2f - anchorOffset.y);
                    }
//                     else
//                     {
//                         vertices[index] = new Vector3(x * scaleX - width / 2f - anchorOffset.x, y * scaleY - length / 2f - anchorOffset.y, 0.0f);
//                     }
                    uvs[index++] = new Vector2(x * uvFactorX, y * uvFactorY);
                }
            }

            index = 0;
            for (int y = 0; y < lengthSegments; y++)
            {
                for (int x = 0; x < widthSegments; x++)
                {
                    triangles[index] = (y * hCount2) + x;
                    triangles[index + 1] = ((y + 1) * hCount2) + x;
                    triangles[index + 2] = (y * hCount2) + x + 1;

                    triangles[index + 3] = ((y + 1) * hCount2) + x;
                    triangles[index + 4] = ((y + 1) * hCount2) + x + 1;
                    triangles[index + 5] = (y * hCount2) + x + 1;
                    index += 6;
                }
            }



            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            meshFilter.sharedMesh = mesh;

//             AssetDatabase.CreateAsset(go, "Assets/TEST/" + planeAssetName);
//             AssetDatabase.SaveAssets();
            EditorUtility.ReplacePrefab(go, prefab, ReplacePrefabOptions.ConnectToPrefab);
        }
    }

    public static Dictionary<string, UITextureInfo> loadTexturesFromTexturePackerJSON(string filename, Vector2 textureSize)
    {
        var textures = new Dictionary<string, UITextureInfo>();

        var asset = AssetDatabase.LoadAssetAtPath(filename, typeof(TextAsset)) as TextAsset;
        if (asset == null)
        {
            Debug.LogError("Could not find Texture Packer json config file in Resources folder: " + filename);
        }

        var jsonString = asset.text;
        var decodedHash = jsonString.hashtableFromJson();
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
        asset = null;
        Resources.UnloadUnusedAssets();

        return textures;
    }
}

public struct UITextureInfo
{
    public UIUVRect uvRect;
    public Rect frame;
    public Rect spriteSourceSize;
    public Vector2 sourceSize;
    public bool trimmed;
    public bool rotated;
}

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
}*/