using System.Collections;
using UnityEngine;

[System.Serializable]
public class TerrainTypeRow {
  public TerrainData.TerrainType[] row;
}
[System.Serializable]
public class GameObjectRow {
  public GameObject[] row;
}

public class TerrainData : MonoBehaviour {

  public enum TerrainType {
    Walkable,
    Grass,
    Water
  }

  [SerializeField]
  public TerrainTypeRow[]  terrainTypes = new TerrainTypeRow[3] {
    new TerrainTypeRow { row = new TerrainType[3] { TerrainType.Grass, TerrainType.Walkable, TerrainType.Grass } },
    new TerrainTypeRow { row = new TerrainType[3] { TerrainType.Grass, TerrainType.Walkable, TerrainType.Grass } },
    new TerrainTypeRow { row = new TerrainType[3] { TerrainType.Grass, TerrainType.Walkable, TerrainType.Grass } },
  };

  [SerializeField]
  public GameObjectRow[] gameObjects = new GameObjectRow[3] {
    new GameObjectRow { row = new GameObject[3] { null, null, null } },
    new GameObjectRow { row = new GameObject[3] { null, null, null } },
    new GameObjectRow { row = new GameObject[3] { null, null, null } },
  };

  public Material WalkableMaterial;
  public Material GrassMaterial;
  public Material WaterMaterial;

  public void generate() {
  }  

}