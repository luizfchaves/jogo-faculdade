using UnityEngine;
using Unity.AI.Navigation;

public class TerrainSpawner : MonoBehaviour {

  public GameObject middleTerrain;

  public void calculateNavMesh(){
    Debug.Log("Calculating NavMesh for middle terrain...");
    // get navmesh from middle and calculate
    NavMeshSurface surface = middleTerrain.GetComponent<NavMeshSurface>();
    if (surface == null) {
        Debug.LogError("No NavMeshSurface found on middleTerrain!");
        return;
    }
    Debug.Log("NavMeshSurface found, building NavMesh...");
    surface.BuildNavMesh();
  }
}