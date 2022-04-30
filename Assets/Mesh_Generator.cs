using UnityEditor;
using UnityEngine;

public class Mesh_Generator : ParentEditor
{
    public Texture2D heightMap;
    public Material mat;
    public Vector3 size = new Vector3(2048, 300, 2048);
    public string mname = "Enter name";
    GameObject generated;

    [MenuItem("Tools/Mesh Generator")]
    static void Init()
    {
        Mesh_Generator mg = (Mesh_Generator)GetWindow(typeof(Mesh_Generator));
        mg.Show();
    }

    void OnGUI()
    {
        heightMap = (Texture2D)EditorGUILayout.ObjectField("Height map", heightMap, typeof(Texture2D), false);
        mat = (Material)EditorGUILayout.ObjectField("Material", mat, typeof(Material), false);
        size = EditorGUILayout.Vector3Field("Size", size); mname = EditorGUILayout.TextField("Name", mname);
        if (GUILayout.Button("Generate mesh", GUILayout.Height(20)))
            generated = Generate();
        if (GUILayout.Button("Save", GUILayout.Height(20)))
        {
            Mesh generated_mesh = generated.GetComponent<MeshFilter>().sharedMesh;
            CreatePaths(); AssetDatabase.CreateAsset(generated_mesh, "Assets/MeshTools/Meshes/Generated/" + mname + ".asset");
            PrefabUtility.CreatePrefab("Assets/MeshTools/Prefabs/Generated/" + mname + ".prefab", generated);
        }
    }

    GameObject Generate()
    {
        GameObject go = new GameObject(mname);
        go.transform.position = Vector3.zero;
        go.AddComponent<MeshFilter>();
        go.AddComponent<MeshRenderer>();
        
        if (mat != null)
            go.GetComponent<Renderer>().material = mat;
        else
        {
            Debug.LogError("No material attached! Aborting");
            return null;
        }
        
        int width = Cut(heightMap.width);
        int height = Cut(heightMap.height);
        
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[height * width];
        Vector2[] UVs = new Vector2[height * width];
        Vector4[] tangs = new Vector4[height * width];
        Vector2 uvScale = new Vector2(1 / (width - 1), 1 / (height - 1));
        Vector3 sizeScale = new Vector3(size.x / (width - 1), size.y, size.z / (height - 1));
        
        int index;
        float pixelHeight;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                index = y * width + x;
                
                pixelHeight = heightMap.GetPixel(x, y).grayscale;
                Vector3 vertex = new Vector3(x, pixelHeight, y);
                vertices[index] = Vector3.Scale(sizeScale, vertex);
                Vector2 cur_uv = new Vector2(x, y);
                UVs[index] = Vector2.Scale(cur_uv, uvScale);
                
                Vector3 leftV = new Vector3(x - 1, heightMap.GetPixel(x - 1, y).grayscale, y);
                Vector3 rightV = new Vector3(x + 1, heightMap.GetPixel(x + 1, y).grayscale, y);
                Vector3 tang = Vector3.Scale(sizeScale, rightV - leftV).normalized;
                tangs[index] = new Vector4(tang.x, tang.y, tang.z, 1);

            }

        }
        mesh.vertices = vertices;
        mesh.uv = UVs;
        
        index = 0;
        int[] triangles = new int[(height - 1) * (width - 1) * 6];
        for (int y = 0; y < height - 1; y++)
        {
            for (int x = 0; x < width - 1; x++)
            {
                triangles[index++] = (y * width) + x;
                triangles[index++] = ((y + 1) * width) + x;
                triangles[index++] = (y * width) + x + 1;
                triangles[index++] = ((y + 1) * width) + x;
                triangles[index++] = ((y + 1) * width) + x + 1;
                triangles[index++] = (y * width) + x + 1;
            }

        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.tangents = tangs;
        go.GetComponent<MeshFilter>().sharedMesh = mesh;
        return go;
    }
}
