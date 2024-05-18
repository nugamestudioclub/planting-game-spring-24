using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLoader : MonoBehaviour
{
    #region vars

    // >>Prefabs<<
    [SerializeField]
    GameObject tilePrefab;

    // >>Grid Generation<<
    // Dimensions of grid
    [SerializeField]
    int gridWidth = 15;
    [SerializeField]
    int gridHeight = 10;
    [SerializeField]
    int floodedXBand;
    [SerializeField]
    int floodedYBand;

    // Representations of the grid
    [SerializeField]
    Vector2 gridStartPoint;
    [SerializeField]
    float gridTransXDiff;
    [SerializeField]
    float gridTransYDiff;
    // >>Grid Storage<<
    GameObject[][] tileObjectGrid;
    Tile[][] tileScriptGrid;

    #endregion

    // Attempts to gather coordinate and returns null if out of bounds
    T gridGet<T>(int x, int y, T[][] grid) where T : class
    {
        if(x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            return grid[y][x];
        }
        else
        {
            return null;
        }
    }

    Tile tileGet(int x, int y)
    {
        return gridGet(x, y, tileScriptGrid);
    }
    // Start is called before the first frame update
    void Start()
    {
        // Loads in arrays
        tileObjectGrid = new GameObject[gridHeight][];
        tileScriptGrid = new Tile[gridHeight][];
        for(int y = 0; y < gridHeight; y++)
        {
            tileObjectGrid[y] = new GameObject[gridWidth];
            tileScriptGrid[y] = new Tile[gridWidth];
            for(int x = 0; x < gridWidth; x++)
            {
                Vector3 pos = new Vector3(gridStartPoint.x + gridTransXDiff * (x - (gridWidth - 1) / (float) 2), gridStartPoint.y + gridTransYDiff  * (y - (gridHeight - 1) / (float)2));
                tileObjectGrid[y][x] = Instantiate(tilePrefab, pos, Quaternion.identity);
                tileScriptGrid[y][x] = tileObjectGrid[y][x].GetComponent<Tile>();
            }
        }
        // Activates all of the tiles
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if( x < floodedXBand || x >= gridWidth - floodedXBand || y < floodedYBand || y >= gridHeight - floodedYBand)
                {
                    tileScriptGrid[y][x].activate(
                        Tile.TileState.Submerged,
                        tileGet(x - 1, y - 1),
                        tileGet(x - 1, y),
                        tileGet(x - 1, y + 1),
                        tileGet(x, y + 1),
                        tileGet(x + 1, y + 1),
                        tileGet(x + 1, y),
                        tileGet(x + 1, y - 1),
                        tileGet(x, y - 1)
                        );
                }
                else
                {
                    tileScriptGrid[y][x].activate(
                        Tile.TileState.Default,
                        tileGet(x - 1, y - 1),
                        tileGet(x - 1, y),
                        tileGet(x - 1, y + 1),
                        tileGet(x, y + 1),
                        tileGet(x + 1, y + 1),
                        tileGet(x + 1, y),
                        tileGet(x + 1, y - 1),
                        tileGet(x, y - 1)
                        );
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
