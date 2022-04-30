using UnityEditor;
using UnityEngine;

public class ParentEditor : EditorWindow
{
    public void CreatePaths()
    {
        if (!AssetDatabase.IsValidFolder("Assets/MeshTools")) AssetDatabase.CreateFolder("Assets", "MeshTools");
        if (!AssetDatabase.IsValidFolder("Assets/MeshTools/Meshes")) AssetDatabase.CreateFolder("Assets/MeshTools", "Meshes");
        if (!AssetDatabase.IsValidFolder("Assets/MeshTools/Meshes/Generated")) AssetDatabase.CreateFolder("Assets/MeshTools/Meshes", "Generated");
        if (!AssetDatabase.IsValidFolder("Assets/MeshTools/Meshes/Updated")) AssetDatabase.CreateFolder("Assets/MeshTools/Meshes", "Updated");
        if (!AssetDatabase.IsValidFolder("Assets/MeshTools/Prefabs")) AssetDatabase.CreateFolder("Assets/MeshTools", "Prefabs");
        if (!AssetDatabase.IsValidFolder("Assets/MeshTools/Prefabs/Generated")) AssetDatabase.CreateFolder("Assets/MeshTools/Prefabs", "Generated");
        if (!AssetDatabase.IsValidFolder("Assets/MeshTools/Prefabs/Updated")) AssetDatabase.CreateFolder("Assets/MeshTools/Prefabs", "Updated");
    }

    public int Cut(int value)
    {
        return Mathf.Min(value, 255);
    }

    public Mesh CopyMesh(Mesh mesh)
    {
        Mesh newmesh = new Mesh();
        newmesh.vertices = mesh.vertices;
        newmesh.triangles = mesh.triangles;
        newmesh.uv = mesh.uv;
        newmesh.normals = mesh.normals;
        newmesh.tangents = mesh.tangents;
        return newmesh;
    }
}

