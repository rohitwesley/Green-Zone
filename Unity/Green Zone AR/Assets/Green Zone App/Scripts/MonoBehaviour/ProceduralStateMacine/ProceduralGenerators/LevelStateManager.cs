using GameLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStateManager : MonoBehaviour
{
    /// <summary>
    /// Procedural Level Map Model
    /// </summary>
    /// [Tooltip("reference to the game manager")]
	[SerializeField] GameManager gameManager;
    [SerializeField] private string seed;
    [SerializeField] private bool useRandomSeed;
    [SerializeField] private int smoothStep = 5;
    [SerializeField] private int borderSize = 1;

    [Range(0.01f, 10)]
    [SerializeField] private float mapCellScale = 1.0f;
    [Range(1, 10)]
    [SerializeField] private int cellsPerPathWidth = 3;
    [Range(1, 10)]
    [SerializeField] private int cellsPerWallWidth = 1;
    [Range(0, 100)]
    [SerializeField] private int randomFillPercent;

    [Tooltip("Floor Tile Prefab")]
    [SerializeField] private LevelCell MazeCellPrefab;
    [Tooltip("Path Tile Prefab")]
    [SerializeField] private LevelPath LevelPassagePrefab;
    [Tooltip("Door Tile Prefab")]
    [SerializeField] private LevelDoor LevelDoorPrefab;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float doorProbability = 0.1f;
    [Tooltip("Wall Tile Prefab")]
    [SerializeField] private LevelWall[] MazeWallPrefab;
    [Tooltip("Room Settings")]
    [SerializeField] private LevelRoomSettings[] roomSettings;
    //[Range(1, 100)]
    [SerializeField] private IntVector2 mapDimensions;
    private LevelCell[,] map;
    private List<LevelRoom> rooms = new List<LevelRoom>();    

    [Tooltip("Speed of Path")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float speed = 0.0003f;
    Vector2 currentPositonInMap = new Vector2();
    [Tooltip("Generate Room on Map or just view the map")]
    [SerializeField] private bool spawnRoom = true;
    [Tooltip("Auto Generate map on Start Of scene")]
    [SerializeField] private bool autoSpawn = true;

    private void Start()
    {
        if (autoSpawn)
            GenerateMap();
    }

    public void GenerateMap()
    {
        // walk te map and create level
        Walk();
    }

    /// <summary>
    /// Walk on Tile Map functions
    /// </summary>
    private void Walk()
    {       
        // walk from floor tile
        StartCoroutine(randomWalk());
    }

    private IEnumerator scannRowWalk()
    {
        UnitType[,] mapState = RandomFillMap();
        // smooth borders
        for (int i = 0; i < smoothStep; i++)
        {
            mapState = SmoothMap(mapState);
        }
        Vector2Int mapIndex = new Vector2Int((int)currentPositonInMap.x % mapDimensions.x, (int)currentPositonInMap.y % mapDimensions.z);
        while (IsTileInMap(mapIndex) && mapIndex.x <= mapDimensions.x - 1 && mapIndex.y <= mapDimensions.z - 1)
        { 
            // Every half a sec. create a tile
            yield return new WaitForSeconds(0.01f);
            mapIndex = new Vector2Int((int)currentPositonInMap.x % mapDimensions.x, (int)currentPositonInMap.y % mapDimensions.z);
            // Update the current position on the spline based on the speed and spline segment count
            float speedSpline = (float)mapDimensions.x * speed;
            currentPositonInMap.x += speedSpline + Time.deltaTime;
            if (currentPositonInMap.x > mapDimensions.x - 1)
            {
                currentPositonInMap.x = 0.0f;
                currentPositonInMap.y += 1.0f;
                if (currentPositonInMap.y >= mapDimensions.z)
                {
                    currentPositonInMap.y = 0.0f;
                }
            }
            // TODO set up boundry wall based on cave generator
            if(mapState[mapIndex.x, mapIndex.y] == UnitType.Wall)
            {

            }
        }
    }

    private IEnumerator randomWalk()
    {
        WaitForSeconds delay = new WaitForSeconds(speed);
        map = new LevelCell[mapDimensions.x, mapDimensions.z];
        List<LevelCell> activeCells = new List<LevelCell>();
        DoFirstGenerationStep(activeCells);
        while (activeCells.Count > 0)
        {
            yield return delay;
            DoNextGenerationStep(activeCells);
        }
        yield return delay;
        // scan for a floor tile
        yield return StartCoroutine(scannRowWalk());
        gameManager.StartSequence();
    }

    private void DoFirstGenerationStep(List<LevelCell> activeCells)
    {
        LevelCell newCell = CreateCell(RandomCoordinates);
        newCell.Initialize(CreateRoom(-1));
        activeCells.Add(newCell);
    }

    private void DoNextGenerationStep(List<LevelCell> activeCells)
    {
        int currentIndex = activeCells.Count - 1;
        LevelCell currentCell = activeCells[currentIndex];
        if (currentCell.IsFullyInitialized)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }
        LevelDirection direction = currentCell.RandomUninitializedDirection;
        IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
        if (ContainsCoordinates(coordinates))
        {
            LevelCell neighbor = GetCell(coordinates);
            if (neighbor == null)
            {
                neighbor = CreateCell(coordinates);
                CreatePassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
            else if (currentCell.room.settingsIndex == neighbor.room.settingsIndex)
            {
                CreatePassageInSameRoom(currentCell, neighbor, direction);
            }
            else
            {
                CreateWall(currentCell, neighbor, direction);
            }
        }
        else
        {
            CreateWall(currentCell, null, direction);
        }
    }

    private LevelCell CreateCell(IntVector2 coordinates)
    {
        LevelCell newCell = Instantiate(MazeCellPrefab) as LevelCell;
        map[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.tileState = UnitType.Floor;
        newCell.transform.localPosition = new Vector3(coordinates.x - mapDimensions.x * 0.5f + 0.5f, 0f, coordinates.z - mapDimensions.z * 0.5f + 0.5f);
        return newCell;
    }

    private void CreatePassage(LevelCell cell, LevelCell otherCell, LevelDirection direction)
    {
        LevelPath prefab = Random.value < doorProbability ? LevelDoorPrefab : LevelPassagePrefab;
        LevelPath passage = Instantiate(prefab) as LevelPath;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(prefab) as LevelPath;
        if (passage is LevelDoor)
        {
            otherCell.Initialize(CreateRoom(cell.room.settingsIndex));
        }
        else
        {
            otherCell.Initialize(cell.room);
        }
        passage.Initialize(otherCell, cell, direction.GetOpposite());
        cell.tileState = UnitType.Path;
    }

    private void CreatePassageInSameRoom(LevelCell cell, LevelCell otherCell, LevelDirection direction)
    {
        LevelPath passage = Instantiate(LevelPassagePrefab) as LevelPath;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(LevelPassagePrefab) as LevelPath;
        passage.Initialize(otherCell, cell, direction.GetOpposite());
        if (cell.room != otherCell.room)
        {
            LevelRoom roomToAssimilate = otherCell.room;
            cell.room.Assimilate(roomToAssimilate);
            rooms.Remove(roomToAssimilate);
            Destroy(roomToAssimilate);
        }
        cell.tileState = UnitType.Path;
    }

    private void CreateWall(LevelCell cell, LevelCell otherCell, LevelDirection direction)
    {
        LevelWall wall = Instantiate(MazeWallPrefab[Random.Range(0, MazeWallPrefab.Length)]) as LevelWall;
        wall.Initialize(cell, otherCell, direction);
        if (otherCell != null)
        {
            wall = Instantiate(MazeWallPrefab[Random.Range(0, MazeWallPrefab.Length)]) as LevelWall;
            wall.Initialize(otherCell, cell, direction.GetOpposite());
        }
    }

    private LevelRoom CreateRoom(int indexToExclude)
    {
        LevelRoom newRoom = ScriptableObject.CreateInstance<LevelRoom>();
        newRoom.settingsIndex = Random.Range(0, roomSettings.Length);
        if (newRoom.settingsIndex == indexToExclude)
        {
            newRoom.settingsIndex = (newRoom.settingsIndex + 1) % roomSettings.Length;
        }
        newRoom.settings = roomSettings[newRoom.settingsIndex];
        rooms.Add(newRoom);
        return newRoom;
    }

    public IntVector2 RandomCoordinates
    {
        get
        {
            return new IntVector2(Random.Range(0, mapDimensions.x), Random.Range(0, mapDimensions.z));
        }
    }

    public bool ContainsCoordinates(IntVector2 coordinate)
    {
        return coordinate.x >= 0 && coordinate.x < mapDimensions.x && coordinate.z >= 0 && coordinate.z < mapDimensions.z;
    }

    public LevelCell GetCell(IntVector2 coordinates)
    {
        return map[coordinates.x, coordinates.z];
    }

    /// <summary>
    /// Fill Map functions
    /// </summary>

    private UnitType[,] RandomFillMap()
    {
        UnitType[,] mapState = new UnitType[mapDimensions.x, mapDimensions.z];
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        for (int x = 0; x < mapDimensions.x; x++)
        {
            for (int y = 0; y < mapDimensions.z; y++)
            {
                LevelDirection direction = LevelDirection.North;
                //Map Edge Walls
                if (x == 0 || x == mapDimensions.x - 1 || y == 0 || y == mapDimensions.z - 1)
                {
                    mapState[x, y] = UnitType.Wall;
                    if (x == 0)
                        direction = LevelDirection.East;
                    if (y == 0)
                        direction = LevelDirection.South;
                    if (x == mapDimensions.x - 1)
                        direction = LevelDirection.West;
                    if (y == mapDimensions.z - 1)
                        direction = LevelDirection.North;
                }
                else
                {
                    mapState[x, y] = UnitType.Floor;
                    // Randomise Tile to wall or floor
                    mapState[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? UnitType.Wall : UnitType.Floor;
                }
                if(mapState[x, y] == UnitType.Wall)
                {
                    CreateWall(map[x, y], null, direction);
                }
            }
        }
        return mapState;
    }

    private UnitType[,] SmoothMap(UnitType[,] mapState)
    {
        for (int x = 0; x < mapDimensions.x; x++)
        {
            for (int y = 0; y < mapDimensions.z; y++)
            {
                int neighbourMazeWalls = GetSurroundingAgentCount(new Vector2Int(x,y), UnitType.Wall);

                if (neighbourMazeWalls > 4)
                    mapState[x, y] = UnitType.Wall;
                else if (neighbourMazeWalls < 4)
                    mapState[x, y] = UnitType.Floor;

            }
        }
        return mapState;
    }

    int GetSurroundingAgentCount(Vector2Int index, UnitType agentType)
    {
        int agentCount = 0;
        for (int neighbourX = index.x - 1; neighbourX <= index.x + 1; neighbourX++)
        {
            for (int neighbourY = index.y - 1; neighbourY <= index.y + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < mapDimensions.x && neighbourY >= 0 && neighbourY < mapDimensions.z)
                {
                    // check tiles surounding give tile 
                    if (neighbourX != index.x || neighbourY != index.y)
                    {
                        // if is a wall tile
                        if (map[neighbourX, neighbourY].tileState == agentType) agentCount++;// += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    // if edge of the map
                    agentCount++;
                }
            }
        }

        return agentCount;
    }
    
    /// <summary>
    /// Get random walk direction
    /// </summary>
    /// <returns></returns>
    private LevelDirection RandomDirection()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        return (LevelDirection)pseudoRandom.Next(0, 4);
    }

    /// <summary>
    /// Get Random Walk Direction Index
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private static Vector2Int RandomDirectionIndex(LevelDirection direction)
    {
        Vector2Int[] directionIndex =
        {
            new Vector2Int(0,1),
            new Vector2Int(1,0),
            new Vector2Int(0,-1),
            new Vector2Int(-1,0),
        };
        return directionIndex[(int)direction];
    }

    /// <summary>
    /// Get a random time from the TileMap
    /// </summary>
    private Vector2Int RandomTileIndex
    {
        get
        {
            if (useRandomSeed)
            {
                seed = Time.time.ToString();
            }
            System.Random pseudoRandom = new System.Random(seed.GetHashCode());
            return new Vector2Int(pseudoRandom.Next(0, mapDimensions.x), pseudoRandom.Next(0, mapDimensions.z));
        }
    }

    /// <summary>
    /// Check if TileMap Index is in range
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private bool IsTileInMap(Vector2Int index)
    {
        return (index.x >= 0 && index.x < mapDimensions.x && index.y >= 0 && index.y < mapDimensions.z);
    }

    /// <summary>
    /// Static function to get Opposit Direction
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static LevelDirection GetOpposite(LevelDirection direction)
    {
        return oppositeMazeDirection[(int)direction];
    }

    private static LevelDirection[] oppositeMazeDirection =
    {
        LevelDirection.South,
        LevelDirection.West,
        LevelDirection.North,
        LevelDirection.East
    };

    public static Quaternion GetMazeDirectionToWorldOrientation(LevelDirection direction)
    {
        return MazeDirectionToWorldOrientation[(int)direction];
    }

    private static Quaternion[] MazeDirectionToWorldOrientation =
    {
       Quaternion.identity,
       Quaternion.Euler(0f, 90f, 0f),
       Quaternion.Euler(0f, 180f, 0f),
       Quaternion.Euler(0f, 270f, 0f),
    };

    /// <summary>
    /// Draw TileMap Debuger 
    /// </summary>
    /*private void OnDrawGizmosSelected()
    {
        if (map != null)
        {
            Vector2 mapSmoothIndex = new Vector2(currentPositonInMap.x%(mapDimensions.x), currentPositonInMap.y%mapDimensions.z);
            Vector2Int mapIndex = new Vector2Int((int)currentPositonInMap.x%mapDimensions.x, (int)currentPositonInMap.y%mapDimensions.z);
            Vector3 position = new Vector3(-mapDimensions.z / 2 + mapIndex.x, 0, -mapDimensions.z / 2 + mapIndex.y);
            position *= mapCellScale;

            Gizmos.color = Color.yellow;
            LevelCell currentTile = map[mapIndex.x, mapIndex.y];
            if (currentTile.IsFullyInitialized)
            {
                Gizmos.color = Color.red;
            }
            // Debug.Log("Current Point - mapSmoothIndex: " + mapSmoothIndex + "mapIndex: " + mapIndex + "position: " + position);
            
            Gizmos.DrawCube(position, Vector3.one * mapCellScale);
            // smooth step from one box to the next
            position = new Vector3(-mapDimensions.x / 2 + mapSmoothIndex.x, 0, -mapDimensions.z / 2 + mapSmoothIndex.y);
            position *= mapCellScale;
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(position, Vector3.one * mapCellScale);

            // Draw TileMap
            for (int x = 0; x < mapDimensions.x; x++)
            {
                // Debug.Log("Drawing Maze Row" + x);
                for (int y = 0; y < mapDimensions.z; y++)
                {
                    position = new Vector3(x - mapDimensions.x / 2, 0,y - mapDimensions.z / 2);
                    position *= mapCellScale;

                    if (map[x, y].IsFullyInitialized)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireCube(position, Vector3.one * mapCellScale);
                    }
                    else if(map[x, y].tileState == UnitType.Start)
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawCube(position, Vector3.one * mapCellScale);
                    }
                    else if (map[x, y].tileState == UnitType.Path)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawCube(position, Vector3.one * mapCellScale);
                    }
                    else if (map[x, y].tileState == UnitType.Wall)
                    {
                        Gizmos.color = Color.Lerp(Color.red, Color.yellow, 0.25f);
                        Gizmos.DrawCube(position, Vector3.one * mapCellScale);
                    }
                    else if (map[x, y].tileState == UnitType.Floor)
                    {
                        Gizmos.color = Color.white;
                        Gizmos.DrawWireCube(position, Vector3.one * mapCellScale);
                    }
                    else
                    {
                        Gizmos.color = Color.white;
                        Gizmos.DrawWireCube(position, Vector3.one * mapCellScale);
                    }

                }
            }
        }

    }*/

}




