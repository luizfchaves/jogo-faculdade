using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainData))]
public class TerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TerrainData terrainData = (TerrainData)target;
        if (GUILayout.Button("Generate Terrain"))
        {
            terrainData.GUIGenerate();
        }
    }
}