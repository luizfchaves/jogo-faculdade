using System.Collections;
using UnityEngine;
using Unity.AI.Navigation;
using System.Collections.Generic;

enum BlockType {
  Nothing,
  BottomToTop,
  LeftToRight,
  BottomToLeft,
  LeftToTop,
  TopToRight,
  RightToBottom,
  StartPosition
}
class Block {
  public GameObject prefab;
  public int[,] positions;
  public BlockType name;

  public Block(GameObject prefab) {
    this.prefab = prefab;
  }

  public void makePrefab(BlockType name) {
    // melhorar essa lógica depois
    this.name = name;
    switch (name) {
      case BlockType.Nothing:
        positions = new int[,]{
          {0,0,0},
          {0,0,0},
          {0,0,0}
        };
        break;

      case BlockType.BottomToTop:
        positions = new int[,]{
          {0,1,0},
          {0,1,0},
          {0,1,0}
        };
        break;
      case BlockType.LeftToRight:
        positions = new int[,]{
          {0,0,0},
          {1,1,1},
          {0,0,0}
        };
        break;

      case BlockType.BottomToLeft:
        positions = new int[,]{
          {0,0,0},
          {1,1,0},
          {0,1,0}
        };
        break;
      case BlockType.LeftToTop:
        positions = new int[,]{
          {0,1,0},
          {1,1,0},
          {0,0,0}
        };
        break;
      case BlockType.TopToRight:
        positions = new int[,]{
          {0,1,0},
          {0,1,1},
          {0,0,0}
        };
        break;
      case BlockType.RightToBottom:
        positions = new int[,]{
          {0,0,0},
          {0,1,1},
          {0,1,0}
        };
        break;
      case BlockType.StartPosition:
        positions = new int[,]{
          {0,1,0},
          {0,1,0},
          {0,0,0}
        };
        break;
    }
  }
  public GameObject instantitePrefab(Vector3 position){
    GameObject block = GameObject.Instantiate(prefab, position, Quaternion.identity);

    switch (name) {
      case BlockType.Nothing:
        block.transform.Rotate(0, 0, 0);
        break;
      case BlockType.BottomToTop:
        block.transform.Rotate(0, 90, 0);
        break;
      case BlockType.LeftToRight:
        block.transform.Rotate(0, 0, 0);
        break;
      case BlockType.BottomToLeft:
        block.transform.Rotate(0, 270, 0);
        break;
      case BlockType.LeftToTop:
        block.transform.Rotate(0, 0, 0);
        break;
      case BlockType.TopToRight:
        block.transform.Rotate(0, 90, 0);
        break;
      case BlockType.RightToBottom:
        block.transform.Rotate(0, 180, 0);
        break;
      case BlockType.StartPosition:
        block.transform.Rotate(0, 270, 0);
        break;
    }

    return block;
  }
  public bool checkIfNeighborIsValid(string position, Block block) {
    // Aqui basicamente vai verificar se o bloco pode ser colocado na posição
    // top tem que ser calcular se o bottom dele vai com o top desse
    // int[,] positions = block.positions;
    int[,] positions = new int[,]{
      {0,0,0},
      {0,0,0},
      {0,0,0}
    };

    if(block != null){
      positions = block.positions;
    }

    bool isValid = true;
    switch(position){
      case "TOP":
        if(
          positions[2,0] != this.positions[0,0] ||
          positions[2,1] != this.positions[0,1] ||
          positions[2,2] != this.positions[0,2]
        ){
          isValid = false;
        }
        break;
      case "BOTTOM":
        if(
          positions[0,0] != this.positions[2,0] ||
          positions[0,1] != this.positions[2,1] ||
          positions[0,2] != this.positions[2,2]
        ){
          isValid = false;
        }
        break;
      case "LEFT":
        if(
          positions[0,2] != this.positions[0,0] ||
          positions[1,2] != this.positions[1,0] ||
          positions[2,2] != this.positions[2,0]
        ){
          isValid = false;
        }
        break;
      case "RIGHT":
        if(
          positions[0,0] != this.positions[0,2] ||
          positions[1,0] != this.positions[1,2] ||
          positions[2,0] != this.positions[2,2]
        ){
          isValid = false;
        }
        break;
    }
      return isValid;
  }
}


public class TerrainData : MonoBehaviour {
  public static int mapSize = 7;
  private int roadCount = ((mapSize * mapSize )/ 4)* 3;

  int gap = 6;

  List<Block>[,] blocks;

  public GameObject BottomToTopPrefab;
  public GameObject BottomToLeftPrefab;
  public GameObject NothingPrefab;
  public GameObject StartPosition;

  public GameObject CastlePrefab;


  Block[] possibleBlocks;

  int seed;

  System.Random seededRandom; 

  void generatePossibleBlocks() {
    possibleBlocks = new Block[8];

    // [0,0,0]
    // [0,0,0]
    // [0,0,0]
    Block nothing = new Block(NothingPrefab);
    nothing.makePrefab(BlockType.Nothing);
    possibleBlocks[0] = nothing;

    // [0,1,0]
    // [0,1,0]
    // [0,1,0]
    Block bottomToTop = new Block(BottomToTopPrefab);
    bottomToTop.makePrefab(BlockType.BottomToTop);
    possibleBlocks[1] = bottomToTop;

    // [0,0,0]
    // [1,1,1]
    // [0,0,0]
    Block leftToRight = new Block(BottomToTopPrefab);
    leftToRight.makePrefab(BlockType.LeftToRight);
    possibleBlocks[2] = leftToRight;

    // [0,0,0]
    // [1,1,0]
    // [0,1,0]
    Block bottomToLeft = new Block(BottomToLeftPrefab);
    bottomToLeft.makePrefab(BlockType.BottomToLeft);
    possibleBlocks[3] = bottomToLeft;

    // [0,1,0]
    // [1,1,0]
    // [0,0,0]
    Block leftToTop = new Block(BottomToLeftPrefab);
    leftToTop.makePrefab(BlockType.LeftToTop);
    possibleBlocks[4] = leftToTop;

    // [0,1,0]
    // [0,1,1]
    // [0,0,0]
    Block topToRight = new Block(BottomToLeftPrefab);
    topToRight.makePrefab(BlockType.TopToRight);
    possibleBlocks[5] = topToRight;

    // [0,0,0]
    // [0,1,1]
    // [0,1,0]
    Block rightToBottom = new Block(BottomToLeftPrefab);
    rightToBottom.makePrefab(BlockType.RightToBottom);
    possibleBlocks[6] = rightToBottom;

    // [0,1,0]
    // [0,1,0]
    // [0,0,0]
    Block startPosition = new Block(StartPosition);
    startPosition.makePrefab(BlockType.StartPosition);
    possibleBlocks[7] = startPosition;
  }  

  void startGameObjects(){
    blocks = new List<Block>[mapSize, mapSize];

    for (int i = 0; i < mapSize; i++) {
      for (int j = 0; j < mapSize; j++) {
        blocks[i, j] = new List<Block>();
        for(int k = 0; k < possibleBlocks.Length-1; k++){
        // Create a new Block instance for each cell
        Block original = possibleBlocks[k];
        Block copy = new Block(original.prefab);
        copy.makePrefab(original.name);
        blocks[i, j].Add(copy);
      }
      }
    }

  }

  public void propagateBlocks(int x, int y, bool forcePropagation = false){
    if(x < 0 || x >= mapSize || y < 0 || y >= mapSize) {
      return;
    }
    bool thisBlockChanged = false;

    // Removendo os blocos inválidos de cima
    if(y+1 < mapSize){
      for(int i = 0; i < blocks[x,y+1].Count; i++){
        Block currentNeighborBlock = blocks[x,y+1][i];
        bool isValidWithAtLeastOne = false;

        for(int j = 0; j < blocks[x,y].Count; j++){
          Block currentBlock = blocks[x,y][j];
          if(currentBlock.checkIfNeighborIsValid("TOP", currentNeighborBlock)){
            isValidWithAtLeastOne = true;
            break;
          }
        }

        if(!isValidWithAtLeastOne){
          thisBlockChanged = true;
          blocks[x,y+1].Remove(currentNeighborBlock);
          i--;
        }
      }
    }

    // Removendo os blocos inválidos de baixo
    if(y-1 >= 0){
      for(int i = 0; i < blocks[x,y-1].Count; i++){
        Block currentNeighborBlock = blocks[x,y-1][i];
        bool isValidWithAtLeastOne = false;

        for(int j = 0; j < blocks[x,y].Count; j++){
          Block currentBlock = blocks[x,y][j];
          if(currentBlock.checkIfNeighborIsValid("BOTTOM", currentNeighborBlock)){
            isValidWithAtLeastOne = true;
            break;
          }
        }

        if(!isValidWithAtLeastOne){
          thisBlockChanged = true;
          blocks[x,y-1].Remove(currentNeighborBlock);
          i--;
        }
      }
    }

    // Removendo os blocos inválidos da esquerda
    if(x-1 >= 0){
      for(int i = 0; i < blocks[x-1,y].Count; i++){
        Block currentNeighborBlock = blocks[x-1,y][i];
        bool isValidWithAtLeastOne = false;

        for(int j = 0; j < blocks[x,y].Count; j++){
          Block currentBlock = blocks[x,y][j];
          if(currentBlock.checkIfNeighborIsValid("LEFT", currentNeighborBlock)){
            isValidWithAtLeastOne = true;
            break;
          }
        }

        if(!isValidWithAtLeastOne){
          thisBlockChanged = true;
          blocks[x-1,y].Remove(currentNeighborBlock);
          i--;
        }
      }
    }

    // Removendo os blocos inválidos da direita
    if(x+1 < mapSize){
      for(int i = 0; i < blocks[x+1,y].Count; i++){
        Block currentNeighborBlock = blocks[x+1,y][i];
        bool isValidWithAtLeastOne = false;

        for(int j = 0; j < blocks[x,y].Count; j++){
          Block currentBlock = blocks[x,y][j];
          if(currentBlock.checkIfNeighborIsValid("RIGHT", currentNeighborBlock)){
            isValidWithAtLeastOne = true;
            break;
          }
        }

        if(!isValidWithAtLeastOne){
          thisBlockChanged = true;
          blocks[x+1,y].Remove(currentNeighborBlock);
          i--;
        }
      }
    }

    if(thisBlockChanged || forcePropagation){
      propagateBlocks(x+1, y);
      propagateBlocks(x-1, y);
      propagateBlocks(x, y+1);
      propagateBlocks(x, y-1);
    }
  }

  public void removeEdgeBlocks(){
    for (int x = 0; x < mapSize; x++) {
      for(int y = 0; y < mapSize; y++){
        if(x == 0 || x == mapSize-1 || y == 0 || y == mapSize-1){
          for(int blockIndex = 0; blockIndex < blocks[x,y].Count; blockIndex++){
            Block currentBlock = blocks[x,y][blockIndex];


            if(x == 0){
              bool isValid = currentBlock.checkIfNeighborIsValid("LEFT", null);
              if(!isValid){
                blocks[x,y].Remove(currentBlock);
                blockIndex--;
              }
            }
            if(x == mapSize-1){
              bool isValid = currentBlock.checkIfNeighborIsValid("RIGHT", null);
              if(!isValid){
                blocks[x,y].Remove(currentBlock);
                blockIndex--;
              }
            }
            if(y == 0){
              bool isValid = currentBlock.checkIfNeighborIsValid("BOTTOM", null);
              if(!isValid){
                blocks[x,y].Remove(currentBlock);
                blockIndex--;
              }
            }
            if(y == mapSize-1){
              bool isValid = currentBlock.checkIfNeighborIsValid("TOP", null);
              if(!isValid){
                blocks[x,y].Remove(currentBlock);
                blockIndex--;
              }
            }
          }
        }
      }
    }
  }

  public bool generateRandomBlockAndPropagate(int x, int y){
    if(x < 0 || x >= mapSize || y < 0 || y >= mapSize) {
      return false;
    }

    if(blocks[x,y].Count == 0){
      return false;
    }


    int randomIndex = seededRandom.Next(0, blocks[x, y].Count);
    // Debug.Log("Random index: " + randomIndex);
  
    Block randomBlock = blocks[x, y][randomIndex];
    blocks[x,y] = new List<Block>{randomBlock};
  
    propagateBlocks(x,y);
    return true;
  }

  public int startBlocks(){
    startGameObjects();

    int startX = seededRandom.Next(0,mapSize);
    Block startBlock = new Block(possibleBlocks[(int)BlockType.StartPosition].prefab);
    startBlock.makePrefab(BlockType.StartPosition);
    blocks[startX,0] = new List<Block>{startBlock};

    removeEdgeBlocks();
    propagateBlocks(startX,0);

    return startX;
  }

  public int[,] getLastPlaceRecursive(int prevX,int prevY, int currentX, int currentY){
    int nextX = currentX;
    int nextY = currentY;

    int[,] positions = blocks[currentX, currentY][0].positions;
    Debug.Log("Validating if postion: " + currentX + "," + currentY + " is empty");
    // if every position is 0, return prevX, prevY
    if(positions[0,0] == 0 && positions[0,1] == 0 && positions[0,2] == 0 &&
       positions[1,0] == 0 && positions[1,1] == 0 && positions[1,2] == 0 &&
       positions[2,0] == 0 && positions[2,1] == 0 && positions[2,2] == 0){
      Debug.Log("Position: " + currentX + "," + currentY + " is empty!!");
      return new int[2,2]{
        {-1,-1},
        {-1,-1}
      };
    }

    Debug.Log("Checking wich direction is the next position");

    if (positions[0, 1] == 1 && currentY + 1 != prevY && currentY + 1 < mapSize){
      nextY = currentY + 1; // cima
    } else if (positions[2, 1] == 1 && currentY - 1 != prevY && currentY - 1 >= 0) {
      nextY = currentY - 1; // baixo
    } else if (positions[1, 2] == 1 && currentX + 1 != prevX && currentX + 1 < mapSize) {
      nextX = currentX + 1; // direita
    } else if (positions[1, 0] == 1 && currentX - 1 != prevX && currentX - 1 >= 0) {
      nextX = currentX - 1; // esquerda
    }

    if (nextX == currentX && nextY == currentY) {
      return new int[2,2]{
        {prevX, prevY},
        {currentX, currentY},
      };
    } else {
      int[,] lastPlace = getLastPlaceRecursive(currentX, currentY, nextX, nextY);
      if(lastPlace[0,0] != -1 && lastPlace[0,1] != -1){
        return lastPlace;
      } else {
        return new int[2,2]{
          {prevX, prevY},
          {currentX, currentY},
        };
      }
    }
 }

  public int[] getTowerPosition(int prevX, int prevY, int currentX, int currentY){
    int [,] positions = blocks[currentX, currentY][0].positions;

    if(positions[0,1] == 1 && currentY + 1 != prevY && currentY + 1 < mapSize){
      return new int[2]{0,2};
    } else if (positions[2,1] == 1 && currentY - 1 != prevY && currentY - 1 >= 0) {
      return new int[2]{0,-2};
    } else if (positions[1,2] == 1 && currentX + 1 != prevX && currentX + 1 < mapSize) {
      return new int[2]{2,0};
    } else if (positions[1,0] == 1 && currentX - 1 != prevX && currentX - 1 >= 0) {
      return new int[2]{-2,0};
    }

    Debug.LogError("Algo bizarro aconteceu [getTowerPosition]");
    return new int[2]{-1,-1};
  }
  
  [ContextMenu("Generate Terrain")]
  public void generate() {

    seededRandom = new System.Random(seed);
    Debug.Log("Seed: " + seed);
    generatePossibleBlocks();
    // get every child of this object and destroy it
    foreach (Transform child in transform) {
      GameObject.Destroy(child.gameObject);
    }

    int startX;


    // int currentNextX;
    // int currentNextY;
    int tilesCreated = 0;

    while(true){
      Debug.Log("Generating terrain...");
      startX = startBlocks();

      int prevX = startX;
      int prevY = 0;
      int currentX = startX;
      int currentY = 1;

 
      tilesCreated = 0;
      int count = roadCount;

      while(count > 0){

        // very fast sleep
        System.Threading.Thread.Sleep(1);

        if(currentX < 0 || currentY >= mapSize || currentX < 0 || currentY >= mapSize) {
          break;
        }

        tilesCreated++;
        generateRandomBlockAndPropagate(currentX, currentY);
        if (blocks[currentX, currentY].Count == 0) {
          break;
        }

        int[,] positions = blocks[currentX, currentY][0].positions;

        int nextX = currentX;
        int nextY = currentY;

        // Decide próxima direção (prioridade: cima, baixo, direita, esquerda)
        if (positions[0, 1] == 1 && currentY + 1 != prevY && currentY + 1 < mapSize){
          nextY = currentY + 1; // cima
        } else if (positions[2, 1] == 1 && currentY - 1 != prevY && currentY - 1 >= 0) {
          nextY = currentY - 1; // baixo
        } else if (positions[1, 2] == 1 && currentX + 1 != prevX && currentX + 1 < mapSize) {
          nextX = currentX + 1; // direita
        } else if (positions[1, 0] == 1 && currentX - 1 != prevX && currentX - 1 >= 0) {
          nextX = currentX - 1; // esquerda
        } else {
          // Não há caminho válido, encerra
          break;
        }

        // Atualiza a posição atual
        prevY = currentY;
        prevX = currentX;
        currentX = nextX;
        currentY = nextY;

        count--;
      }

      if(count == 0){
        break;
      }
    }

    for(int x=0; x < mapSize; x++){
      for(int y=0; y < mapSize; y++){
        if(blocks[x,y].Count > 1){
          Block nothingBlock = new Block(possibleBlocks[(int)BlockType.Nothing].prefab);
          nothingBlock.makePrefab(BlockType.Nothing);

          blocks[x,y] = new List<Block>{nothingBlock};
        }
      }
    }
    


    int[,] lastPlace = getLastPlaceRecursive(-1,-1, startX, 0);
    Debug.Log("Last place: " + lastPlace[1,0] + "," + lastPlace[1,1]);
    int[] towerPosition = getTowerPosition(lastPlace[0,0], lastPlace[0,1], lastPlace[1,0], lastPlace[1,1]);

    Debug.Log("Tower position: " + towerPosition[0] + "," + towerPosition[1]);
    Debug.Log("Blocks generated:" + tilesCreated);
    int castleX = lastPlace[1,0] * gap;
    int castleY = lastPlace[1,1] * gap;

    int placeX = castleX + towerPosition[0];
    int placeY = castleY + towerPosition[1];




    GameObject castle = GameObject.Instantiate(CastlePrefab, new Vector3(placeX, 1, placeY), Quaternion.identity);
    castle.transform.parent = transform;


    for (int i = 0; i < mapSize; i++) {
      for (int j = 0; j < mapSize; j++) {
        for(int k=0; k < blocks[i,j].Count; k++){
          Block currentBlock = blocks[i,j][k];
          GameObject block = currentBlock.instantitePrefab(new Vector3(i * gap, k*4, j * gap));
          block.name = "Block_" + i + "_" + j + "_" + currentBlock.name;
          block.transform.parent = transform;
        }
      }
    }

    // get TerrainSpanwer class from start and call calculate function
    TerrainSpawner terrainSpawner = GetComponentInChildren<TerrainSpawner>();
    terrainSpawner.calculateNavMesh();
  }  

  public void GUIGenerate() {
    Debug.Log("Button clicked!");
    generate();
  }

  // void Awake() {
  // }

  void Start() {
    Debug.Log("Chaning seed to: " + System.DateTime.Now.Millisecond);
    seed = System.DateTime.Now.Millisecond;
    generate();
  }

}