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
    int gridWidth = 10;
    [SerializeField]
    int gridHeight = 7;
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
                Vector3 pos = new Vector3(gridStartPoint.x + gridTransXDiff * x, gridStartPoint.y + gridTransYDiff * y);
                tileObjectGrid[y][x] = Instantiate(tilePrefab);
                tileScriptGrid[y][x] = tileObjectGrid[y][x].GetComponent<Tile>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
