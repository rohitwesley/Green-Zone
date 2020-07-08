using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    
    /// <summary>
    /// ProceduralWall Model
    /// </summary>
    
    [Range(0.01f,10)]
    [SerializeField] private float mapCellScale = 1.0f;
    [Range(1,10)]
    [SerializeField] private int cellsPerPathWidth = 3;
    [Range(1,10)]
    [SerializeField] private int cellsPerWallWidth = 1;
    
    Vector2Int mapDimensions;

    /// <summary>
    /// Initialize the TileMap with starting cell
    /// </summary>
    public void InitilizeMap()
    {
        // Initialise TileMap and status variables
        mapDimensions = new Vector2Int(6,6);
    }
    
    private void Start()
    {
        Debug.Log("Initializing Tile Map");
        InitilizeMap();
    }

    /// <summary>
    /// TileMap View
    /// </summary>

    /// <summary>
    /// Draw TileMap Debuger 
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // Draw Base Axis (Base wall tiles)
        Gizmos.color = Color.red;
        for (int x = 0; x < mapDimensions.x; x++)
        {
            // Draw bottom row wall tiles
            Vector3 position = new Vector3(-mapDimensions.x/2 + x + 0.5f,
                                            0,
                                            -mapDimensions.y/2 - 1 + 0.5f);
            position *= mapCellScale;
            for (int px = 0; px < cellsPerPathWidth+cellsPerWallWidth; px++)
            {
                for (int py = 0; py < cellsPerPathWidth+cellsPerWallWidth; py++)
                {                        
                    // Draw Wall tiles
                    if(px%(cellsPerWallWidth)==cellsPerPathWidth || py%(cellsPerPathWidth+cellsPerWallWidth)==cellsPerPathWidth)
                    {
                        Vector3 pathPosition = new Vector3(px,0,py);    
                        pathPosition *= mapCellScale;
                        pathPosition = position*(cellsPerPathWidth+cellsPerWallWidth)+pathPosition;
                        Gizmos.DrawWireCube(pathPosition, Vector3.one*mapCellScale);
                    }
                }
            }

            // Draw left row wall tiles
            position = new Vector3(-mapDimensions.x/2 - 1 + 0.5f,
                                            0,
                                            -mapDimensions.y/2 + x + 0.5f);
            position *= mapCellScale;
            for (int px = 0; px < cellsPerPathWidth+cellsPerWallWidth; px++)
            {
                for (int py = 0; py < cellsPerPathWidth+cellsPerWallWidth; py++)
                {                        
                    // Draw Wall tiles
                    if(px%(cellsPerPathWidth+cellsPerWallWidth)==cellsPerPathWidth || py%(cellsPerWallWidth)==cellsPerPathWidth)
                    {
                        Vector3 pathPosition = new Vector3(px,0,py);    
                        pathPosition *= mapCellScale;
                        pathPosition = position*(cellsPerPathWidth+cellsPerWallWidth)+pathPosition;
                        Gizmos.DrawWireCube(pathPosition, Vector3.one*mapCellScale);
                    }
                }
            }
        }

        // Draw TileMap
        for (int x = 0; x < mapDimensions.x; x++)
        {
            // Debug.Log("Drawing Maze Row" + x);
            for (int y = 0; y < mapDimensions.y; y++)
            {
                Vector3 position = new Vector3(-mapDimensions.x/2 + x + 0.5f,
                                                0,
                                                -mapDimensions.y/2 + y + 0.5f);
                position *= mapCellScale;
                for (int px = 0; px < cellsPerPathWidth+cellsPerWallWidth; px++)
                {
                    for (int py = 0; py < cellsPerPathWidth+cellsPerWallWidth; py++)
                    {   
                        // Draw path tiles
                        Vector3 pathPosition = new Vector3(px,0,py);
                        pathPosition *= mapCellScale;
                        pathPosition = position*(cellsPerPathWidth+cellsPerWallWidth)+pathPosition;
                        Gizmos.DrawWireCube(pathPosition, Vector3.one*mapCellScale);
                    }
                }

            }
        }

    }

}
