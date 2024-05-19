using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    #region vars

    // >>Prefabs<<
    [SerializeField]
    GameObject tilePrefab;

    // >>Grid Generation<<
    // Dimensions of grid
    const int gridWidth = 15;
    const int gridHeight = 15;
    const int floodedXBand = 4;
    const int floodedYBand = 4;

    // Representations of the grid
    const int xStartPoint = 0;
    const int yStartPoint = 0;
    const float gridTransXDiff = 0.959994f;
    const float gridTransYDiff = 0.54000003f;

    // >>Nutrient Generation<<
    // Kernal size
    const int smoothingKernalSize = 3;
    // Nutrient amount
    const float maxNutrientLevel = 1;
    // Scale of base
    // The lower less percent of the max nutrient the tiles have
    const float ascendPower = 1.01f;
    // Scale of decension
    // The higher the more the more harshley nutrients farther in the kernal are weighed
    [SerializeField]
    const float descendPower = 3.0f;

    // >>Grid Storage<<
    GameObject[][] tileObjectGrid;
    Tile[][] tileScriptGrid;
    #endregion

    #region functions
    //>>General Helper Functions
    // Attempts to gather coordinate and returns null if out of bounds
    T gridGet<T>(int x, int y, T[][] grid, T outOfBoundValue)
    {
        if(x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            return grid[y][x];
        }
        else
        {
            return outOfBoundValue;
        }
    }

    Tile tileGet(int x, int y)
    {
        return gridGet(x, y, tileScriptGrid, null);
    }

    //>>Start<<
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
                Vector3 pos = new Vector3(xStartPoint + gridTransXDiff * (x - (gridWidth - 1) / (float) 2), yStartPoint + gridTransYDiff  * (y - (gridHeight - 1) / (float)2));
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
                        maxNutrientLevel,
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
                        maxNutrientLevel,
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
        // Updates all of the tiles
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                tileScriptGrid[y][x].bayUpdate();
            }
        }
        // Updates nutrition values
        updateNutritionValues();
    }
    //>>Nutrition Gathering<<
    // Converts the distance of a location into a kernal weight
    float distToWeight(int orgX, int orgY, int newX, int newY)
    {
        float xDif = newX - orgX;
        float yDif = newY - orgY;
        return Mathf.Pow(ascendPower, - (Mathf.Pow(xDif,descendPower) + Mathf.Pow(yDif, descendPower)));
    }
    // Takes in surrounding values to average out
    float smoothFloat(int xLoc, int yLoc,int kSize, float[][] givenOriginal)
    {
        ArrayList weightList = new ArrayList();
        // Finds weighted value of every tile in the kernal
        for (int y = yLoc - kSize + 1; y < yLoc + kSize; y++)
        {
            for (int x = xLoc - kSize + 1; x < xLoc + kSize; x++)
            {
                // If it is out of bounds, the tile does not factor into calculation
                float getOrgVal = gridGet<float>(x, y, givenOriginal, (float)-1.0);
                if(getOrgVal >= -0.1)
                {
                    float getWeight = distToWeight(xLoc, yLoc, x, y);
                    weightList.Add(getOrgVal * getWeight);
                }
            }
        }
        if (weightList.Count != 0)
        {
            // Averages out values found in the kernal
            float retVal = 0;
            for (int i = 0; i < weightList.Count; i++)
            {
                retVal += (float)weightList[i];
            }
            retVal = retVal / weightList.Count;
            return retVal;
        }
        else
        {
            return 0;
        }
    }
    // Uses Linear Diffusion Filter to assign new values to each nutrition value
    void updateNutritionValues()
    {
        // Sets up base nutrient values
        float[][] newBaseNutrVals = new float[gridHeight][];
        for (int y = 0; y < gridHeight; y++)
        {
            newBaseNutrVals[y] = new float[gridWidth];
            for (int x = 0; x < gridWidth; x++)
            {
                newBaseNutrVals[y][x] = tileScriptGrid[y][x].baseNutrientCalculation();
            }
        }
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                tileScriptGrid[y][x].updateNutr(smoothFloat(x, y, smoothingKernalSize, newBaseNutrVals));
            }
        }
    }

    //>>Waves<<
    // Gets total non submerged land tiles in order to calculate whether to recede or 
    public int getTotalLandTiles()
    {
        int landIter = 0;
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if(tileScriptGrid[y][x].currentState != Tile.TileState.Submerged)
                {
                    landIter += 1;
                }
            }
        }
        return landIter;
    }
    // Sends a wave in a direction
    public void sendWave(Tile.Direction dir, int coord, int width, bool flood)
    {
        if(dir == Tile.Direction.L)
        {
            for(int i = coord - width; i < coord + width; i++)
            {
                if (i >= 0 && i < gridHeight)
                {
                    tileScriptGrid[i][gridWidth - 1].newWaveAttempt(dir, flood);
                }
            }
        }
        if (dir == Tile.Direction.U)
        {
            for (int i = coord - width; i < coord + width; i++)
            {
                if (i >= 0 && i < gridWidth)
                {
                    tileScriptGrid[0][i].newWaveAttempt(dir, flood);
                }
            }
        }
        if (dir == Tile.Direction.R)
        {
            for (int i = coord - width; i < coord + width; i++)
            {
                if (i >= 0 && i < gridHeight)
                {
                    tileScriptGrid[i][0].newWaveAttempt(dir, flood);
                }
            }
        }
        if (dir == Tile.Direction.D)
        {

        }
    }
    //>>Update<<
    // Update is called once per frame
    void Update()
    {
        // Updates all of the tiles
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                tileScriptGrid[y][x].bayUpdate();
            }
        }
    }
    #endregion
}
