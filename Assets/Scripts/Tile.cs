using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region vars
    // >>State of tile<<
    public enum TileState
    {
        Default,
        Submerged,
        Planted
    }

    // >>Check if tile has been loaded<<
    bool active = false;

    public TileState currentState;

    // >>Nutrient Levels<<
    public float maxNutr;
    // Actual value 
    public float nutritionLevels = 1;

    // Surrounding tiles
    public Tile leftDownTile;
    public Tile leftTile;
    public Tile leftUpTile;
    public Tile upTile;
    public Tile rightUpTile;
    public Tile rightTile;
    public Tile rightDownTile;
    public Tile downTile;
    #endregion
    // Fills in information of tile and activates it
    public void activate(float maxNutrInsert, TileState setState, Tile LD, Tile L, Tile LU, Tile U, Tile RU, Tile R, Tile RD, Tile D)
    {
        maxNutr = maxNutrInsert;
        active = true;
        currentState = setState;
        leftDownTile = LD;
        leftTile = L;
        leftUpTile = LU;
        upTile = U;
        rightUpTile = RU;
        rightTile = R;
        rightDownTile = RD;
        downTile = D;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float baseNutrientCalculation()
    {
        if(currentState == TileState.Submerged)
        {
            return maxNutr;
        }
        else
        {
            int subOrMissingEdge = 0;
            void addOnTileSubOrMissing(Tile givenTile)
            {
                if (givenTile == null || givenTile.currentState == TileState.Submerged)
                {
                    subOrMissingEdge += 1;
                }
            }
            addOnTileSubOrMissing(leftDownTile);
            addOnTileSubOrMissing(leftTile);
            addOnTileSubOrMissing(leftUpTile);
            addOnTileSubOrMissing(upTile);
            addOnTileSubOrMissing(rightUpTile);
            addOnTileSubOrMissing(rightTile);
            addOnTileSubOrMissing(rightDownTile);
            addOnTileSubOrMissing(downTile);
            return  maxNutr * ((float)subOrMissingEdge / 8);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // DEBUG CODE
        if (currentState == TileState.Default)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.yellow * (nutritionLevels / maxNutr);
        }
        if (currentState == TileState.Planted)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
        if (currentState == TileState.Submerged)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }
}
