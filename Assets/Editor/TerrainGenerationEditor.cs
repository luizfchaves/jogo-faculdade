using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGeneration))]
public class TerrainGenerationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TerrainGeneration terrain = (TerrainGeneration)target;

        if(GUILayout.Button("Generate Terrain"))
        {
            terrain.GenerateTerrain();
        }
    }
}