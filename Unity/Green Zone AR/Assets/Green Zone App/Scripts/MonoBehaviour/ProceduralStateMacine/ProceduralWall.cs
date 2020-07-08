using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralWall : MonoBehaviour
{
    /// <summary>
    /// ProceduralWall Model
    /// </summary>
    
    [Range(0.01f,10)]
    [SerializeField] private float mazeScale = 0.3f;
    [Range(1,10)]
    [SerializeField] private int pathWidth = 3;
    [Range(1,10)]
    [SerializeField] private int wallWidth = 1;
    
    Vector2Int mazeDimensions;
    int noVisitedCells;
    //Track All Cells Status
    CellNeighbours[,] maze;
    //Track All Visited Cells Index
    Stack<Vector2Int> stack;

    enum CellNeighbours
    {
        Cell_Empty,
        Cell_Path_N,
        Cell_Path_S,
        Cell_Path_E,
        Cell_Path_W,
        Cell_Visited,
    }

    /// <summary>
    /// ProceduralWall Controler
    /// </summary>
    
    
    /// <summary>
    /// Initialize the Maze with starting cell
    /// </summary>
    public void InitilizeMaze()
    {
        // Initialise maze and status variables
        noVisitedCells = 0;
        mazeDimensions = new Vector2Int(6,6);//new Vector2Int(40,25);
        maze = new CellNeighbours[mazeDimensions.x, mazeDimensions.y];
        stack = new Stack<Vector2Int>();
        
        // Add 1st cell to the stack
        stack.Push(new Vector2Int(0,0));
        // Update 1st cells status to visited
        maze[0,0] = CellNeighbours.Cell_Visited;
        // Track no of visited cells to know when all cells visited.
        noVisitedCells++;

    }

    /// <summary>
    /// Recursive backtracker Algorithm to travers the whole Maze
    /// https://en.wikipedia.org/wiki/Maze_generation_algorithm#Recursive_backtracker
    /// https://www.youtube.com/watch?v=Y37-gB83HKE
    /// </summary>
    public void WalkMaze()
    {
        // TODO Fix drawing walls and paths
        // Walk only if not all cells are visited and not backtracked to start.
        if(noVisitedCells < mazeDimensions.x * mazeDimensions.y)
        {
            // Step 1 : Create Set of unvisited neighbours.
            Vector2Int currentVisitedCellIndex = stack.Peek();
            
            // Collect all Neighbour States for Current Cell.
            List<Vector2Int> stackNeighbours = new List<Vector2Int>();
            // North Neighbours
            if(currentVisitedCellIndex.y < mazeDimensions.y-1)
            {
                Vector2Int nextVisitedCellIndex = new Vector2Int(currentVisitedCellIndex.x,currentVisitedCellIndex.y+1);
                // Add Neighbour if there is a path or Neighbouring Cell not visited 
                if(maze[nextVisitedCellIndex.x, nextVisitedCellIndex.y] == CellNeighbours.Cell_Empty)
                    stackNeighbours.Add(nextVisitedCellIndex);
            }
            // South Neighbours
            if(currentVisitedCellIndex.y > 0)
            {
                Vector2Int nextVisitedCellIndex = new Vector2Int(currentVisitedCellIndex.x,currentVisitedCellIndex.y-1);
                // Add Neighbour if there is a path or Neighbouring Cell not visited 
                if(maze[nextVisitedCellIndex.x, nextVisitedCellIndex.y] == CellNeighbours.Cell_Empty)
                    stackNeighbours.Add(nextVisitedCellIndex);
            }
            // West Neighbours
            if(currentVisitedCellIndex.x < mazeDimensions.x-1)
            {
                Vector2Int nextVisitedCellIndex = new Vector2Int(currentVisitedCellIndex.x+1,currentVisitedCellIndex.y);
                // Add Neighbour if there is a path or Neighbouring Cell not visited 
                if(maze[nextVisitedCellIndex.x, nextVisitedCellIndex.y] == CellNeighbours.Cell_Empty)
                    stackNeighbours.Add(nextVisitedCellIndex);
            }
            // East Neighbours
            if(currentVisitedCellIndex.x > 0)
            {
                Vector2Int nextVisitedCellIndex = new Vector2Int(currentVisitedCellIndex.x-1,currentVisitedCellIndex.y);
                // Add Neighbour if there is a path or Neighbouring Cell not visited 
                if(maze[nextVisitedCellIndex.x, nextVisitedCellIndex.y] == CellNeighbours.Cell_Empty)
                    stackNeighbours.Add(nextVisitedCellIndex);
            }

            
            // Are there any neighbours ?
            if(stackNeighbours.Count>0)
            {
                // Choose a random direction from the unvisited neighbours to go to
                int nextCellDirection = Random.Range(0, stackNeighbours.Count);
                switch (nextCellDirection)
                {
                    // Move to North Cell
                    case 0 :
                            maze[currentVisitedCellIndex.x ,currentVisitedCellIndex.y] = CellNeighbours.Cell_Path_N;
                            break;
                    // Move to South Cell
                    case 1 :
                            maze[currentVisitedCellIndex.x ,currentVisitedCellIndex.y] = CellNeighbours.Cell_Path_S;
                            break;
                    // Move to West Cell
                    case 2 :
                            maze[currentVisitedCellIndex.x ,currentVisitedCellIndex.y] = CellNeighbours.Cell_Path_W;
                            break;
                    // Move to East Cell
                    case 3 :
                            maze[currentVisitedCellIndex.x ,currentVisitedCellIndex.y] = CellNeighbours.Cell_Path_E;
                            break;
                }

                // Debug.Log("New Cell: " + stack.Peek().x + "," + stack.Peek().y);
                // new cell
                // Add next cell to the stack
                stack.Push(stackNeighbours[nextCellDirection]);
                // Update next cells status to visited
                maze[stackNeighbours[nextCellDirection].x ,stackNeighbours[nextCellDirection].y] = CellNeighbours.Cell_Visited;
                // Track no of visited cells to know when all cells visited.
                noVisitedCells++;
            }
            else
            {
                // Backtrack
                stack.Pop();
            }
            

        }
    }

    /// <summary>
    /// ProceduralWall View
    /// </summary>
    
    private void Start()
    {
        Debug.Log("Initializing Maze.");
        InitilizeMaze();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        WalkMaze();
    }

    /// <summary>
    /// Draw maze 
    /// </summary>
    // public void DrawMaze()
    private void OnDrawGizmos()
    {
        // Draw Base Axis (Base wall tiles)
        Gizmos.color = Color.red;
        for (int x = 0; x < mazeDimensions.x; x++)
        {
            // Draw bottom row wall tiles
            Vector3 position = new Vector3(-mazeDimensions.x/2 + x + 0.5f,
                                            0,
                                            -mazeDimensions.y/2 - 1 + 0.5f);
            position *= mazeScale;
            for (int px = 0; px < pathWidth+wallWidth; px++)
            {
                for (int py = 0; py < pathWidth+wallWidth; py++)
                {                        
                    // Draw Wall tiles
                    if(px%(wallWidth)==pathWidth || py%(pathWidth+wallWidth)==pathWidth)
                    {
                        Vector3 pathPosition = new Vector3(px,0,py);    
                        pathPosition *= mazeScale;
                        pathPosition = position*(pathWidth+wallWidth)+pathPosition;
                        Gizmos.DrawCube(pathPosition, Vector3.one*mazeScale);
                    }
                }
            }

            // Draw left row wall tiles
            position = new Vector3(-mazeDimensions.x/2 - 1 + 0.5f,
                                            0,
                                            -mazeDimensions.y/2 + x + 0.5f);
            position *= mazeScale;
            for (int px = 0; px < pathWidth+wallWidth; px++)
            {
                for (int py = 0; py < pathWidth+wallWidth; py++)
                {                        
                    // Draw Wall tiles
                    if(px%(pathWidth+wallWidth)==pathWidth || py%(wallWidth)==pathWidth)
                    {
                        Vector3 pathPosition = new Vector3(px,0,py);    
                        pathPosition *= mazeScale;
                        pathPosition = position*(pathWidth+wallWidth)+pathPosition;
                        Gizmos.DrawCube(pathPosition, Vector3.one*mazeScale);
                    }
                }
            }
        }

        // Draw Maze
        for (int x = 0; x < mazeDimensions.x; x++)
        {
            // Debug.Log("Drawing Maze Row" + x);
            for (int y = 0; y < mazeDimensions.y; y++)
            {
                Vector3 position = new Vector3(-mazeDimensions.x/2 + x + 0.5f,
                                                0,
                                                -mazeDimensions.y/2 + y + 0.5f);
                position *= mazeScale;
                for (int px = 0; px < pathWidth+wallWidth; px++)
                {
                    for (int py = 0; py < pathWidth+wallWidth; py++)
                    {
                        // Set Current Visited Cell
                        if(maze[x,y] == CellNeighbours.Cell_Visited)
                        {
                            Gizmos.color = Color.blue;
                        }
                        // Set a Visited Cell
                        else if(maze[x,y] != CellNeighbours.Cell_Empty)
                        {
                            Gizmos.color = Color.gray;
                        }
                        // Set Empty Cell
                        else 
                        {
                            Gizmos.color = Color.white;
                        }
                        
                        // Set Wall Tile 
                        if(px%(pathWidth+wallWidth)==pathWidth || py%(pathWidth+wallWidth)==pathWidth)
                        {
                            Gizmos.color = Color.black;

                            // Set North Path Tile
                            if((maze[x,y] == CellNeighbours.Cell_Path_N)
                                && px%(pathWidth+wallWidth)!=pathWidth 
                                && py%(pathWidth+wallWidth)==pathWidth)
                            {
                                Gizmos.color = Color.green;
                                Gizmos.color = Color.gray;
                            }
                            // Set South Path Tile
                            else if((maze[x,y] == CellNeighbours.Cell_Path_S)
                                && px%(pathWidth+wallWidth)!=pathWidth 
                                && py%(pathWidth+wallWidth)==pathWidth)
                            {
                                Gizmos.color = Color.red;
                                Gizmos.color = Color.gray;
                            }
                            // Set East Path Tile
                            else if((maze[x,y] == CellNeighbours.Cell_Path_E)
                                && px%(pathWidth+wallWidth)==pathWidth 
                                && py%(pathWidth+wallWidth)!=pathWidth)
                            {
                                Gizmos.color = Color.yellow;
                                Gizmos.color = Color.gray;
                            } 
                            // Set West Path Tile
                            else if((maze[x,y] == CellNeighbours.Cell_Path_W)
                                && px%(pathWidth+wallWidth)==pathWidth 
                                && py%(pathWidth+wallWidth)!=pathWidth)
                            {
                                Gizmos.color = Color.yellow;
                                Gizmos.color = Color.gray;
                            }   
                        }
                        
                        // Draw path tiles
                        Vector3 pathPosition = new Vector3(px,0,py);
                        pathPosition *= mazeScale;
                        pathPosition = position*(pathWidth+wallWidth)+pathPosition;
                        Gizmos.DrawCube(pathPosition, Vector3.one*mazeScale);
                    }
                }

            }
        }

    }


}
